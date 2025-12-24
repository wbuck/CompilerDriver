using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Compiler.Common.Helpers;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Annotation;

public class SwitchLabelAnnotation
{
    private readonly LabelGenerator _labelGenerator = new();

    public static ProgramNode Annotate(ProgramNode node)
    {
        SwitchLabelAnnotation annotator = new();
        var functions = node.Functions
            .Select(f => annotator.Function(f, null, null))
            .ToList();
        
        return node with { Functions = functions };
    }
        
    private FunctionDeclarationNode Function(FunctionDeclarationNode node, string? label, List<SwitchLabel>? cases)
        => node with { Body = node.Body is not null ? Block(node.Body, label, cases, false) : null };

    [return: NotNullIfNotNull(nameof(statement))]
    private IStatementNode? Statement(IStatementNode? statement, string? label, List<SwitchLabel>? cases, bool inLoop)
        => statement switch
        {
            IfNode node => If(node, label, cases, inLoop),
            LabelNode node => Label(node, label, cases, inLoop),
            WhileNode node => While(node, label, cases),
            DoWhileNode node => DoWhile(node, label, cases),
            ForNode node => For(node, label, cases),
            SwitchNode node => Switch(node),
            CompoundNode node => Compound(node, label, cases, inLoop),
            CaseNode node => Case(node, label, cases, inLoop),
            DefaultNode node => Default(node, label, cases, inLoop),
            BreakNode node => Break(node, label, inLoop),
            null => null,
            _ =>  statement           
        };

    private static BreakNode Break(BreakNode node, string? label, bool inLoop)
    {
        // The break statement may already have an associated
        // label from an outer loop, so we'll have to replace it.
        if (label is null || inLoop) return node;
        return node with { Label = label };
    }

    private DefaultNode Default(DefaultNode node, string? label, List<SwitchLabel>? cases, bool inLoop)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(DefaultNode)} is not null");
        if (label is null) return node;
        
        var defaultLabel = $"{label}.default"; 
        cases?.Add(new SwitchLabel(defaultLabel, null, null));
        
        return node with { Label = defaultLabel, Statement = Statement(node.Statement, label, cases, inLoop)};       
    }

    private CaseNode Case(CaseNode node, in string? label, List<SwitchLabel>? cases, bool inLoop)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(CaseNode)} is not null");
        if (label is null) return node;
        
        var constant = FoldExpression(node.ConstantExpression);

        // Case labels will look like the following: switch1.case.123.
        var caseLabel = _labelGenerator.GetNextLabel($"{label}.case");
        cases?.Add(new SwitchLabel(caseLabel, node.ConstantExpression, constant));

        return node with
        {
            Label = caseLabel, 
            Statement = Statement(node.Statement, label, cases, inLoop)
        };

    }

    private int FoldExpression(IExpressionNode expression)
        => expression switch
        {
            BinaryNode node => Binary(node),
            UnaryNode node => Unary(node),
            BitwiseNode node => Bitwise(node),
            ConstantNode<int> node => node.Value,
            _ => throw new UnreachableException()
        };

    private int Bitwise(BitwiseNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);

        return node.Operator switch
        {
            BitwiseAndNode => lhs & rhs,
            BitwiseOrNode => lhs | rhs,
            BitwiseXorNode => lhs ^ rhs,
            BitwiseLeftShiftNode => lhs << rhs,
            BitwiseRightShiftNode => lhs >> rhs,
            _ => throw new UnreachableException()
        };
    }
    
    private int Unary(UnaryNode node)
    {
        var constant = FoldExpression(node.Expression);
        
        return node.Operator switch
        {
            NegateNode => -constant,
            ComplementNode => ~constant,
            NotNode => constant == 0 ? 1 : 0,
            _ => throw new UnreachableException()
        };
    }

    private int Binary(BinaryNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);

        return node.Operator switch
        {
            AdditionNode => lhs + rhs,
            SubtractionNode => lhs - rhs,
            MultiplicationNode => lhs * rhs,
            DivisionNode => lhs / rhs,
            RemainderNode => lhs % rhs,
            _ => throw new UnreachableException()
        };
    }
    
    private SwitchNode Switch(SwitchNode node)
    {
        Debug.Assert(node.Label is null, $"Label: '{node.Label}', for {nameof(SwitchNode)} is not null");

        List<SwitchLabel> cases = new(10);
        var label = _labelGenerator.GetNextLabel("switch");
        var body = Statement(node.Body, label, cases, false);
        
        return node with { Body = body, Cases = cases, Label = label };
    }
    
    private WhileNode While(WhileNode node, string? label, List<SwitchLabel>? cases)
        => node with { Body = Statement(node.Body, label, cases, true) };
    
    private DoWhileNode DoWhile(DoWhileNode node, string? label, List<SwitchLabel>? cases)
        => node with { Body = Statement(node.Body, label, cases, true) };
    
    private ForNode For(ForNode node, string? label, List<SwitchLabel>? cases)
        => node with { Body = Statement(node.Body, label, cases, true) };
        
    private LabelNode Label(LabelNode node, string? label, List<SwitchLabel>? cases, bool inLoop)
        => node with { Statement = Statement(node.Statement, label, cases, inLoop) };

    private IfNode If(IfNode node, string? label, List<SwitchLabel>? cases, bool inLoop)
        => node with
        {
            Then = Statement(node.Then, label, cases, inLoop), 
            Else = Statement(node.Else, label, cases, inLoop)
        };

    private CompoundNode Compound(CompoundNode node, string? label, List<SwitchLabel>? cases, bool inLoop)
        => node with { Block = Block(node.Block, label, cases, inLoop) };

    private BlockNode Block(BlockNode node, string? label, List<SwitchLabel>? cases, bool inLoop)
    {
        var items = node.Items
            .Select(i => i is IStatementNode s ? Statement(s, label, cases, inLoop) : i)
            .ToList();
        return node with { Items = items };
    }
}