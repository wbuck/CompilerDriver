using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.Ast;

public class DataBase : TheoryData<string, ProgramNode>
{
    protected static UnaryNode Complement(IExpressionNode expression)
        => new(ComplementNode.Operator, expression);

    protected static UnaryNode Negate(IExpressionNode expression)
        => new(NegateNode.Operator, expression);
    
    protected static ConstantNode<int> Constant(int value) =>
        new(value);
    
    protected static ProgramNode GetExpected(IExpressionNode expression) =>
        new(new FunctionNode("main", "int", new ReturnNode(expression)));
}