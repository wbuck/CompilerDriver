using Compiler.Common.Ast;

namespace Compiler.Common.Stages;

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

    public void Validate(ProgramNode program) =>
        ValidateFunction(program.Function);
    
    private void ValidateFunction(FunctionNode function)
    {
        Clear();
        VisitBlock(function.Body);

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
            default:
                return;
        }
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