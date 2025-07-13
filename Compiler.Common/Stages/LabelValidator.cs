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
        foreach (var statement in function.Body.OfType<IStatementNode>())
            VisitStatement(statement);

        foreach (var label in _gotos)
        {
            if (!_labels.Contains(label))
                throw new FormatException($"Label '{label}' used but not defined");
        }       
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
            default:
                return;
        }
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