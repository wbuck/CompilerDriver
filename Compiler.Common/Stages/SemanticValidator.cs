using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Compiler.Common.Ast;
using Compiler.Common.Extensions;

namespace Compiler.Common.Stages;


public class SemanticValidator
{
    private int _variableCount = 0;
    private readonly Dictionary<Original, Mangled> _variables = [];

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
        => new ProgramNode(Function: ValidateFunction(program.Function));    

    private FunctionNode ValidateFunction(FunctionNode function)
    {
        _variables.Clear();
        return function with
        {
            Body = [..function.Body.Select(i => i switch
            {
                DeclarationNode declaration => ResolveDeclaration(declaration),
                IStatementNode statement => ResolveStatement(statement),
                _ => throw new UnreachableException($"Unknown block item: {i.Tag.ToStringFast()}")
            })]
        };
    }

    private IBlockItem ResolveDeclaration(DeclarationNode declaration)
    {
        if (_variables.ContainsKey(new Original(declaration.Identifier)))
            throw new FormatException($"Duplicate variable declaration: {declaration.Identifier}");
        
        var name = _variables.GetOrAdd
        (
            new Original(declaration.Identifier), 
            key => new Mangled(MangleIdentifier(key.Name))
        );
        
        var initializer = declaration.Initializer is not null
            ? ResolveExpression(declaration.Initializer)
            : null;
        
        return new DeclarationNode(name, initializer);
    }

    private IExpressionNode ResolveExpression(IExpressionNode expression)
        => expression switch
        {
            AssignmentNode assignment => ResolveAssignment(assignment),
            UnaryNode unary => ResolveUnary(unary),
            BinaryNode binary => ResolveBinary(binary),
            BitwiseNode bitwise => ResolveBitwise(bitwise),
            VariableNode variable => ResolveVariable(variable),
            IConstantNode constant => constant,            
            _ => throw new UnreachableException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private VariableNode ResolveVariable(VariableNode variable)
        => _variables.TryGetValue(new Original(variable.Identifier), out var mangled)
         ? new VariableNode(mangled)
         : throw new FormatException($"Undeclared variable: {variable.Identifier}");
    
    private string MangleIdentifier(string identifier)
        => $"{identifier}.{_variableCount++}";

    private BitwiseNode ResolveBitwise(BitwiseNode bitwise)
        => new(bitwise.Operator, ResolveExpression(bitwise.Lhs), ResolveExpression(bitwise.Rhs));
    
    private BinaryNode ResolveBinary(BinaryNode binary)
        => new(binary.Operator, ResolveExpression(binary.Lhs), ResolveExpression(binary.Rhs));

    private UnaryNode ResolveUnary(UnaryNode unary)
        => unary with { Expression = ResolveExpression(unary.Expression) };

    private AssignmentNode ResolveAssignment(AssignmentNode assignment)
        => assignment.Lhs is not VariableNode
            ? throw new FormatException($"Invalid lvalue type found: {assignment.Lhs.Tag.ToStringFast()}")
            : new AssignmentNode(ResolveExpression(assignment.Lhs), ResolveExpression(assignment.Rhs));

    private IBlockItem ResolveStatement(IStatementNode statement)
        => statement switch
        {
            ReturnNode ret => new ReturnNode(ResolveExpression(ret.Expression)),
            ExpressionNode exp => new ExpressionNode(ResolveExpression(exp.Expression)),
            NullNode @null => @null,
            _ => throw new UnreachableException($"Unknow statement type: {statement.Tag.ToStringFast()}")
        };
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }

    private readonly record struct Original(string Name);

    private readonly record struct Mangled(string Name)
    {
        public static implicit operator string(Mangled mangled) => mangled.Name;
    }
}