using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.Ast;

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
}