using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Compiler.Common.Ast;
using Compiler.Common.Helpers;

namespace Compiler.Common.Stages;

public class LoopLabelAnnotation
{
    private readonly LabelGenerator _labelGenerator = new();

    public static ProgramNode Annotate(ProgramNode node)
        =>  node with { Function = new LoopLabelAnnotation().Function(node.Function, null) };
        
    private FunctionNode Function(FunctionNode node, string? label)
        => node with { Body = Block(node.Body, label) };

    [return: NotNullIfNotNull(nameof(statement))]
    private IStatementNode? Statement(IStatementNode? statement, string? label)
        => statement switch
        {
            IfNode node => If(node, label),
            LabelNode node => Label(node, label),
            WhileNode node => While(node),
            DoWhileNode node => DoWhile(node),
            ForNode node => For(node),
            SwitchNode node => Switch(node, label),
            CaseNode node => Case(node, label),
            DefaultNode node => Default(node, label),
            CompoundNode node => Compound(node, label),
            BreakNode node => Break(node, label),
            ContinueNode node => Continue(node, label),
            null => null,
            _ =>  statement           
        };
    
    private WhileNode While(WhileNode node)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(WhileNode)} is not null");
        var label = _labelGenerator.GetNextLabel("while");
        var body = Statement(node.Body, label);
        return node with { Body = body, Label = label };
    }
    
    private DoWhileNode DoWhile(DoWhileNode node)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(DoWhileNode)} is not null");
        var label = _labelGenerator.GetNextLabel("do_while");
        var body = Statement(node.Body, label);
        return node with { Body = body, Label = label };
    }  
    
    private ForNode For(ForNode node)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(ForNode)} is not null");
        var label = _labelGenerator.GetNextLabel("for");
        var body = Statement(node.Body, label);
        return node with { Body = body, Label = label };
    }

    private static BreakNode Break(BreakNode node, string? label)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(BreakNode)} is not null");
        return new BreakNode { Label = label };
    }
    
    private static ContinueNode Continue(ContinueNode node, string? label)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(ContinueNode)} is not null");
        return new ContinueNode { Label = label };
    }
        
    private LabelNode Label(LabelNode node, string? label)
        => node with { Statement = Statement(node.Statement, label) };

    private IfNode If(IfNode node, string? label)
        => node with { Then = Statement(node.Then, label), Else = Statement(node.Else, label) };

    private CompoundNode Compound(CompoundNode node, string? label)
        => node with { Block = Block(node.Block, label) };
    
    private SwitchNode Switch(SwitchNode node, string? label)
        => node with { Body = Statement(node.Body, label) };
    
    private CaseNode Case(CaseNode node, string? label)
        => node with { Statement = Statement(node.Statement, label) };
    
    private DefaultNode Default(DefaultNode node, string? label)
        => node with { Statement = Statement(node.Statement, label) };

    private BlockNode Block(BlockNode node, string? label)
    {
        var items = node.Items
            .Select(i => i is IStatementNode s ? Statement(s, label) : i)
            .ToList();
        return node with { Items = items };
    }
}