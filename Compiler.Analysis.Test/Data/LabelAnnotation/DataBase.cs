using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Test.Data.LabelAnnotation;

public class DataBase : TheoryData<string, ProgramNode>
{
    protected static BreakNode Break(string? label = null) 
        => new(label);
    
    protected static ContinueNode Continue(string? label = null)
        => new(label);
    
    protected static UnaryNode Complement(IExpressionNode expression)
        => new(ComplementNode.Operator, expression);

    protected static UnaryNode Negate(IExpressionNode expression)
        => new(NegateNode.Operator, expression);
    
    protected static ConstantNode<int> Const(int value) =>
        new(value);
    
    protected static ReturnNode Ret(IExpressionNode value) =>
        new(value);
    
    protected static VariableNode Var(string identifier) =>
        new(identifier);
    
    protected static CompoundNode Compound(params IBlockItem[] items)
        => new(new BlockNode(items.ToList()));
    
    protected static ProgramNode GetExpected(params List<IBlockItem> body) =>
        new([new FunctionDeclarationNode("main", "int", [], new BlockNode(body))]);
    
    protected static ProgramNode GetExpected(IExpressionNode expression) =>
        new([new FunctionDeclarationNode("main", "int", [], new BlockNode([new ReturnNode(expression)]))]);
}