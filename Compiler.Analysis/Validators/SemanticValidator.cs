using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Compiler.Common.Extensions;
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
        Dictionary<string, List<Entry>> identifiers = [];
        var nodes = program.Nodes
            .Select(n => n switch 
            { 
                FunctionDeclarationNode func => ResolveFunction(func, identifiers, false),
                VariableDeclarationNode variable => ResolveFileScopedVariableDecl(variable, identifiers),
                _ => throw new UnreachableException($"Unknown program node: {n.Tag.ToStringFast()}")
            })
            .ToList();
        
        return new ProgramNode(nodes);
    }           
    
    private BlockNode ResolveBlock(BlockNode block, Dictionary<string, List<Entry>> identifiers)
        => new([
            ..block.Items.Select(i => i switch
            {
                VariableDeclarationNode declaration => (IBlockItem)ResolveVariableDecl(declaration, identifiers),
                FunctionDeclarationNode node => ResolveFunction(node, identifiers, true),
                IStatementNode statement => ResolveStatement(statement, identifiers),
                _ => throw new UnreachableException($"Unknown block item: {i.Tag.ToStringFast()}")
            })
        ]);

    private IDeclarationNode ResolveFunction(
        FunctionDeclarationNode node, 
        Dictionary<string, List<Entry>> identifiers,
        bool isBlockScope)
    {
        if (identifiers.TryGetValue(node.Name, out var entries))
        {
            var previous = entries.Last();
            if (previous is { FromCurrentScope: true, IsFunction: false })
                throw new FormatException($"error: redefinition of '{node.Name}' as different kind of symbol");
            if (previous is { FromCurrentScope: true, HasLinkage: false })
                throw new FormatException($"error: redefinition of '{node.Name}'");
        }
        
        entries ??= new List<Entry>(2);
        var name = new Entry(node.Name, true, true, true);
        
        entries.Add(name);
        identifiers[node.Name] = entries;
        
        var duplicated = Duplicate(identifiers);
        
        var parameters = node.Parameters
            .Select(p => ResolveParameter(p, duplicated))
            .ToList();
        
        if (node.Body is not null && isBlockScope)
            throw new FormatException("error: function definition is not allowed here");
        
        if (isBlockScope && node.StorageClass is StorageClass.Static)
            throw new FormatException("error: function declared in block scope cannot have 'static' storage class");
   
        return node with
        {
            Parameters = parameters,
            Body = node.Body is not null ? ResolveBlock(node.Body, duplicated) : null
        };       
    }

    private static VariableDeclarationNode ResolveFileScopedVariableDecl(
        VariableDeclarationNode declaration, 
        Dictionary<string, List<Entry>> identifiers)
    {
        identifiers.AddOrUpdate
        (
            declaration.Identifier,
            static id => [GetName(id)],
            static (id, names) =>
            {
                names.Add(GetName(id));
                return names;
            }
        );
        return declaration;
        
        static Entry GetName(string id) => 
            new(id, true, true, false);
    }

    private VariableDeclarationNode ResolveVariableDecl(
        VariableDeclarationNode decl, 
        Dictionary<string, List<Entry>> identifiers)
    {
        if (identifiers.TryGetValue(decl.Identifier, out var entries))
        {
            var previous = entries.Last();
            if (previous is { FromCurrentScope: true, IsFunction: true })
             throw new FormatException($"error: redefinition of '{decl.Identifier}' as different kind of symbol");
            
            if (previous is { FromCurrentScope: true, HasLinkage: true } && 
                decl.StorageClass is not StorageClass.Extern)
            {
                throw new FormatException($"error: non-extern declaration of '{decl.Identifier}' follows extern declaration");
            }
            if (previous is { FromCurrentScope: true, HasLinkage: false } &&
                decl.StorageClass is StorageClass.Extern)
            {
                throw new FormatException($"error: extern declaration of '{decl.Identifier}' follows non-extern declaration");           
            }
            if (previous is { FromCurrentScope: true, HasLinkage: false } && 
                decl.StorageClass is not StorageClass.Extern)
            {
                throw new FormatException($"error: redefinition of '{decl.Identifier}'");
            }
        }
        
        entries ??= new List<Entry>(2);
        
        var hasLinkage = decl.StorageClass is StorageClass.Extern;
        var identifier = !hasLinkage 
            ? MangleIdentifier(decl.Identifier) 
            : decl.Identifier;
        
        var name = new Entry
        (
            identifier, 
            true, 
            hasLinkage, 
            false
        );
        entries.Add(name);

        identifiers[decl.Identifier] = entries;        
        
        var initializer = decl.Initializer is { } init
            ? ResolveExpression(init, identifiers)
            : null;
        
        return new VariableDeclarationNode(name, initializer);
    }

    private string ResolveParameter(string parameter, Dictionary<string, List<Entry>> identifiers)
    {
        if (identifiers.TryGetValue(parameter, out var names) && names.Last().FromCurrentScope)
            throw new FormatException($"error: redefinition of parameter '{parameter}'");
        
        names ??= new List<Entry>(2);
        var name = new Entry
        (
            MangleIdentifier(parameter), 
            true, 
            false, 
            false
        );
        names.Add(name);
        
        identifiers[parameter] = names;

        return name;
    }

    private IExpressionNode ResolveExpression(IExpressionNode expression, Dictionary<string, List<Entry>> identifiers)
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
        Dictionary<string, List<Entry>> identifiers)
    {
        if (!identifiers.TryGetValue(node.Identifier, out var names))
            throw new FormatException($"error: use of undeclared identifier '{node.Identifier}'");
        
        var args = node.Args
            .Select(arg => ResolveExpression(arg, identifiers))
            .ToList();
        
        return new FunctionCallNode(names.Last(), args);
    }

    private ConditionalNode ResolveConditional(ConditionalNode conditional, Dictionary<string, List<Entry>> identifiers) 
        => new
        (
            ResolveExpression(conditional.Condition, identifiers), 
            ResolveExpression(conditional.True, identifiers), 
            ResolveExpression(conditional.False, identifiers)
        );

    private static VariableNode ResolveVariable(
        VariableNode variable,
        Dictionary<string, List<Entry>> identifiers)
        => !identifiers.TryGetValue(variable.Identifier, out var names)
            ? throw new FormatException($"Undeclared variable: {variable.Identifier}")
            : new VariableNode(names.Last());
    
    private string MangleIdentifier(string identifier)
        => $"{identifier}.{_variableCount++}";

    private BitwiseNode ResolveBitwise(BitwiseNode bitwise, Dictionary<string, List<Entry>> identifiers)
        => new
           (
               bitwise.Operator, 
               ResolveExpression(bitwise.Lhs, identifiers), 
               ResolveExpression(bitwise.Rhs,  identifiers)
           );
    
    private BinaryNode ResolveBinary(BinaryNode binary,  Dictionary<string, List<Entry>> identifiers)
        => new
           (
               binary.Operator, 
               ResolveExpression(binary.Lhs, identifiers), 
               ResolveExpression(binary.Rhs, identifiers)
           );

    private UnaryNode ResolveUnary(UnaryNode unary,  Dictionary<string, List<Entry>> identifiers)
    {
        if ((IsIncrement(unary.Operator) || IsDecrement(unary.Operator)) && unary.Expression is not VariableNode)        
            throw new FormatException("error: expression is not assignable");

        return unary with { Expression = ResolveExpression(unary.Expression, identifiers) };

        static bool IsIncrement(IUnaryOperatorNode op) =>
            op is PrefixIncrementNode or PostfixIncrementNode;
        
        static bool IsDecrement(IUnaryOperatorNode op) =>
            op is PrefixDecrementNode or PostfixDecrementNode;
    }

    private IAssignmentNode ResolveAssignment(IAssignmentNode assignment,  Dictionary<string, List<Entry>> identifiers)
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
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private LeftShiftAssignmentNode ResolveLeftShiftAssignment(
        LeftShiftAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private BitwiseXorAssignmentNode ResolveBitwiseXorAssignment(
        BitwiseXorAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private BitwiseOrAssignmentNode ResolveBitwiseOrAssignment(
        BitwiseOrAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private BitwiseAndAssignmentNode ResolveBitwiseAndAssignment(
        BitwiseAndAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private RemainderAssignmentNode ResolveRemainderAssignment(
        RemainderAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private DivisionAssignmentNode ResolveDivisionAssignment(
        DivisionAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private MultiplicationAssignmentNode ResolveMultiplicationAssignment(
        MultiplicationAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private SubtractionAssignmentNode ResolveSubtractionAssignment(
        SubtractionAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private AdditionAssignmentNode ResolveAdditionAssignment(
        AdditionAssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };
    
    private AssignmentNode ResolveAssignment(
        AssignmentNode assignment,
        Dictionary<string, List<Entry>> identifiers) => assignment with
    {
        Lhs = ResolveExpression(assignment.Lhs, identifiers),
        Rhs = ResolveExpression(assignment.Rhs, identifiers)
    };

    [return: NotNullIfNotNull(nameof(statement))]
    private IStatementNode? ResolveStatement(IStatementNode? statement, Dictionary<string, List<Entry>> identifiers)
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
    
    private DefaultNode ResolveDefault(DefaultNode node, Dictionary<string, List<Entry>> identifiers)
        => node with { Statement = ResolveStatement(node.Statement, identifiers) };

    private CaseNode ResolveCase(CaseNode node, Dictionary<string, List<Entry>> identifiers)
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
    
    private SwitchNode ResolveSwitch(SwitchNode node, Dictionary<string, List<Entry>> identifiers)
        => node with
        {
            Value = ResolveExpression(node.Value, identifiers), 
            Body = ResolveStatement(node.Body, identifiers)
        };

    private LabelNode ResolveLabel(LabelNode node, Dictionary<string, List<Entry>> identifiers)
        => node with { Statement = ResolveStatement(node.Statement, identifiers) };

    private ReturnNode ResolveReturn(ReturnNode node, Dictionary<string, List<Entry>> identifiers) 
        => new(ResolveExpression(node.Expression, identifiers));

    private ExpressionNode ResolveExpressionStatement(
        ExpressionNode node, Dictionary<string, List<Entry>> identifiers) 
            => new(ResolveExpression(node.Expression, identifiers));

    private ForNode ResolveFor(ForNode node, Dictionary<string, List<Entry>> identifiers)
    {
        IForLoopInitializer? init = node.Initializer switch
        {
            VariableDeclarationNode n => ResolveVariableDecl(n, identifiers),
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

    private DoWhileNode ResolveDoWhile(DoWhileNode node, Dictionary<string, List<Entry>> identifiers)
        => new
        (
            ResolveStatement(node.Body, identifiers),
            ResolveExpression(node.Condition, identifiers)
        );
    
    private WhileNode ResolveWhile(WhileNode node, Dictionary<string, List<Entry>> identifiers)
        => new
        (
            ResolveExpression(node.Condition, identifiers),
            ResolveStatement(node.Body, identifiers)
        );

    private IfNode ResolveIf(IfNode node, Dictionary<string, List<Entry>> identifiers)
        => new
        (
            ResolveExpression(node.Condition, identifiers),
            ResolveStatement(node.Then, identifiers),
            ResolveStatement(node.Else, identifiers)
        );
    
    private CompoundNode ResolveCompound(CompoundNode compound, Dictionary<string, List<Entry>> identifiers)
        => new(ResolveBlock(compound.Block, identifiers));

    /// <summary>
    /// Creates a deep copy of the identifiers lookup table, cloning each entry to ensure
    /// that changes in the new scope are isolated from the parent scope.
    /// </summary>
    /// <param name="identifiers">
    /// The dictionary representing the current scope's identifier table. Keys are identifier names,
    /// and values are lists of entries containing information about the identifier.
    /// </param>
    /// <returns>
    /// A dictionary containing a copied version of the input where each list of entries is
    /// cloned to prevent modifications from affecting the original.
    /// </returns>
    private static Dictionary<string, List<Entry>> Duplicate(in Dictionary<string, List<Entry>> identifiers)
        => identifiers.ToDictionary
           (
               kvp => kvp.Key, 
               kvp => kvp.Value
                   .Select(entry => new Entry(entry, false, false, entry.IsFunction))
                   .ToList()
           );
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }

    private readonly record struct Entry(string Name, bool FromCurrentScope, bool HasLinkage, bool IsFunction)
    {
        public static implicit operator string(Entry entry) => entry.Name;
    }
}