using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public class ValidUnaryData : TheoryData<string, ProgramNode>
{
    public ValidUnaryData()
    {
        Add
        (
            """
            int main(void)
            {
                return -((((10))));
            }
            """,
            GetExpected(new UnaryNode(NegateNode.Operator, Constant(10)))
        );
        Add
        (
            """
            int main(void)
            {
                return (-2);
            }
            """,
            GetExpected(new UnaryNode(NegateNode.Operator, Constant(2)))
        );
        Add
        (
            """
            int main(void) {
                return -(-4);
            }
            """,
            GetExpected(Negate(Negate(Constant(4))))
        );
        Add
        (
            """
            int main(void) {
                return ~(2);
            }
            """,
            GetExpected(Complement(Constant(2)))
        );
        Add
        (
            """
            int main(void) {
                return -1000;
            }
            """,
            GetExpected(Negate(Constant(1000)))
        );
        Add
        (
            """
            int main(void) {
                return ~1000;
            }
            """,
            GetExpected(Complement(Constant(1000)))
        );
        Add
        (
            """
            int main(void) {
                return ~-2147483647;
            }
            """,
            GetExpected(Complement(Negate(Constant(2147483647))))
        );
    }
    
    private static UnaryNode Complement(IExpressionNode expression)
        => new(ComplementNode.Operator, expression);

    private static UnaryNode Negate(IExpressionNode expression)
        => new(NegateNode.Operator, expression);
    
    private static ConstantNode<int> Constant(int value) =>
        new(value);
    
    private static ProgramNode GetExpected(IExpressionNode expression) =>
        new(new FunctionNode("main", "int", new ReturnNode(expression)));

}