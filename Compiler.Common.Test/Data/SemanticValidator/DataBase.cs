using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.SemanticValidator;

public class DataBase : TheoryData<string, ProgramNode>
{
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
    
    protected static ProgramNode GetExpected(params List<IBlockItem> body) =>
        new(new FunctionNode("main", "int", body));
    
    protected static ProgramNode GetExpected(IExpressionNode expression) =>
        new(new FunctionNode("main", "int", [new ReturnNode(expression)]));
}