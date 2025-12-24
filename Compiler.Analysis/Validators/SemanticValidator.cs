using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Validators;

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

    public ProgramNode Validate(ProgramNode program)
    {
        Dictionary<string, List<Mangled>> identifiers = [];
        var functions = program.Functions
            .Select(f => ResolveFunction(f, identifiers, true))
            .ToList();
        
        return new ProgramNode(functions);
    }           
    
    private BlockNode ResolveBlock(BlockNode block, Dictionary<string, List<Mangled>> identifiers)
        => new([
            ..block.Items.Select(i => i switch
            {
                VariableDeclarationNode declaration => (IBlockItem)ResolveDeclaration(declaration, identifiers),
                FunctionDeclarationNode node => ResolveFunction(node, identifiers, false),
                IStatementNode statement => ResolveStatement(statement, identifiers),
                _ => throw new UnreachableException($"Unknown block item: {i.Tag.ToStringFast()}")
            })
        ]);

    private FunctionDeclarationNode ResolveFunction(
        FunctionDeclarationNode node, 
        Dictionary<string, List<Mangled>> identifiers,
        bool allowDefinition)
    {
        if (identifiers.TryGetValue(node.Name, out var names))
        {
            if (names.Last() is { FromCurrentScope: true, ExternalLinkage: false })
                throw new FormatException($"redefinition of '{node.Name}'");
        }
        
        names ??= new List<Mangled>(2);
        var name = new Mangled(node.Name, true, true);
        
        names.Add(name);
        identifiers[node.Name] = names;
        
        var duplicated = Duplicate(identifiers);
        var parameters = node.Parameters
            .Select(p => ResolveParameter(p, duplicated))
            .ToList();
        
        if (node.Body is not null && !allowDefinition)
            throw new FormatException("function definition is not allowed here");
   
        return node with
        {
            Parameters = parameters,
            Body = node.Body is not null ? ResolveBlock(node.Body, duplicated) : null
        };       
    }

    private VariableDeclarationNode ResolveDeclaration(
        VariableDeclarationNode declaration, 
        Dictionary<string, List<Mangled>> identifiers)
    {
        if (identifiers.TryGetValue(declaration.Identifier, out var names) && names.Last().FromCurrentScope)
            throw new FormatException($"Duplicate variable declaration: {declaration.Identifier}");
        
        names ??= new List<Mangled>(2);
        
        var name = new Mangled(MangleIdentifier(declaration.Identifier), true, false);
        names.Add(name);

        identifiers[declaration.Identifier] = names;        
        
        var initializer = declaration.Initializer is { } init
            ? ResolveExpression(init, identifiers)
            : null;
        
        return new VariableDeclarationNode(name, initializer);
    }

    private string ResolveParameter(string parameter, Dictionary<string, List<Mangled>> identifiers)
    {
        if (identifiers.TryGetValue(parameter, out var names) && names.Last().FromCurrentScope)
            throw new FormatException($"redefinition of parameter '{parameter}'");
        
        names ??= new List<Mangled>(2);
        var name = new Mangled(MangleIdentifier(parameter), true, false);
        names.Add(name);
        
        identifiers[parameter] = names;

        return name;
    }

    private IExpressionNode ResolveExpression(IExpressionNode expression, Dictionary<string, List<Mangled>> identifiers)
        => expression switch
        {
            IAssignmentNode assignment => ResolveAssignment(assignment, identifiers),
            UnaryNode unary => ResolveUnary(unary, identifiers),
            BinaryNode binary => ResolveBinary(binary, identifiers),
            BitwiseNode bitwise => ResolveBitwise(bitwise, identifiers),
            VariableNode variable => ResolveVariable(variable, identifiers),
            ConditionalNode conditional => ResolveConditional(conditional, identifiers),
            FunctionCallNode functionCall => ResolveFunctionCall(functionCall, identifiers),
            IConstantNode constant => constant,            
            _ => throw new UnreachableException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private FunctionCallNode ResolveFunctionCall(
        FunctionCallNode node,
        Dictionary<string, List<Mangled>> identifiers)
    {
        if (!identifiers.TryGetValue(node.Identifier, out var names))
            throw new FormatException($"use of undeclared identifier '{node.Identifier}'");
        
        var args = node.Args
            .Select(arg => ResolveExpression(arg, identifiers))
            .ToList();
        
        return new FunctionCallNode(names.Last(), args);
    }

    private ConditionalNode ResolveConditional(ConditionalNode conditional, Dictionary<string, List<Mangled>> identifiers) 
        => new
        (
            ResolveExpression(conditional.Condition, identifiers), 
            ResolveExpression(conditional.True, identifiers), 
            ResolveExpression(conditional.False, identifiers)
        );

    private static VariableNode ResolveVariable(
        VariableNode variable,
        Dictionary<string, List<Mangled>> identifiers)
        => !identifiers.TryGetValue(variable.Identifier, out var names)
            ? throw new FormatException($"Undeclared variable: {variable.Identifier}")
            : new VariableNode(names.Last());
    
    private string MangleIdentifier(string identifier)
        => $"{identifier}.{_variableCount++}";

    private BitwiseNode ResolveBitwise(BitwiseNode bitwise, Dictionary<string, List<Mangled>> identifiers)
        => new
           (
               bitwise.Operator, 
               ResolveExpression(bitwise.Lhs, identifiers), 
               ResolveExpression(bitwise.Rhs,  identifiers)
           );
    
    private BinaryNode ResolveBinary(BinaryNode binary,  Dictionary<string, List<Mangled>> identifiers)
        => new
           (
               binary.Operator, 
               ResolveExpression(binary.Lhs, identifiers), 
               ResolveExpression(binary.Rhs, identifiers)
           );

    private UnaryNode ResolveUnary(UnaryNode unary,  Dictionary<string, List<Mangled>> identifiers)
    {
        if (IsIncrement(unary.Operator) && unary.Expression is not VariableNode)        
            throw new FormatException("An lvalue is required as increment operand");
        
        if (IsDecrement(unary.Operator) && unary.Expression is not VariableNode)        
            throw new FormatException("An lvalue is required as decrement operand");
        
        return unary with { Expression = ResolveExpression(unary.Expression, identifiers) };

        static bool IsIncrement(IUnaryOperatorNode op) =>
            op is PrefixIncrementNode or PostfixIncrementNode;
        
        static bool IsDecrement(IUnaryOperatorNode op) =>
            op is PrefixDecrementNode or PostfixDecrementNode;
    }

    private IAssignmentNode ResolveAssignment(IAssignmentNode assignment,  Dictionary<string, List<Mangled>> identifiers)
    {
        if (assignment.Lhs is not VariableNode)
            throw new FormatException("Expression must be modifiable lvalue");

        return assignment switch
        {
            AssignmentNode node => ResolveAssignment(node, identifiers),
            AdditionAssignmentNode node => ResolveAdditionAssignment(node, identifiers),
            SubtractionAssignmentNode node => ResolveSubtractionAssignment(node, identifiers),
            MultiplicationAssignmentNode node => ResolveMultiplicationAssignment(node, identifiers),
            DivisionAssignmentNode node => ResolveDivisionAssignment(node, identifiers),
            RemainderAssignmentNode node => ResolveRemainderAssignment(node, identifiers),
            BitwiseAndAssignmentNode node => ResolveBitwiseAndAssignment(node, identifiers),
            BitwiseOrAssignmentNode node => ResolveBitwiseOrAssignment(node, identifiers),
            BitwiseXorAssignmentNode node => ResolveBitwiseXorAssignment(node, identifiers),
            LeftShiftAssignmentNode node => ResolveLeftShiftAssignment(node, identifiers),
            RightShiftAssignmentNode node => ResolveRightShiftAssignment(node, identifiers),
            _ => throw new UnreachableException($"Unknown assignment type: {assignment.Tag.ToStringFast()}")
        };
    }
    
    private RightShiftAssignmentNode ResolveRightShiftAssignment(
        RightShiftAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private LeftShiftAssignmentNode ResolveLeftShiftAssignment(
        LeftShiftAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private BitwiseXorAssignmentNode ResolveBitwiseXorAssignment(
        BitwiseXorAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private BitwiseOrAssignmentNode ResolveBitwiseOrAssignment(
        BitwiseOrAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private BitwiseAndAssignmentNode ResolveBitwiseAndAssignment(
        BitwiseAndAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private RemainderAssignmentNode ResolveRemainderAssignment(
        RemainderAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private DivisionAssignmentNode ResolveDivisionAssignment(
        DivisionAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private MultiplicationAssignmentNode ResolveMultiplicationAssignment(
        MultiplicationAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private SubtractionAssignmentNode ResolveSubtractionAssignment(
        SubtractionAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private AdditionAssignmentNode ResolveAdditionAssignment(
        AdditionAssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private AssignmentNode ResolveAssignment(
        AssignmentNode assignment,
        Dictionary<string, List<Mangled>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };

    [return: NotNullIfNotNull(nameof(statement))]
    private IStatementNode? ResolveStatement(IStatementNode? statement, Dictionary<string, List<Mangled>> identifiers)
        => statement switch
        {
            ReturnNode node => ResolveReturn(node, identifiers),
            ExpressionNode node => ResolveExpressionStatement(node, identifiers),
            IfNode node => ResolveIf(node, identifiers),
            WhileNode node => ResolveWhile(node, identifiers),
            DoWhileNode node => ResolveDoWhile(node, identifiers),
            ForNode node => ResolveFor(node, Duplicate(identifiers)),
            LabelNode node => ResolveLabel(node, identifiers),
            CompoundNode node => ResolveCompound(node, Duplicate(identifiers)),
            SwitchNode node => ResolveSwitch(node, identifiers),
            CaseNode node => ResolveCase(node, identifiers),
            DefaultNode node => ResolveDefault(node, identifiers),
            GotoNode node => node,
            NullNode node => node,
            BreakNode node => node,
            ContinueNode node => node,
            null => null,
            _ => throw new UnreachableException($"Unknow statement type: {statement.Tag.ToStringFast()}")
        };
    
    private DefaultNode ResolveDefault(DefaultNode node, Dictionary<string, List<Mangled>> identifiers)
        => node with { Statement = ResolveStatement(node.Statement, identifiers) };

    private CaseNode ResolveCase(CaseNode node, Dictionary<string, List<Mangled>> identifiers)
    { 
        AssertConstant(node.ConstantExpression);
        return node with
        {
            ConstantExpression = ResolveExpression(node.ConstantExpression, identifiers),
            Statement = ResolveStatement(node.Statement, identifiers)
        };
    }

    private void AssertConstant(IExpressionNode expression)
    {
        switch (expression)
        {
            case BinaryNode node:
                Binary(node);
                break;
            case UnaryNode node:
                Unary(node);
                break;
            case BitwiseNode node:
                Bitwise(node);
                break;
            case ConstantNode<int>:
                break;
            default: 
                throw new FormatException("case label does not reduce to an integer constant");
        }
    }

    private void Bitwise(BitwiseNode node)
    {
        AssertConstant(node.Lhs);
        AssertConstant(node.Rhs);
    }
    
    private void Unary(UnaryNode node)
        => AssertConstant(node.Expression);

    private void Binary(BinaryNode node)
    {
        AssertConstant(node.Lhs);
        AssertConstant(node.Rhs);
    }
    
    private SwitchNode ResolveSwitch(SwitchNode node, Dictionary<string, List<Mangled>> identifiers)
        => node with
        {
            Value = ResolveExpression(node.Value, identifiers), 
            Body = ResolveStatement(node.Body, identifiers)
        };

    private LabelNode ResolveLabel(LabelNode node, Dictionary<string, List<Mangled>> identifiers)
        => node with { Statement = ResolveStatement(node.Statement, identifiers) };

    private ReturnNode ResolveReturn(ReturnNode node, Dictionary<string, List<Mangled>> identifiers) 
        => new(ResolveExpression(node.Expression, identifiers));

    private ExpressionNode ResolveExpressionStatement(
        ExpressionNode node, Dictionary<string, List<Mangled>> identifiers) 
            => new(ResolveExpression(node.Expression, identifiers));

    private ForNode ResolveFor(ForNode node, Dictionary<string, List<Mangled>> identifiers)
    {
        IForLoopInitializer? init = node.Initializer switch
        {
            VariableDeclarationNode n => ResolveDeclaration(n, identifiers),
            IExpressionNode n => ResolveExpression(n, identifiers),
            _ => null
        };

        var condition = node.Condition is not null
            ? ResolveExpression(node.Condition, identifiers)
            : null;

        var post = node.Post is not null
            ? ResolveExpression(node.Post, identifiers)
            : null;
        
        var body = ResolveStatement(node.Body, identifiers);
        
        return new ForNode(init, condition, post, body);
    }

    private DoWhileNode ResolveDoWhile(DoWhileNode node, Dictionary<string, List<Mangled>> identifiers)
        => new
        (
            ResolveStatement(node.Body, identifiers),
            ResolveExpression(node.Condition, identifiers)
        );
    
    private WhileNode ResolveWhile(WhileNode node, Dictionary<string, List<Mangled>> identifiers)
        => new
        (
            ResolveExpression(node.Condition, identifiers),
            ResolveStatement(node.Body, identifiers)
        );

    private IfNode ResolveIf(IfNode node, Dictionary<string, List<Mangled>> identifiers)
        => new
        (
            ResolveExpression(node.Condition, identifiers),
            ResolveStatement(node.Then, identifiers),
            ResolveStatement(node.Else, identifiers)
        );
    
    private CompoundNode ResolveCompound(CompoundNode compound, Dictionary<string, List<Mangled>> identifiers)
        => new(ResolveBlock(compound.Block, identifiers));

    // Create a copy of the identifiers lookup table for
    // each new scope that's entered.
    private static Dictionary<string, List<Mangled>> Duplicate(in Dictionary<string, List<Mangled>> identifiers)
        => identifiers.ToDictionary
           (
               kvp => kvp.Key, 
               kvp => kvp.Value.Select(mangled => new Mangled(mangled, false, false)).ToList()
           );
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }

    private readonly record struct Mangled(string Name, bool FromCurrentScope, bool ExternalLinkage)
    {
        public static implicit operator string(Mangled mangled) => mangled.Name;
    }
}