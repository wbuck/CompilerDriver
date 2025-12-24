using System.Diagnostics;
using System.Diagnostics.Contracts;
using Compiler.Common.Helpers;
using Compiler.Parser.Nodes;
using Compiler.Tacky.Helpers;
using Compiler.Tacky.Tac;

namespace Compiler.Tacky;

public class TackyVisitor
{
    private readonly LabelGenerator _labelGenerator = new();
    
    public TackyProgram Visit(ProgramNode program)
    {
        var funcDefinitions = program.Functions
            .Where(f => f.Body is not null)
            .ToArray();
        
        return new TackyProgram(funcDefinitions.Select(VisitFunction).ToList());
    }

    private TackyFunction VisitFunction(FunctionDeclarationNode node)
    {
        var instructions = VisitBlock(node.Body!, [], new VariableFactory());        
        instructions.Add(new TackyReturn(new TackyConstant<int>(0)));
        return new TackyFunction(node.Name, node.Parameters, instructions);
    }

    private List<ITackyInstruction> VisitBlock(
        BlockNode block, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        foreach (var item in block.Items)
        {
            switch (item)
            {
                case VariableDeclarationNode node:
                    VisitDeclaration(node, instructions, factory);
                    break;
                case IStatementNode node:
                    VisitStatement(node, instructions, factory);
                    break;
                case FunctionDeclarationNode:
                    // We don't care about function declarations in blocks.'
                    break;
                default:
                    throw new UnreachableException($"Unknown block item type: {item.Tag.ToStringFast()}");
            }
        }
        return instructions;
    }

    private List<ITackyInstruction> VisitDeclaration(
        VariableDeclarationNode declaration, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        if (declaration is not { Initializer: { } rhs }) 
            return instructions;
        
        var lhs = new VariableNode(declaration.Identifier);
        VisitAssignment(lhs, rhs, instructions, factory);
        return instructions;
    }

    private List<ITackyInstruction> VisitStatement(
        IStatementNode statement, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        switch (statement)
        {
            case ReturnNode node:
                return VisitReturn(node, instructions, factory);
            case IfNode node:
                return VisitIf(node, instructions, factory);
            case ExpressionNode node:
                // We don't care about the result of the expression statement
                // in this case.
                _ = VisitExpression(node.Expression, instructions, factory);
                return instructions;
            case NullNode:
                return instructions;
            case LabelNode node:
                return VisitLabel(node, instructions, factory);
            case GotoNode node:
                return VisitGoto(node, instructions);
            case CompoundNode { Block: { } block }:
                return VisitBlock(block, instructions, factory);
            case BreakNode node:
                return VisitBreak(node, instructions);
            case ContinueNode node:
                return VisitContinue(node, instructions);
            case WhileNode node:
                return VisitWhile(node, instructions, factory);
            case DoWhileNode node:
                return VisitDoWhile(node, instructions, factory);
            case ForNode node:
                return VisitFor(node, instructions, factory);
            case SwitchNode node:
                return VisitSwitch(node, instructions, factory);
            case CaseNode node:
                return VisitCase(node, instructions, factory);
            case DefaultNode node:
                return VisitDefault(node, instructions, factory);
            default:
                throw new UnreachableException($"Unknown statement type: {statement.Tag.ToStringFast()}");
        }
    }
    
    private List<ITackyInstruction> VisitDefault(
        DefaultNode node, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        Debug.Assert(node.Label is not null);
        instructions.Add(new TackyLabel(node.Label!));
        
        VisitStatement(node.Statement, instructions, factory);
        
        return instructions;
    }

    private List<ITackyInstruction> VisitCase(
        CaseNode node, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        Debug.Assert(node.Label is not null);
        instructions.Add(new TackyLabel(node.Label!));
        
        VisitStatement(node.Statement, instructions, factory);
        
        return instructions;
    }

    private List<ITackyInstruction> VisitSwitch(
        SwitchNode node, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        Debug.Assert(node.Label is not null); 
                
        if (node is { Cases: { } cases })
        {
            var dest = factory.GetNextVariable();           
            var rhs = VisitExpression(node.Value, instructions, factory);

            foreach (var @case in cases
                         .Where(c => !c.Label.EndsWith("default"))
                         .OrderByDescending(c => c.CalculatedValue))
            {
                var equal = new TackyBinary
                (
                    TackyEqual.Operator,
                    new TackyConstant<int>(@case.CalculatedValue!.Value),
                    rhs,
                    dest
                );
                instructions.Add(equal);
                instructions.Add(new TackyJumpIfNotZero(dest, @case.Label));
            }

            if (cases.FirstOrDefault(c => c.Label.EndsWith("default")) is { Label: not null } @default)            
                instructions.Add(new TackyJump(@default.Label));     
            
            instructions.Add(new TackyJump(GetBreakLabel(node.Label)));
                
        }
        VisitStatement(node.Body, instructions, factory);
        instructions.Add(new TackyLabel(GetBreakLabel(node.Label)));
        
        return instructions;
    }

