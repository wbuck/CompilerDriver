using System.Text;
using Compiler.Common.Extensions;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Validators;

public class LabelValidator
{
    private readonly HashSet<string> _labels = [];
    private readonly HashSet<string> _gotos = [];
    
    public static bool TryValidate(ProgramNode program)
    {
        try
        {
            var analyzer = new LabelValidator();      
            analyzer.Validate(program);
            return true;           
        }
        catch (FormatException ex)
        {
            PrintError(ex.Message);
        }
        return false;   
    }

    public void Validate(ProgramNode program)
    {
        foreach (var function in program.Nodes.OfType<FunctionDeclarationNode>())
            ValidateFunction(function);
    }
    
    private void ValidateFunction(FunctionDeclarationNode function)
    {
        Clear();
        
        if (function.Body is { } body)
            VisitBlock(body);

        foreach (var label in _gotos)
        {
            if (!_labels.Contains(label))
                throw new FormatException($"Label '{label}' used but not defined");
        }       
    }

    private void VisitBlock(BlockNode block)
    {
        foreach (var statement in block.Items.OfType<IStatementNode>())
            VisitStatement(statement);
    }

    private void VisitStatement(IStatementNode? statement)
    {
        switch (statement)
        {
            case IfNode node:
                VisitIf(node);
                break;
            case GotoNode node:
                _gotos.Add(node.Label);
                break;
            case LabelNode node:
                VisitLabel(node);
                break;
            case CompoundNode node:
                VisitBlock(node.Block);
                break;
            case WhileNode node:
                ValidateWhile(node);
                break;
            case DoWhileNode node:
                ValidateDoWhile(node);
                break;
            case ForNode node:
                ValidateFor(node);
                break;
            case BreakNode node:
                ValidateBreakLabel(node);
                break;
            case ContinueNode node:
                ValidateContinueLabel(node);
                break;
            case SwitchNode node:
                ValidateSwitch(node);
                break;
            case CaseNode node:
                ValidateCase(node);
                break;
            case DefaultNode node:
                ValidateDefault(node);
                break;
            default:
                return;
        }
    }

    private void ValidateSwitch(SwitchNode node)
    {
        if (node.Cases is { Count: > 0 })
        {
            StringBuilder? sb = null;
            if (node.Cases.Count(c => c.Label.EndsWith("default")) > 1)
            {
                sb = new StringBuilder();
                sb.AppendLine("multiple default labels in one switch");
            }
            
            Dictionary<int, (SwitchLabel, int)> lookup = new(node.Cases.Count);
            foreach (var @case in node.Cases.Where(c => c.CalculatedValue.HasValue))
            {
                lookup.AddOrUpdate
                (
                    @case.CalculatedValue!.Value, 
                    _ => (@case, 1), 
                    (_, prev) => (prev.Item1, prev.Item2 + 1)
                );
            }
            
            foreach (var (@case, count) in lookup.Values)
            {
                if (count == 1) continue;
                
                sb ??= new StringBuilder();               
                sb.AppendLine($"duplicate case value: {@case.CalculatedValue}");
            }
            if (sb is not null)
                throw new FormatException(sb.ToString().TrimEnd());
        }
        VisitStatement(node.Body);
    }

    private void ValidateDefault(DefaultNode node)
    {
        if (node.Label is null)
            throw new FormatException("default statement not within switch");
        
        VisitStatement(node.Statement);
    }

    private void ValidateCase(CaseNode node)
    {
        if (node.Label is null)
            throw new FormatException("case statement not within switch");
        
        VisitStatement(node.Statement);
    }
    
    private void ValidateFor(ForNode node)
        => VisitStatement(node.Body);
    
    private void ValidateDoWhile(DoWhileNode node)
        => VisitStatement(node.Body);

    private void ValidateWhile(WhileNode node)
        => VisitStatement(node.Body);
    
    private static void ValidateContinueLabel(ContinueNode node)
    {
        if (node is { Label: null })
            throw new FormatException("continue statement not within a loop");
    }

    private static void ValidateBreakLabel(BreakNode node)
    {
        if (node is { Label: null })
            throw new FormatException("break statement not within loop or switch");
    }
    
    private void VisitLabel(LabelNode node)
    { 
        if (!_labels.Add(node.Name))
            throw new FormatException($"Duplicate label: {node.Name}");
        
        VisitStatement(node.Statement);
    }
    

    private void VisitIf(IfNode @if)
    {
        VisitStatement(@if.Then);
        VisitStatement(@if.Else);
    }

    private void Clear()
    {
        _labels.Clear();
        _gotos.Clear();
    }
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}