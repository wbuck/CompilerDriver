using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Compiler.Common.Ast;
using Compiler.Common.Extensions;

namespace Compiler.Common.Stages;

public class SemanticValidator
{
    private int _variableCount;

    public static bool TryValidate(ProgramNode program, [NotNullWhen(true)] out ProgramNode? analyzed)
    {
        try
        {
            var analyzer = new SemanticValidator();       
            analyzed = analyzer.Validate(program);
            return true;           
        }
        catch (FormatException ex)
        {
            PrintError(ex.Message);
        }
        analyzed = null;
        return false;       
    }
    
    public ProgramNode Validate(ProgramNode program) => 
        new(Function: ValidateFunction(program.Function));    

    private FunctionNode ValidateFunction(FunctionNode function)
    => function with { Body = ResolveBlock(function.Body, [], 0) };

    private BlockNode ResolveBlock(BlockNode block, Dictionary<Original, Mangled> variables, int depth)
        => new([
            ..block.Items.Select(i => i switch
            {
                DeclarationNode declaration => ResolveDeclaration(declaration, variables, depth),
                IStatementNode statement => ResolveStatement(statement, variables, depth),
                _ => throw new UnreachableException($"Unknown block item: {i.Tag.ToStringFast()}")
            })
        ]);

    private IBlockItem ResolveDeclaration(
        DeclarationNode declaration, 
        Dictionary<Original, Mangled> variables,
        int depth)
    {
        var id = new Original(declaration.Identifier, depth);
        if (variables.TryGetValue(id, out var mangled) && mangled.FromCurrentBlock)
            throw new FormatException($"Duplicate variable declaration: {declaration.Identifier}");
        
        var name = variables.GetOrAdd
        (
            id, 
            key => new Mangled(MangleIdentifier(key.Name), true)
        );
        
        var initializer = declaration.Initializer is { } init
            ? ResolveExpression(init, variables, depth)
            : null;
        
        return new DeclarationNode(name, initializer);
    }

