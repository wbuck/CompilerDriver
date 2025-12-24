using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

public class UnaryOperatorData : DataBase
{
    public UnaryOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 42;
            }
            """,
            GetExpected([new TackyReturn(Const(42))])
        );
        Add
        (
            """
            int main(void) {
                return -42;
            }
            """,
            GetExpected([
                new TackyUnary(TackyNegate.Operator, Const(42), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~42;
            }
            """,
            GetExpected([
                new TackyUnary(TackyComplement.Operator, Const(42), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~-42;
            }
            """,
            GetExpected([
                new TackyUnary(TackyNegate.Operator, Const(42), Var(1)),
                new TackyUnary(TackyComplement.Operator, Var(1), Var(2)),
                new TackyReturn(Var(2)),
            ])
        );
        Add
        (
            """
            int main(void) {
                return -(-42);
            }
            """,
            GetExpected([
                new TackyUnary(TackyNegate.Operator, Const(42), Var(1)),
                new TackyUnary(TackyNegate.Operator, Var(1), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
    }
}