    private static List<ITackyInstruction> VisitBreak(BreakNode node, List<ITackyInstruction> instructions)
    {
        Debug.Assert(node.Label is not null);
        instructions.Add(new TackyJump(GetBreakLabel(node.Label!)));
        return instructions;
    }
    
    private static List<ITackyInstruction> VisitContinue(ContinueNode node, List<ITackyInstruction> instructions)
    {
        Debug.Assert(node.Label is not null);
        instructions.Add(new TackyJump(GetContinueLabel(node.Label!)));
        return instructions;
    }

    private List<ITackyInstruction> VisitForInit(
        IForLoopInitializer? init, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        switch (init)
        {
            case VariableDeclarationNode declaration:
                return VisitDeclaration(declaration, instructions, factory);
            case IExpressionNode expression:
                // We can ignore the return value here as it's not used in the
                // for loop initializer.
                VisitExpression(expression, instructions, factory);
                return instructions;
            case null:
                return instructions;
            default:
                throw new UnreachableException($"Unknown init type: {init.GetType().Name}");
        }
    }
    
    private List<ITackyInstruction> VisitFor(
        ForNode node, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        Debug.Assert(node.Label is not null);  
        
        var continueLabel = GetContinueLabel(node.Label!);
        var breakLabel = GetBreakLabel(node.Label);
        var beginLabel = BeginLabel(node.Label);
        
        VisitForInit(node.Initializer, instructions, factory);
        instructions.Add(new TackyLabel(beginLabel));

        if (node is { Condition: { } condition })
        {
            var result = VisitExpression(condition, instructions, factory);
            instructions.Add(new TackyJumpIfZero(result, breakLabel));
        }
        
        VisitStatement(node.Body, instructions, factory);
        instructions.Add(new TackyLabel(continueLabel));

        if (node is { Post: { } post })
        {
            // We can ignore the return value here as it's not used
            // in the for loop post-expression.
            VisitExpression(post, instructions, factory);            
        }
        instructions.Add(new TackyJump(beginLabel));
        instructions.Add(new TackyLabel(breakLabel));
        
        return instructions;
    }
    
    private List<ITackyInstruction> VisitDoWhile(
        DoWhileNode node, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        Debug.Assert(node.Label is not null);
        
        var continueLabel = GetContinueLabel(node.Label!);
        var breakLabel = GetBreakLabel(node.Label);
        var beginLabel = BeginLabel(node.Label);
        
        instructions.Add(new TackyLabel(beginLabel));
        VisitStatement(node.Body, instructions, factory);        
        instructions.Add(new TackyLabel(continueLabel));
        
        var condition = VisitExpression(node.Condition, instructions, factory);        
        instructions.Add(new TackyJumpIfNotZero(condition, beginLabel));
        
        instructions.Add(new TackyLabel(breakLabel));
        
        return instructions;
    }

    private List<ITackyInstruction> VisitWhile(
        WhileNode node, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        Debug.Assert(node.Label is not null);
        var continueLabel = GetContinueLabel(node.Label!);
        var breakLabel = GetBreakLabel(node.Label);
        
        instructions.Add(new TackyLabel(continueLabel));
        
        var condition = VisitExpression(node.Condition, instructions, factory);        
        instructions.Add(new TackyJumpIfZero(condition,  breakLabel));

        VisitStatement(node.Body, instructions, factory);
        
        instructions.Add(new TackyJump(continueLabel));
        instructions.Add(new TackyLabel(breakLabel));
        
        return instructions;
    }

    private static List<ITackyInstruction> VisitGoto(GotoNode @goto, List<ITackyInstruction> instructions)
    {
        instructions.Add(new TackyJump(@goto.Label));
        return instructions;
    }

    private List<ITackyInstruction> VisitLabel(LabelNode label, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        instructions.Add(new TackyLabel(label.Name));
        return VisitStatement(label.Statement, instructions, factory);
    }

    private List<ITackyInstruction> VisitReturn(ReturnNode @return, List<ITackyInstruction> instructions, VariableFactory factory)
    {        
        instructions.Add(new TackyReturn(VisitExpression(@return.Expression, instructions, factory)));
        return instructions;
    }
    
