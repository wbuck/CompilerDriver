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

    private BlockNode ResolveBlock(BlockNode block, Dictionary<Original, Mangled> variables, int scope)
        => new([
            ..block.Items.Select(i => i switch
            {
                DeclarationNode declaration => (IBlockItem)ResolveDeclaration(declaration, variables, scope),
                IStatementNode statement => ResolveStatement(statement, variables, scope),
                _ => throw new UnreachableException($"Unknown block item: {i.Tag.ToStringFast()}")
            })
        ]);

    private DeclarationNode ResolveDeclaration(
        DeclarationNode declaration, 
        Dictionary<Original, Mangled> variables,
        int scope)
    {
        var id = new Original(declaration.Identifier, scope);
        if (variables.TryGetValue(id, out var mangled) && mangled.FromCurrentBlock)
            throw new FormatException($"Duplicate variable declaration: {declaration.Identifier}");
        
        var name = variables.GetOrAdd
        (
            id, 
            key => new Mangled(MangleIdentifier(key.Name), true)
        );
        
        var initializer = declaration.Initializer is { } init
            ? ResolveExpression(init, variables, scope)
            : null;
        
        return new DeclarationNode(name, initializer);
    }

    private IExpressionNode ResolveExpression(IExpressionNode expression, Dictionary<Original, Mangled> variables, int scope)
        => expression switch
        {
            IAssignmentNode assignment => ResolveAssignment(assignment, variables, scope),
            UnaryNode unary => ResolveUnary(unary, variables, scope),
            BinaryNode binary => ResolveBinary(binary, variables, scope),
            BitwiseNode bitwise => ResolveBitwise(bitwise, variables, scope),
            VariableNode variable => ResolveVariable(variable, variables, scope),
            ConditionalNode conditional => ResolveConditional(conditional, variables, scope),
            IConstantNode constant => constant,            
            _ => throw new UnreachableException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private ConditionalNode ResolveConditional(ConditionalNode conditional, Dictionary<Original, Mangled> variables, int scope) 
        => new
        (
            ResolveExpression(conditional.Condition, variables, scope), 
            ResolveExpression(conditional.True, variables, scope), 
            ResolveExpression(conditional.False, variables, scope)
        );

    private static VariableNode ResolveVariable(
        VariableNode variable, 
        Dictionary<Original, Mangled> variables,
        int scope)
    {
        while (scope >= 0)
        {
            // If needed check outer scope(s) to find the matching variable.
            if (variables.TryGetValue(new Original(variable.Identifier, scope--), out var mangled))
                return new VariableNode(mangled);
        }
        throw new FormatException($"Undeclared variable: {variable.Identifier}");
    }
    
    private string MangleIdentifier(string identifier)
        => $"{identifier}.{_variableCount++}";

    private BitwiseNode ResolveBitwise(BitwiseNode bitwise, Dictionary<Original, Mangled> variables, int scope)
        => new
           (
               bitwise.Operator, 
               ResolveExpression(bitwise.Lhs, variables, scope), 
               ResolveExpression(bitwise.Rhs,  variables, scope)
           );
    
    private BinaryNode ResolveBinary(BinaryNode binary,  Dictionary<Original, Mangled> variables, int scope)
        => new
           (
               binary.Operator, 
               ResolveExpression(binary.Lhs, variables, scope), 
               ResolveExpression(binary.Rhs, variables, scope)
           );

    private UnaryNode ResolveUnary(UnaryNode unary,  Dictionary<Original, Mangled> variables, int scope)
    {
        if (IsIncrement(unary.Operator) && unary.Expression is not VariableNode)        
            throw new FormatException("An lvalue is required as increment operand");
        
        if (IsDecrement(unary.Operator) && unary.Expression is not VariableNode)        
            throw new FormatException("An lvalue is required as decrement operand");
        
        return unary with { Expression = ResolveExpression(unary.Expression, variables, scope) };

        static bool IsIncrement(IUnaryOperatorNode op) =>
            op is PrefixIncrementNode or PostfixIncrementNode;
        
        static bool IsDecrement(IUnaryOperatorNode op) =>
            op is PrefixDecrementNode or PostfixDecrementNode;
    }

    private IAssignmentNode ResolveAssignment(IAssignmentNode assignment,  Dictionary<Original, Mangled> variables, int scope)
    {
        if (assignment.Lhs is not VariableNode)
            throw new FormatException("Expression must be modifiable lvalue");

        return assignment switch
        {
            AssignmentNode => new AssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            AdditionAssignmentNode => new AdditionAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            SubtractionAssignmentNode => new SubtractionAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            MultiplicationAssignmentNode => new MultiplicationAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            DivisionAssignmentNode => new DivisionAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            RemainderAssignmentNode => new RemainderAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            BitwiseAndAssignmentNode => new BitwiseAndAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            BitwiseOrAssignmentNode => new BitwiseOrAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            BitwiseXorAssignmentNode => new BitwiseXorAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            LeftShiftAssignmentNode => new LeftShiftAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            RightShiftAssignmentNode => new RightShiftAssignmentNode(ResolveExpression(assignment.Lhs, variables, scope), ResolveExpression(assignment.Rhs, variables, scope)),
            _ => throw new UnreachableException($"Unknown assignment type: {assignment.Tag.ToStringFast()}")
        };
    }

    [return: NotNullIfNotNull(nameof(statement))]
    private IStatementNode? ResolveStatement(IStatementNode? statement, Dictionary<Original, Mangled> variables, int scope)
        => statement switch
        {
            ReturnNode node => ResolveReturn(node, variables, scope),
            ExpressionNode node => ResolveExpressionStatement(node, variables, scope),
            IfNode node => ResolveIf(node, variables, scope),
            WhileNode node => ResolveWhile(node, variables, scope),
            DoWhileNode node => ResolveDoWhile(node, variables, scope),
            ForNode node => ResolveFor(node, Duplicate(variables), scope + 1),
            LabelNode node => ResolveLabel(node, variables, scope),
            CompoundNode node => ResolveCompound(node, Duplicate(variables), scope + 1),
            GotoNode node => node,
            NullNode node => node,
            BreakNode node => node,
            ContinueNode node => node,
            null => null,
            _ => throw new UnreachableException($"Unknow statement type: {statement.Tag.ToStringFast()}")
        };

    private LabelNode ResolveLabel(LabelNode node, Dictionary<Original, Mangled> variables, int scope)
        => node with { Statement = ResolveStatement(node.Statement, variables, scope) };

    private ReturnNode ResolveReturn(ReturnNode node, Dictionary<Original, Mangled> variables, int scope) 
        => new(ResolveExpression(node.Expression, variables, scope));

    private ExpressionNode ResolveExpressionStatement(
        ExpressionNode node, Dictionary<Original, Mangled> variables, int scope) 
            => new(ResolveExpression(node.Expression, variables, scope));

    private ForNode ResolveFor(ForNode node, Dictionary<Original, Mangled> variables, int scope)
    {
        IForLoopInitializer? init = node.Initializer switch
        {
            DeclarationNode n => ResolveDeclaration(n, variables, scope),
            IExpressionNode n => ResolveExpression(n, variables, scope),
            _ => null
        };

        var condition = node.Condition is not null
            ? ResolveExpression(node.Condition, variables, scope)
            : null;

        var post = node.Post is not null
            ? ResolveExpression(node.Post, variables, scope)
            : null;
        
        var body = ResolveStatement(node.Body, variables, scope);
        
        return new ForNode(init, condition, post, body);
    }

    private DoWhileNode ResolveDoWhile(DoWhileNode node, Dictionary<Original, Mangled> variables, int scope)
        => new
        (
            ResolveStatement(node.Body, variables, scope),
            ResolveExpression(node.Condition, variables, scope)
        );
    
    private WhileNode ResolveWhile(WhileNode node, Dictionary<Original, Mangled> variables, int scope)
        => new
        (
            ResolveExpression(node.Condition, variables, scope),
            ResolveStatement(node.Body, variables, scope)
        );

    private IfNode ResolveIf(IfNode node, Dictionary<Original, Mangled> variables, int scope)
        => new
        (
            ResolveExpression(node.Condition, variables, scope),
            ResolveStatement(node.Then, variables, scope),
            ResolveStatement(node.Else, variables, scope)
        );
    
    private CompoundNode ResolveCompound(CompoundNode compound, Dictionary<Original, Mangled> variables, int scope)
        => new(ResolveBlock(compound.Block, variables, scope));

    // Create a copy of the variables lookup table for
    // each new scope that's entered.
    private static Dictionary<Original, Mangled> Duplicate(in Dictionary<Original, Mangled> variables)
        => variables.ToDictionary(kvp => kvp.Key, kvp => kvp.Value with { FromCurrentBlock = false });
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }

    private readonly record struct Original(string Name, int Scope);

    private readonly record struct Mangled(string Name, bool FromCurrentBlock)
    {
        public static implicit operator string(Mangled mangled) => mangled.Name;
    }
}