    private IExpressionNode ResolveExpression(IExpressionNode expression, Dictionary<Original, Mangled> variables, int depth)
        => expression switch
        {
            IAssignmentNode assignment => ResolveAssignment(assignment, variables, depth),
            UnaryNode unary => ResolveUnary(unary, variables, depth),
            BinaryNode binary => ResolveBinary(binary, variables, depth),
            BitwiseNode bitwise => ResolveBitwise(bitwise, variables, depth),
            VariableNode variable => ResolveVariable(variable, variables, depth),
            ConditionalNode conditional => ResolveConditional(conditional, variables, depth),
            IConstantNode constant => constant,            
            _ => throw new UnreachableException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private ConditionalNode ResolveConditional(ConditionalNode conditional, Dictionary<Original, Mangled> variables, int depth) 
        => new
        (
            ResolveExpression(conditional.Condition, variables, depth), 
            ResolveExpression(conditional.True, variables, depth), 
            ResolveExpression(conditional.False, variables, depth)
        );

    private static VariableNode ResolveVariable(
        VariableNode variable, 
        Dictionary<Original, Mangled> variables,
        int depth)
    {
        while (depth >= 0)
        {
            // If needed check outer scope(s) to find the matching variable.
            if (variables.TryGetValue(new Original(variable.Identifier, depth--), out var mangled))
                return new VariableNode(mangled);
        }
        throw new FormatException($"Undeclared variable: {variable.Identifier}");
    }
    
    private string MangleIdentifier(string identifier)
        => $"{identifier}.{_variableCount++}";

    private BitwiseNode ResolveBitwise(BitwiseNode bitwise, Dictionary<Original, Mangled> variables, int depth)
        => new
           (
               bitwise.Operator, 
               ResolveExpression(bitwise.Lhs, variables, depth), 
               ResolveExpression(bitwise.Rhs,  variables, depth)
           );
    
    private BinaryNode ResolveBinary(BinaryNode binary,  Dictionary<Original, Mangled> variables, int depth)
        => new
           (
               binary.Operator, 
               ResolveExpression(binary.Lhs, variables, depth), 
               ResolveExpression(binary.Rhs, variables, depth)
           );

    private UnaryNode ResolveUnary(UnaryNode unary,  Dictionary<Original, Mangled> variables, int depth)
    {
        if (IsIncrement(unary.Operator) && unary.Expression is not VariableNode)        
            throw new FormatException("An lvalue is required as increment operand");
        
        if (IsDecrement(unary.Operator) && unary.Expression is not VariableNode)        
            throw new FormatException("An lvalue is required as decrement operand");
        
        return unary with { Expression = ResolveExpression(unary.Expression, variables, depth) };

        static bool IsIncrement(IUnaryOperatorNode op) =>
            op is PrefixIncrementNode or PostfixIncrementNode;
        
        static bool IsDecrement(IUnaryOperatorNode op) =>
            op is PrefixDecrementNode or PostfixDecrementNode;
    }

    private IAssignmentNode ResolveAssignment(IAssignmentNode assignment,  Dictionary<Original, Mangled> variables, int depth)
    {
        if (assignment.Lhs is not VariableNode)
            throw new FormatException("Expression must be modifiable lvalue");

        return assignment switch
        {
            AssignmentNode => new AssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            AdditionAssignmentNode => new AdditionAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            SubtractionAssignmentNode => new SubtractionAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            MultiplicationAssignmentNode => new MultiplicationAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            DivisionAssignmentNode => new DivisionAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            RemainderAssignmentNode => new RemainderAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            BitwiseAndAssignmentNode => new BitwiseAndAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            BitwiseOrAssignmentNode => new BitwiseOrAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            BitwiseXorAssignmentNode => new BitwiseXorAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            LeftShiftAssignmentNode => new LeftShiftAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            RightShiftAssignmentNode => new RightShiftAssignmentNode(ResolveExpression(assignment.Lhs, variables, depth), ResolveExpression(assignment.Rhs, variables, depth)),
            _ => throw new UnreachableException($"Unknown assignment type: {assignment.Tag.ToStringFast()}")
        };
    }

    [return: NotNullIfNotNull(nameof(statement))]
    private IStatementNode? ResolveStatement(IStatementNode? statement, Dictionary<Original, Mangled> variables, int depth)
        => statement switch
        {
            ReturnNode ret => new ReturnNode(ResolveExpression(ret.Expression, variables, depth)),
            ExpressionNode exp => new ExpressionNode(ResolveExpression(exp.Expression, variables, depth)),
            IfNode @if => new IfNode(
                ResolveExpression(@if.Condition, variables, depth), 
                ResolveStatement(@if.Then, variables, depth), 
                ResolveStatement(@if.Else, variables, depth)),
            LabelNode label => label with { Statement = ResolveStatement(label.Statement, variables, depth) },
            CompoundNode compound => ResolveCompound(compound, variables, depth),
            GotoNode @goto => @goto,
            NullNode @null => @null,
            null => null,
            _ => throw new UnreachableException($"Unknow statement type: {statement.Tag.ToStringFast()}")
        };
    
    private CompoundNode ResolveCompound(CompoundNode compound, Dictionary<Original, Mangled> variables, int depth)
        => new(ResolveBlock(compound.Block, Duplicate(variables), depth + 1));

    // Create a copy of the variables lookup table for
    // each new block scope that's entered.
    private static Dictionary<Original, Mangled> Duplicate(in Dictionary<Original, Mangled> variables)
        => variables.ToDictionary(kvp => kvp.Key, kvp => kvp.Value with { FromCurrentBlock = false });

    
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }

    private readonly record struct Original(string Name, int Depth);

    private readonly record struct Mangled(string Name, bool FromCurrentBlock)
    {
        public static implicit operator string(Mangled mangled) => mangled.Name;
    }
}