    private ITackyValue VisitExpression(
        IExpressionNode expression, in List<ITackyInstruction> instructions, VariableFactory factory)
        => expression switch
        {
            IConstantNode constant => VisitConstant(constant),
            UnaryNode unary => VisitUnary(unary, instructions, factory),            
            BitwiseNode bitwise => VisitBitwise(bitwise, instructions, factory),
            BinaryNode binary => VisitBinary(binary, instructions, factory),
            VariableNode variable => factory.GetNextVariable(variable.Identifier),
            ConditionalNode conditional => VisitConditional(conditional, instructions, factory),
            IAssignmentNode { IsCompound: false } assignment => VisitAssignment(assignment, instructions, factory),
            IAssignmentNode { IsCompound: true } assignment => VisitCompoundAssignment(assignment, instructions, factory),
            FunctionCallNode functionCall => VisitFunctionCall(functionCall, instructions, factory),
            _ => throw new FormatException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private TackyVariable VisitFunctionCall(
        FunctionCallNode node, 
        List<ITackyInstruction> instructions,
        VariableFactory factory)
    {
        var args = node.Args
            .Select(a => VisitExpression(a, instructions, factory))
            .ToList();

        var dest = factory.GetNextVariable();
        instructions.Add(new TackyFunctionCall(node.Identifier, args, dest));
        return dest;
    }

    private TackyVariable VisitConditional(
        ConditionalNode conditional, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var elseLabel = _labelGenerator.GetNextLabel(TackyConstants.CONDITION_ELSE_LABEL);
        var endLabel =  _labelGenerator.GetNextLabel(TackyConstants.CONDITION_END_LABEL);
        var dest = factory.GetNextVariable();
        
        var condition = VisitExpression(conditional.Condition, instructions, factory); 
        
        instructions.Add(new TackyJumpIfZero(condition, elseLabel));       
        instructions.Add(new TackyCopy
        (
            VisitExpression(conditional.True, instructions, factory), 
            dest
        ));        
        instructions.Add(new TackyJump(endLabel));
        
        instructions.Add(new TackyLabel(elseLabel));                   
        instructions.Add(new TackyCopy
        (
            VisitExpression(conditional.False, instructions, factory), 
            dest
        ));
        
        instructions.Add(new TackyLabel(endLabel));
        return dest;
    }

    private TackyVariable VisitAssignment(
        IAssignmentNode assignment, List<ITackyInstruction> instructions, VariableFactory factory)
        => VisitAssignment(assignment.Lhs, assignment.Rhs, instructions, factory);

    private TackyVariable VisitCompoundAssignment(IAssignmentNode assignment, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var left = VisitExpression(assignment.Lhs, instructions, factory);
        var right = VisitExpression(assignment.Rhs, instructions, factory);
        
        // This should never happen as the semantic analysis stage should
        // have handled this already.
        Debug.Assert(left is TackyVariable, $"Invalid assignment type: {left.Tag.ToStringFast()}");
 
        var variable = (TackyVariable)left;
        if (ToTackyBinaryOperator(assignment) is { } binary)
        {
            instructions.Add(new TackyBinary(binary, variable, right, variable));
            return variable;
        }

        if (ToTackyBitwiseOperator(assignment) is not { } bitwise)
            throw new FormatException($"Unknown compound assignment operator: {assignment.Tag.ToStringFast()}");
        
        instructions.Add(new TackyBitwise(bitwise, variable, right, variable));
        return variable;

    }

    private TackyVariable VisitAssignment(
        IExpressionNode lhs, IExpressionNode rhs, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var left = VisitExpression(lhs, instructions, factory);
        var right = VisitExpression(rhs, instructions, factory);        
        
        // This should never happen as the semantic analysis stage should
        // have handled this already.
        Debug.Assert(left is TackyVariable, $"Invalid assignment type: {left.Tag.ToStringFast()}");
        
        var variable = (TackyVariable)left;
        instructions.Add(new TackyCopy(right, variable));
        return variable;
    }

    private TackyVariable VisitBitwise(
        BitwiseNode bitwise, in List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var lhs = VisitExpression(bitwise.Lhs, instructions, factory);
        var rhs = VisitExpression(bitwise.Rhs, instructions, factory);
        var dest = factory.GetNextVariable();
        
        instructions.Add(new TackyBitwise(GetBitwiseOperator(bitwise), lhs, rhs, dest));
        return dest;
      
    }
    
    private TackyVariable VisitBinaryLogicalOr(
        BinaryNode binary, in List<ITackyInstruction> instructions, VariableFactory factory)
    {                        
        var trueLabel = _labelGenerator.GetNextLabel(TackyConstants.OR_WHEN_NOT_ZERO_LABEL);        
        
        instructions.Add(new TackyJumpIfNotZero(VisitExpression(binary.Lhs, instructions, factory), trueLabel));
        instructions.Add(new TackyJumpIfNotZero(VisitExpression(binary.Rhs, instructions, factory), trueLabel));
        
        var endLabel = _labelGenerator.GetNextLabel(TackyConstants.OR_END_LABEL);  
        var result = factory.GetNextVariable();
        
        instructions.Add(new TackyCopy(ITackyValue.False, result));
        instructions.Add(new TackyJump(endLabel));
                      
        instructions.Add(new TackyLabel(trueLabel));                       
        instructions.Add(new TackyCopy(ITackyValue.True, result));  
        instructions.Add(new TackyLabel(endLabel));       
        
        return result;
    }
    
    private List<ITackyInstruction> VisitIf(IfNode @if, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        if (@if.Else is not null)
            return VisitIfElse(@if, instructions, factory);
        
        var endLabel = _labelGenerator.GetNextLabel(TackyConstants.IF_END_LABEL);
        var condition = VisitExpression(@if.Condition, instructions, factory);
        instructions.Add(new TackyJumpIfZero(condition, endLabel));
        instructions.AddRange(VisitStatement(@if.Then, [], factory));
        instructions.Add(new TackyLabel(endLabel));
        return instructions;
    }
    
    private List<ITackyInstruction> VisitIfElse(IfNode @if, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var endLabel = _labelGenerator.GetNextLabel(TackyConstants.IF_END_LABEL);
        var elseLabel = _labelGenerator.GetNextLabel(TackyConstants.ELSE_LABEL);
        var condition = VisitExpression(@if.Condition, instructions, factory);
        instructions.Add(new TackyJumpIfZero(condition, elseLabel));
        instructions.AddRange(VisitStatement(@if.Then, [], factory));
        instructions.Add(new TackyJump(endLabel));
        instructions.Add(new TackyLabel(elseLabel));
        Debug.Assert(@if.Else is not null);
        instructions.AddRange(VisitStatement(@if.Else!, [], factory));
        instructions.Add(new TackyLabel(endLabel));
        return instructions;
    }

    private TackyVariable VisitBinaryLogicalAnd(
        BinaryNode binary, in List<ITackyInstruction> instructions, VariableFactory factory)
    {                        
        var falseLabel = _labelGenerator.GetNextLabel(TackyConstants.AND_WHEN_ZERO_LABEL);        
        
        instructions.Add(new TackyJumpIfZero(VisitExpression(binary.Lhs, instructions, factory), falseLabel));
        instructions.Add(new TackyJumpIfZero(VisitExpression(binary.Rhs, instructions, factory), falseLabel));
        
        var endLabel = _labelGenerator.GetNextLabel(TackyConstants.AND_END_LABEL);  
        var result = factory.GetNextVariable();
        
        instructions.Add(new TackyCopy(ITackyValue.True, result));
        instructions.Add(new TackyJump(endLabel));
                      
        instructions.Add(new TackyLabel(falseLabel));                       
        instructions.Add(new TackyCopy(ITackyValue.False, result));  
        instructions.Add(new TackyLabel(endLabel));       
        
        return result;
    }
    
    private TackyVariable VisitBinary(BinaryNode binary, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        return binary switch
        {
            { Operator: LogicalAndNode } => VisitBinaryLogicalAnd(binary, instructions, factory),
            { Operator: LogicalOrNode } => VisitBinaryLogicalOr(binary, instructions, factory),
            _ => VisitBinaryInternal()
        };
        
        TackyVariable VisitBinaryInternal()
        {
            var lhs = VisitExpression(binary.Lhs, instructions, factory);
            var rhs = VisitExpression(binary.Rhs, instructions, factory);    
            var dest = factory.GetNextVariable();
        
            instructions.Add(new TackyBinary(GetBinaryOperator(binary), lhs, rhs, dest));
            return dest;
        }
    }
    
    private TackyVariable VisitUnary(UnaryNode unary, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var source = VisitExpression(unary.Expression, instructions, factory);
        if (unary is { Operator: PrefixIncrementNode or PrefixDecrementNode })
        {
            instructions.Add(new TackyBinary(GetBinaryOperator(unary.Operator), source, ITackyValue.One, source));
            return (TackyVariable)source;
        }
        
        var dest = factory.GetNextVariable();
        if (unary is { Operator: PostfixIncrementNode or PostfixDecrementNode })
        {
            instructions.Add(new TackyCopy(source, dest));
            instructions.Add(new TackyBinary(GetBinaryOperator(unary.Operator), source, ITackyValue.One, source));
            return dest;
        }
        
        instructions.Add(new TackyUnary(GetUnaryOperator(unary), source, dest));
        return dest; 
    }
    
    private static ITackyBinaryOperator GetBinaryOperator(IUnaryOperatorNode op)
        => op switch
        {
            PrefixIncrementNode or PostfixIncrementNode => TackyAddition.Operator,
            PrefixDecrementNode or PostfixDecrementNode => TackySubtraction.Operator,
            _ => throw new UnreachableException($"Invalid unary operator {op.Tag.ToStringFast()}")
        };
    
    private static ITackyUnaryOperator GetUnaryOperator(UnaryNode unary)
        => unary.Operator switch
        {
            ComplementNode => TackyComplement.Operator,
            NegateNode => TackyNegate.Operator,
            NotNode => TackyNot.Operator,
            _ => throw new FormatException($"Unknown unary operator: {unary.Operator.Tag.ToStringFast()}")
        };
    
    private static ITackyBitwiseOperator? ToTackyBitwiseOperator(IAssignmentNode node)
        => node switch
        {
            BitwiseAndAssignmentNode => TackyBitwiseAnd.Operator,
            BitwiseOrAssignmentNode => TackyBitwiseOr.Operator,
            BitwiseXorAssignmentNode => TackyBitwiseXor.Operator,
            LeftShiftAssignmentNode => TackyLeftShift.Operator,
            RightShiftAssignmentNode => TackyRightShift.Operator,
            _ => null
        };

    private static ITackyBinaryOperator? ToTackyBinaryOperator(IAssignmentNode node)
        => node switch
        {
            AdditionAssignmentNode => TackyAddition.Operator,
            SubtractionAssignmentNode => TackySubtraction.Operator,
            MultiplicationAssignmentNode => TackyMultiplication.Operator,
            DivisionAssignmentNode => TackyDivision.Operator,
            RemainderAssignmentNode => TackyRemainder.Operator,
            _ => null
        };
    
    private static ITackyBinaryOperator GetBinaryOperator(BinaryNode binary)
        => binary.Operator switch
        {
            AdditionNode => TackyAddition.Operator,
            SubtractionNode => TackySubtraction.Operator,
            MultiplicationNode => TackyMultiplication.Operator,
            DivisionNode => TackyDivision.Operator,
            RemainderNode => TackyRemainder.Operator,
            EqualNode => TackyEqual.Operator,
            NotEqualNode => TackyNotEqual.Operator,
            LessThanNode => TackyLessThan.Operator,
            LessThanOrEqualNode => TackyLessThanOrEqual.Operator,
            GreaterThanNode => TackyGreaterThan.Operator,
            GreaterThanOrEqualNode => TackyGreaterThanOrEqual.Operator,
            _ => throw new FormatException($"Unknown binary operator: {binary.Operator.Tag.ToStringFast()}")
        };
    
    private static ITackyBitwiseOperator GetBitwiseOperator(BitwiseNode bitwise)
        => bitwise.Operator switch
        {
            BitwiseAndNode => TackyBitwiseAnd.Operator,
            BitwiseOrNode => TackyBitwiseOr.Operator,
            BitwiseXorNode => TackyBitwiseXor.Operator,
            BitwiseLeftShiftNode => TackyLeftShift.Operator,
            BitwiseRightShiftNode => TackyRightShift.Operator,
            _ => throw new FormatException($"Unknown bitwise operator: {bitwise.Operator.Tag.ToStringFast()}")
        };

    private static ITackyValue VisitConstant(IConstantNode constant)
        => constant switch
        {
            ConstantNode<int> integer => new TackyConstant<int>(integer.Value),
            ConstantNode<double> floating => new TackyConstant<double>(floating.Value),
            _ => throw new FormatException($"Unknown constant node type: {constant.Tag.ToStringFast()}")
        }; 
    
    [Pure]
    private static string GetBreakLabel(string label) 
        => $".break{label}";
    
    [Pure]
    private static string GetContinueLabel(string label)
        => $".continue{label}";
    
    [Pure]
    private static string BeginLabel(string label)
        => $".begin{label}";
}