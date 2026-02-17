using System.Diagnostics;
using Compiler.Common.Symbols;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Validators;

public static class TypeChecker
{
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
        SymbolCollection.Clear();
        foreach (var decl in node.Nodes)
        {
            switch (decl)
            {
                case FunctionDeclarationNode function:
                    FunctionDecl(function);
                    break;
                case VariableDeclarationNode variable:
                    FileScopeVariableDecl(variable);
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
                    BlockScopeVariableDecl(declaration);
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
        FuncType newDecl = new(node.Parameters.Count);
        var defined = false;
        var global = node.StorageClass is not StorageClass.Static;
        
        if (SymbolCollection.TryGetValue(node.Name, out var entry))
        {
            if (entry is not FuncEntry { Type: FuncType previousDecl } funcEntry)
                throw new FormatException($"error: redefinition of '{node.Name}' as different kind of symbol");
            
            if (previousDecl != newDecl)
                throw new FormatException($"error: conflicting types for '{node.Name}'");
            
            var attrs = IEntry.GetAttribute<FuncAttributes>(funcEntry);
            
            defined = attrs.Defined;
            
            if (attrs.Global && node.StorageClass is StorageClass.Static)
                throw new FormatException($"error: static declaration of '{node.Name}' follows non-static declaration");
            
            if (defined && node.Body is not null)
                throw new FormatException($"error: redefinition of '{node.Name}'");  
            
            global = attrs.Global;
        }        
                
        var attributes = new FuncAttributes(defined || node.Body is not null, global);
        SymbolCollection.Add(node.Name, new FuncEntry(node.Name, newDecl, attributes));

        if (node.Body is null) return;
        
        node.Parameters.ForEach(p => SymbolCollection.Add(p, new VarEntry(p, Int.Instance, LocalAttributes.Instance)));
        Block(node.Body);
    }

    private static void FileScopeVariableDecl(VariableDeclarationNode node)
    {
        if (node.Initializer is not null and not IConstantNode)
            throw new FormatException("error: initializer element is not a compile-time constant");

        StaticInitValue newInit = node.Initializer switch
        {
            ConstantNode<int> constant => new Initial<int>(constant.Value),
            null when node.StorageClass is StorageClass.Extern => NoInitializer.Instance,
            null => Tentative.Instance,
            _ => throw new UnreachableException($"unreachable code: {node.Tag.ToStringFast()}")
        };
        
        var global = node.StorageClass is not StorageClass.Static;

        if (SymbolCollection.TryGetValue(node.Identifier, out var entry))
        {
            if (entry is not VarEntry { Type: Int, Attributes: StaticAttributes previous })
                throw new FormatException($"error: redefinition of '{entry.Name}' as different kind of symbol");
            
            if (node.StorageClass is StorageClass.Extern)
                global = previous.Global;
            
            else switch (previous.Global)
            {
                case true when !global:
                    throw new FormatException($"error: static declaration of '{entry.Name}' follows non-static declaration");
                case false when global:
                    throw new FormatException($"error: non-static declaration of '{entry.Name}' follows static declaration");
            }

            newInit = previous.InitialValue switch
            {
                IConstantInit when newInit is IConstantInit => 
                    throw new FormatException($"error: redefinition of '{entry.Name}'"),
                IConstantInit => previous.InitialValue,
                Tentative when newInit is not IConstantInit => Tentative.Instance,
                _ => newInit
            };
        }
        
        var attributes = new StaticAttributes(newInit, global);
        SymbolCollection.AddOrUpdate
        (
            node.Identifier, 
            id => new VarEntry(id, Int.Instance, attributes), 
            (id, prev) => (VarEntry)prev with { Attributes = attributes }
        );
    }

    private static void BlockScopeVariableDecl(VariableDeclarationNode node)
    {
        if (node.StorageClass is StorageClass.Extern)
        {
            if (node.Initializer is not null)
                throw new FormatException("error: declaration of block scope identifier with linkage cannot have an initializer");

            if (SymbolCollection.TryGetValue(node.Identifier, out var entry) && entry is not VarEntry)
                throw new FormatException($"error: redefinition of '{entry.Name}' as different kind of symbol");

            SymbolCollection.TryAdd
            (
                node.Identifier,
                new VarEntry(node.Identifier, Int.Instance, new StaticAttributes(NoInitializer.Instance, true))
            );
        }
        else if (node.StorageClass is StorageClass.Static)
        {
            if (node.Initializer is not null and not IConstantNode)
                throw new FormatException("error: initializer element is not a compile-time constant");

            var init = node.Initializer switch
            {
                ConstantNode<int> c => new Initial<int>(c.Value),
                null => new Initial<int>(0),
                _ => throw new UnreachableException($"unreachable code: {node.Tag.ToStringFast()}")
            };

            var symbol = new VarEntry(node.Identifier, Int.Instance, new StaticAttributes(init, false));
            SymbolCollection.AddOrUpdate
            (
                node.Identifier,
                _ => symbol,
                (_, _) => symbol
            );
        }
        else
        {
            SymbolCollection.AddOrUpdate
            (
                node.Identifier,
                id => new VarEntry(id, Int.Instance, LocalAttributes.Instance),
                (id, symbol) => (VarEntry)symbol with { Attributes = LocalAttributes.Instance }
            );
            
            if (node.Initializer is not null) Expression(node.Initializer);
        }
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
            case VariableDeclarationNode node:
                if (node.StorageClass is not StorageClass.None)
                    throw new FormatException("error: declaration of non-local variable in 'for' loop");
                
                BlockScopeVariableDecl(node);
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
        var type = SymbolCollection.Get(node.Identifier).Type;
        
        if (type is not FuncType func)
            throw new FormatException($"error: called object type '{type.TypeName}' is not a function");
        
        if (node.Args.Count < func.ParamCount)
            throw new FormatException($"error: too few arguments to function call, expected {func.ParamCount}, have {node.Args.Count}");
        if (node.Args.Count > func.ParamCount)
            throw new FormatException($"error: too many arguments to function call, expected {func.ParamCount}, have {node.Args.Count}");

        node.Args.ForEach(Expression);
    }

    private static void Variable(VariableNode node)
    {
        if (!SymbolCollection.IsType<Int>(node.Identifier))
            throw new FormatException($"function '{node.Identifier}' used as variable");
    }
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}