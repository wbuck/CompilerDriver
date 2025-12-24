using Compiler.Parser.Nodes;

namespace Compiler.Parser.Test.Data;

public class UnaryOperatorData : DataBase
{
    public UnaryOperatorData()
    {
        Add
        (
            """
            int main(void)
            {
                return -((((10))));
            }
            """,
            GetExpected(new UnaryNode(NegateNode.Operator, Const(10)))
        );
        Add
        (
            """
            int main(void)
            {
                return (-2);
            }
            """,
            GetExpected(new UnaryNode(NegateNode.Operator, Const(2)))
        );
        Add
        (
            """
            int main(void) {
                return -(-4);
            }
            """,
            GetExpected(Negate(Negate(Const(4))))
        );
        Add
        (
            """
            int main(void) {
                return ~(2);
            }
            """,
            GetExpected(Complement(Const(2)))
        );
        Add
        (
            """
            int main(void) {
                return -1000;
            }
            """,
            GetExpected(Negate(Const(1000)))
        );
        Add
        (
            """
            int main(void) {
                return ~1000;
            }
            """,
            GetExpected(Complement(Const(1000)))
        );
        Add
        (
            """
            int main(void) {
                return ~-2147483647;
            }
            """,
            GetExpected(Complement(Negate(Const(2147483647))))
        );
    }
}