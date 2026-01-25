using Compiler.Common.Extensions;
using Compiler.Common.Helpers;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Annotation;

public class LabelReplacer
{
    private readonly LabelGenerator _labelGenerator = new();
    private readonly Dictionary<string, string> _labels = [];
    
    // Tracks which function we're currently in.
    private string _currentFuncName = string.Empty;
    
    public ProgramNode Replace(ProgramNode node)
        => node with
        {
            Nodes = node.Nodes.Select(n =>
            {
                if (n is not FunctionDeclarationNode func) 
                    return n;
                
                _currentFuncName = func.Name;
                return Function(func);
            }).ToList()
        };

    private FunctionDeclarationNode Function(FunctionDeclarationNode node)
        => node with { Body = node.Body is { } body ? Block(body) : null };    

    private IStatementNode Statement(IStatementNode statement)
        => statement switch
        {
            IfNode node => If(node),
            LabelNode node => Label(node),
            GotoNode node => Goto(node),
            CompoundNode node => Compound(node),
            WhileNode node => While(node),
            DoWhileNode node => DoWhile(node),
            ForNode node => For(node),
            SwitchNode node => Switch(node),
            CaseNode node => Case(node),
            DefaultNode node => Default(node),
            _ => statement
        };       
    
    private GotoNode Goto(GotoNode node)
        => node with
        {
            Label = _labels.GetOrAdd
            (
                GetLabelKey(node.Label), 
                _ => _labelGenerator.GetNextLabel(node.Label)
            )
        };
    
    private LabelNode Label(LabelNode node)
    {
        var name = _labels.GetOrAdd
        (
            GetLabelKey(node.Name), 
            _ => _labelGenerator.GetNextLabel(node.Name)
        );
        return node with { Name = name, Statement = Statement(node.Statement) };
    }

    private BlockNode Block(BlockNode node)
    {
        var items = node.Items
            .Select(i => i is IStatementNode s ? Statement(s) : i)
            .ToList();
        
        return node with { Items = items };
    }    
    
    private CompoundNode Compound(CompoundNode node) 
        => node with { Block = Block(node.Block) };
    
    private IfNode If(IfNode node) 
        => node with
        {
            Then = Statement(node.Then), 
            Else = node.Else is { } @else ? Statement(@else) : null
        };
    
    private SwitchNode Switch(SwitchNode node) 
        => node with { Body = Statement(node.Body) };
    
    private CaseNode Case(CaseNode node) 
        => node with { Statement = Statement(node.Statement) };
    
    private DefaultNode Default(DefaultNode node) 
        => node with { Statement = Statement(node.Statement) };
    
    private ForNode For(ForNode node) 
        => node with { Body = Statement(node.Body) };
    
    private WhileNode While(WhileNode node) 
        => node with { Body = Statement(node.Body) };
    
    private DoWhileNode DoWhile(DoWhileNode node) 
        => node with { Body = Statement(node.Body) };
    
    private string GetLabelKey(string label) => $"{_currentFuncName}#{label}";
}