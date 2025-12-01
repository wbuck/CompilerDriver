using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Compiler.Common.Ast;
using Compiler.Common.Helpers;

namespace Compiler.Common.Stages;

public class LabelAnnotation
{
    private readonly LabelGenerator _labelGenerator = new();

    public static ProgramNode Annotate(ProgramNode node)
        =>  node with { Function = new LabelAnnotation().Function(node.Function) };
        
    private FunctionNode Function(FunctionNode node, string? label = null)
        => node with { Body = Block(node.Body, label) };

    [return: NotNullIfNotNull(nameof(statement))]
    private IStatementNode? Statement(IStatementNode? statement, string? label = null)
        => statement switch
        {
            IfNode node => If(node, label),
            LabelNode node => Label(node, label),
            WhileNode node => While(node, _labelGenerator.GetNextLabel("while")),
            DoWhileNode node => DoWhile(node, _labelGenerator.GetNextLabel("do_while")),
            ForNode node => For(node, _labelGenerator.GetNextLabel("for")),
            // TODO: Add switch statement here.
            CompoundNode node => Compound(node, label),
            BreakNode node => Break(node, label),
            ContinueNode node => Continue(node, label),
            null => null,
            _ =>  statement           
        };
    
    private WhileNode While(WhileNode node, string label)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(WhileNode)} is not null");
        var body = Statement(node.Body, label);
        return node with { Body = body, Label = label };
    }
    
    private DoWhileNode DoWhile(DoWhileNode node, string? label = null)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(DoWhileNode)} is not null");
        var body = Statement(node.Body, label);
        return node with { Body = body, Label = label };
    }  
    
    private ForNode For(ForNode node, string? label = null)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(ForNode)} is not null");
        var body = Statement(node.Body, label);
        return node with { Body = body, Label = label };
    }

    private static BreakNode Break(BreakNode node, string? label = null)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(BreakNode)} is not null");
        return new BreakNode { Label = label };
    }
    
    private static ContinueNode Continue(ContinueNode node, string? label = null)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(ContinueNode)} is not null");
        return new ContinueNode { Label = label };
    }
        
    private LabelNode Label(LabelNode node, string? label = null)
        => node with { Statement = Statement(node.Statement, label) };

    private IfNode If(IfNode node, string? label = null)
        => node with { Then = Statement(node.Then, label), Else = Statement(node.Else, label) };

    private CompoundNode Compound(CompoundNode node, string? label = null)
        => node with { Block = Block(node.Block, label) };

    private BlockNode Block(BlockNode node, string? label = null)
    {
        var items = node.Items
            .Select(i => i is IStatementNode s ? Statement(s, label) : i)
            .ToList();
        return node with { Items = items };
    }
}