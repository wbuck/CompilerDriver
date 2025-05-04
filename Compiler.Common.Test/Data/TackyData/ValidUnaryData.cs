using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.TackyData;

public class ValidUnaryData : DataBase
{
    public ValidUnaryData()
    {
        Add
        (
            """
            int main(void) {
                return 42;
            }
            """,
            Create([new TackyReturn(Constant(42))])
        );
        Add
        (
            """
            int main(void) {
                return -42;
            }
            """,
            Create([
                new TackyUnary(TackyNegate.Operator, Constant(42), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~42;
            }
            """,
            Create([
                new TackyUnary(TackyComplement.Operator, Constant(42), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~-42;
            }
            """,
            Create([
                new TackyUnary(TackyNegate.Operator, Constant(42), Variable(1)),
                new TackyUnary(TackyComplement.Operator, Variable(1), Variable(2)),
                new TackyReturn(Variable(2)),
            ])
        );
        Add
        (
            """
            int main(void) {
                return -(-42);
            }
            """,
            Create([
                new TackyUnary(TackyNegate.Operator, Constant(42), Variable(1)),
                new TackyUnary(TackyNegate.Operator, Variable(1), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
    }
}