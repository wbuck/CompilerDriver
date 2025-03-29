using Compiler.Common.Ast;
using Compiler.Common.Generation;

namespace Compiler.Common.Stages;

public static class Generator
{
    public static Program Visit(INode input)
    {
        if (input is not ProgramNode program)
            throw new InvalidOperationException();
                
        foreach (var node in program.Nodes)
        {
            if (node is FunctionNode function)
                return new Program(VisitFunction(function));
        }
        throw new NotImplementedException();
    }

    private static Function VisitFunction(FunctionNode node)
    {
        if (node.Body is not BlockStatementNode block)
            throw new InvalidOperationException();
        
        var instructions = VisitBlockStatement(block);
        instructions.Add(new Ret());
        
        return new Function(node.Name, instructions);
    }

    private static List<IInstruction> VisitBlockStatement(BlockStatementNode block)
    {
        if (block.Body is [ReturnNode { Expression: not null and (IntegerConstantNode or FloatConstantNode) }] expression)
            return [new Mov(VisitExpression(((ReturnNode)expression[0]).Expression!), new Register())];
        
        throw new NotImplementedException();
    }

    private static IOperand VisitExpression(INode expression) =>
        expression switch
        {
            IntegerConstantNode integer => new Imm<int>(integer.Value),
            FloatConstantNode floating => new Imm<double>(floating.Value),
            _ => throw new NotImplementedException()
        };
}