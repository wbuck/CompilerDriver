using System.Collections.Concurrent;
using System.Diagnostics;
using Compiler.Analysis.Types;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Validators;

public static class TypeChecker
{
    private static readonly ConcurrentDictionary<string, IEntry> Symbols = [];
    public static IReadOnlyDictionary<string, IEntry> SymbolTable => Symbols; 

    public static bool TryCheck(ProgramNode node)
    {
        try
        {
            Check(node);
            return true;
        }
        catch (FormatException ex)
        {
            PrintError(ex.Message);
        }
        return false;
    }

    public static void Check(ProgramNode node)
    {
        Symbols.Clear();
        foreach (var decl in node.Nodes)
        {
            switch (decl)
            {
                case FunctionDeclarationNode function:
                    FunctionDecl(function);
                    break;
                case VariableDeclarationNode variable:
                    VariableDecl(variable);
                    break;
                default:
                    throw new UnreachableException($"Unknown program node: {decl.Tag.ToStringFast()}");
            }
        }
    }
    
    private static void Block(BlockNode node) =>
        node.Items.ForEach(i =>
        {
            switch (i)
            {
                case VariableDeclarationNode declaration:
                    VariableDecl(declaration);
                    break;
                case FunctionDeclarationNode declaration:
                    FunctionDecl(declaration);
                    break;
                case IStatementNode statement:
                    Statement(statement);
                    break;
                default:
                    throw new UnreachableException($"Unknown block item: {i.Tag.ToStringFast()}");
            }
        });
    
    private static void FunctionDecl(FunctionDeclarationNode node)
    { 
        FuncDecl f = new(node.Parameters.Count);
        var defined = false;
        
        if (Symbols.TryGetValue(node.Name, out var entry))
        {
            if (entry is not FuncEntry { Type: FuncDecl decl } fe || decl != f)
                throw new FormatException($"conflicting types for '{node.Name}'");
            
            defined = fe.Defined;
        }
        if (defined && node.Body is not null)
            throw new FormatException($"redefinition of '{node.Name}'");
        
        Symbols[node.Name] = new FuncEntry(node.Name, f, defined || node.Body is not null);

        if (node.Body is null) return;
        
        node.Parameters.ForEach(p => Symbols[p] = new VarEntry(p, Int.Instance));
        Block(node.Body);
    }

    private static void VariableDecl(VariableDeclarationNode node)
    {
        Symbols[node.Identifier] = new VarEntry(node.Identifier, Int.Instance);
        if (node.Initializer is not null) Expression(node.Initializer);
    }
    
    private static void Statement(IStatementNode statement)
    {
        switch (statement)
        {
            case ReturnNode node: 
                Return(node);
                break;
            case ExpressionNode node: 
                ExpressionNode(node);
                break;
            case IfNode node: 
                If(node);
                break;
            case WhileNode node: 
                While(node);
                break;
            case DoWhileNode node: 
                DoWhile(node);
                break;
            case ForNode node: 
                For(node);
                break;
            case LabelNode node: 
                Label(node);
                break;
            case CompoundNode node: 
                Compound(node);
                break;
            case SwitchNode node:
                Switch(node);
                break;
            case CaseNode node: 
                Case(node);
                break;
            case DefaultNode node: 
                Default(node);
                break;
        }
    }
    
    private static void Default(DefaultNode node) 
        => Statement(node.Statement);

    private static void Case(CaseNode node)
    {
        Expression(node.ConstantExpression);
        Statement(node.Statement);
    }
    
    private static void Switch(SwitchNode node)
    {
        Expression(node.Value);
        Statement(node.Body);
    }
    
    private static void Compound(CompoundNode node) 
        => Block(node.Block);
    
    private static void Label(LabelNode node) 
        => Statement(node.Statement);

    private static void ForInit(IForLoopInitializer init)
    {
        switch (init)
        {
            case VariableDeclarationNode declaration:
                VariableDecl(declaration);
                break;
            case IExpressionNode node:
                Expression(node);
                break;
        }
    }

    private static void For(ForNode node)
    {
        if (node.Initializer is not null)
            ForInit(node.Initializer);
        if (node.Condition is not null)
            Expression(node.Condition);
        if (node.Post is not null)
            Expression(node.Post);
        
        Statement(node.Body);
    }
    
    private static void DoWhile(DoWhileNode node)
    {
        Expression(node.Condition);
        Statement(node.Body);
    }

    private static void While(WhileNode node)
    {
        Expression(node.Condition);
        Statement(node.Body);
    }

    private static void If(IfNode node)
    {
        Expression(node.Condition);
        Statement(node.Then);
        
        if (node.Else is not null) 
            Statement(node.Else);
    }

    private static void ExpressionNode(ExpressionNode node)
        => Expression(node.Expression);
    
    private static void Return(ReturnNode node) 
        => Expression(node.Expression);

    private static void Expression(IExpressionNode expr)
    {
        switch (expr)
        {
            case FunctionCallNode node:
                FunctionCall(node);
                break;
            case VariableNode node:
                Variable(node);
                break;
            case BinaryNode node:
                Binary(node);
                break;
            case UnaryNode node:
                Unary(node);
                break;
            case BitwiseNode node:
                Bitwise(node);
                break;
            case ConditionalNode node:
                Conditional(node);
                break;
            case IAssignmentNode node:
                Assignment(node);
                break;
        }
    }

    private static void Assignment(IAssignmentNode node)
    {
        Expression(node.Lhs);
        Expression(node.Rhs);
    }

    private static void Conditional(ConditionalNode node)
    {
        Expression(node.Condition);
        Expression(node.True);
        Expression(node.False);
    }
    
    private static void Bitwise(BitwiseNode node)
    {
        Expression(node.Lhs);
        Expression(node.Rhs);
    }

    private static void Binary(BinaryNode node)
    {
        Expression(node.Lhs);
        Expression(node.Rhs);
    }
    
    private static void Unary(UnaryNode node) 
        => Expression(node.Expression);

    private static void FunctionCall(FunctionCallNode node)
    {
        var type = Symbols[node.Identifier].Type;
        
        if (type is not FuncDecl func)
            throw new FormatException($"called object type '{type.TypeName}' is not a function");
        
        if (node.Args.Count < func.ParamCount)
            throw new FormatException($"too few arguments to function call, expected {func.ParamCount}, have {node.Args.Count}");
        if (node.Args.Count > func.ParamCount)
            throw new FormatException($"too many arguments to function call, expected {func.ParamCount}, have {node.Args.Count}");

        node.Args.ForEach(Expression);
    }

    private static void Variable(VariableNode node)
    {
        if (Symbols[node.Identifier].Type is not Int)
            throw new FormatException($"function '{node.Identifier}' used as variable");
    }
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}

public interface IEntry
{
    string Name { get; }
    IType Type { get; }
}

public readonly record struct VarEntry
(
    string Name,
    IType Type
) : IEntry;

public readonly record struct FuncEntry
(
    string Name,
    IType Type,
    bool Defined
) : IEntry;