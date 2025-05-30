using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.Ast;

public class LogicalAndRelationalData : DataBase
{
    public LogicalAndRelationalData()
    {
        Add
        (
            """
            int main(void) {
                return (10 && 0) + (0 && 4) + (0 && 0);
            }
            """,
            GetExpected(new BinaryNode
            (
                AdditionNode.Operator,
                new BinaryNode
                (
                    AdditionNode.Operator,
                    new BinaryNode
                    (
                        LogicalAndNode.Operator,
                        Const(10),
                        Const(0)
                    ),
                    new BinaryNode
                    (
                        LogicalAndNode.Operator,
                        Const(0),
                        Const(4)
                    )
                ),
                new BinaryNode
                (
                    LogicalAndNode.Operator,
                    Const(0),
                    Const(0)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 0 && (1 / 0);
            }
            """,
            GetExpected(new BinaryNode
            (
                LogicalAndNode.Operator,
                Const(0),
                new BinaryNode
                (
                    DivisionNode.Operator,
                    Const(1),
                    Const(0)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1 && -1;
            }
            """,
            GetExpected(new BinaryNode
            (
                LogicalAndNode.Operator,
                Const(1),
                new UnaryNode
                (
                    NegateNode.Operator,
                    Const(1)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 5 >= 0 > 1 <= 0;
            }
            """,
            GetExpected(new BinaryNode
            (
                LessThanOrEqualNode.Operator, 
                new BinaryNode
                (
                    GreaterThanNode.Operator,
                    new BinaryNode
                    (
                        GreaterThanOrEqualNode.Operator, 
                        Const(5),
                        Const(0)
                    ),
                    Const(1)
                ),
                Const(0)
            ))
        );
        Add
        (
            """
            int main(void) {
                return ~2 * -2 == 1 + 5;
            }
            """,
            GetExpected(new BinaryNode
            (
                EqualNode.Operator,
                new BinaryNode
                (
                    MultiplicationNode.Operator, 
                    new UnaryNode(ComplementNode.Operator, Const(2)),
                    new UnaryNode(NegateNode.Operator, Const(2))
                ),
                new BinaryNode
                (
                    AdditionNode.Operator, 
                    Const(1),
                    Const(5)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1 == 2;
            }
            """,
            GetExpected(new BinaryNode
            (
                EqualNode.Operator,
                Const(1),
                Const(2)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 3 == 1 != 2;
            }
            """,
            GetExpected(new BinaryNode(
                NotEqualNode.Operator,
                new BinaryNode
                (
                    EqualNode.Operator,
                    Const(3),
                    Const(1)
                ),
                Const(2)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1 == 1;
            }
            """,
            GetExpected(new BinaryNode(EqualNode.Operator, Const(1), Const(1)))
        );
        Add
        (
            """
            int main(void) {
                return 1 >= 2;
            }
            """,
            GetExpected(new BinaryNode(GreaterThanOrEqualNode.Operator, Const(1), Const(2)))
        );
        Add
        (
            """
            int main(void) {
                return (1 >= 1) + (1 >= -4);
            }
            """,
            GetExpected(new BinaryNode(
                AdditionNode.Operator, 
                new BinaryNode
                (
                    GreaterThanOrEqualNode.Operator,
                    Const(1),
                    Const(1)
                ),
                new BinaryNode
                (
                    GreaterThanOrEqualNode.Operator,
                    Const(1),
                    new UnaryNode(NegateNode.Operator, Const(4))
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return (1 > 2) + (1 > 1);
            }
            """,
            GetExpected(new BinaryNode(
                AdditionNode.Operator, 
                new BinaryNode
                (
                    GreaterThanNode.Operator,
                    Const(1),
                    Const(2)
                ),
                new BinaryNode
                (
                    GreaterThanNode.Operator,
                    Const(1),
                    Const(1)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 15 > 10;
            }
            """,
            GetExpected(new BinaryNode(GreaterThanNode.Operator, Const(15), Const(10)))
        );
        Add
        (
            """
            int main(void) {
                return 1 <= -1;
            }
            """,
            GetExpected(new BinaryNode(
                LessThanOrEqualNode.Operator, 
                Const(1), 
                new UnaryNode(NegateNode.Operator, Const(1))
            ))
        );
        Add
        (
            """
            int main(void) {
                return (0 <= 2) + (0 <= 0);
            }
            """,
            GetExpected(new BinaryNode(
                AdditionNode.Operator, 
                new BinaryNode
                (
                    LessThanOrEqualNode.Operator, 
                    Const(0),
                    Const(2)
                ),
                new BinaryNode
                (
                    LessThanOrEqualNode.Operator, 
                    Const(0),
                    Const(0)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 2 < 1;
            }
            """,
            GetExpected(new BinaryNode(LessThanNode.Operator, Const(2), Const(1)))
        );
        Add
        (
            """
            int main(void) {
                return 1 < 2;
            }
            """,
            GetExpected(new BinaryNode(LessThanNode.Operator, Const(1), Const(2)))
        );
        Add
        (
            """
            int main(void) {
                return 0 || 0 && (1 / 0);
            }
            """,
            GetExpected(new BinaryNode(
                LogicalOrNode.Operator,
                Const(0),
                new BinaryNode
                (
                    LogicalAndNode.Operator,
                    Const(0),
                    new BinaryNode(DivisionNode.Operator, Const(1), Const(0))
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 0 != 0;
            }
            """,
            GetExpected(new BinaryNode(NotEqualNode.Operator, Const(0), Const(0)))
        );
        Add
        (
            """
            int main(void) {
                return -1 != -2;
            }
            """,
            GetExpected(new BinaryNode(
                NotEqualNode.Operator, 
                new UnaryNode(NegateNode.Operator, Const(1)), 
                new UnaryNode(NegateNode.Operator, Const(2))
            ))
        );
        Add
        (
            """
            int main(void) {
                return !-3;
            }
            """,
            GetExpected(new UnaryNode(
                NotNode.Operator, 
                new UnaryNode(NegateNode.Operator, Const(3))
            ))
        );
        Add
        (
            """
            int main(void) {
                return !(3 - 44);
            }
            """,
            GetExpected(new UnaryNode(
                NotNode.Operator,
                new BinaryNode(SubtractionNode.Operator, Const(3), Const(44))
            ))
        );
        Add
        (
            """
            int main(void) {
                return !(4-4);
            }
            """,
            GetExpected(new UnaryNode(
                NotNode.Operator,
                new BinaryNode(SubtractionNode.Operator, Const(4), Const(4))
            ))
        );
        Add
        (
            """
            int main(void) {
                return !0;
            }
            """,
            GetExpected(new UnaryNode(NotNode.Operator, Const(0)))
        );
        Add
        (
            """
            int main(void) {
                return !5;
            }
            """,
            GetExpected(new UnaryNode(NotNode.Operator, Const(5)))
        );
        Add
        (
            """
            int main(void) {
                return ~(0 && 1) - -(4 || 3);
            }
            """,
            GetExpected(new BinaryNode(
                SubtractionNode.Operator, 
                new UnaryNode
                (
                    ComplementNode.Operator,
                    new BinaryNode
                    (
                        LogicalAndNode.Operator,
                        Const(0),
                        Const(1)
                    )
                ),
                new UnaryNode
                (
                    NegateNode.Operator,
                    new BinaryNode
                    (
                        LogicalOrNode.Operator,
                        Const(4),
                        Const(3)
                    )
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1 || (1 / 0);
            }
            """,
            GetExpected(new BinaryNode(
                LogicalOrNode.Operator, 
                Const(1),
                new BinaryNode
                (
                    DivisionNode.Operator, 
                    Const(1),
                    Const(0)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return (4 || 0) + (0 || 3) + (5 || 5);
            }
            """,
            GetExpected(new BinaryNode(
                AdditionNode.Operator,
                new BinaryNode
                (
                    AdditionNode.Operator,
                    new BinaryNode
                    (
                        LogicalOrNode.Operator,
                        Const(4),
                        Const(0)
                    ),
                    new BinaryNode
                    (
                        LogicalOrNode.Operator,
                        Const(0),
                        Const(3)
                    )
                ),
                new BinaryNode
                (
                    LogicalOrNode.Operator,
                    Const(5),
                    Const(5)
                )
            ))
        );
        Add
        (
            """
            int main(void) {
                return (1 || 0) && 0;
            }
            """,
            GetExpected(new BinaryNode(
                LogicalAndNode.Operator,
                new BinaryNode
                (
                    LogicalOrNode.Operator,
                    Const(1),
                    Const(0)
                ),
                Const(0)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 >= 0;
            }
            """,
            GetExpected(new BinaryNode(
                EqualNode.Operator,
                Const(2),
                new BinaryNode(GreaterThanOrEqualNode.Operator, Const(2), Const(0))
            ))
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 || 0;
            }
            """,
            GetExpected(new BinaryNode(
                LogicalOrNode.Operator,
                new BinaryNode(EqualNode.Operator, Const(2), Const(2)),
                Const(0)
            ))
        );
        Add
        (
            """
            int main(void) {
                return (0 == 0 && 3 == 2 + 1 > 1) + 1;
            }
            """,
            GetExpected(new BinaryNode(
                AdditionNode.Operator,
                new BinaryNode
                (
                    LogicalAndNode.Operator,
                    new BinaryNode(EqualNode.Operator, Const(0), Const(0)),
                    new BinaryNode
                    (
                        EqualNode.Operator,
                        Const(3),
                        new BinaryNode
                        (
                            GreaterThanNode.Operator,
                            new BinaryNode(AdditionNode.Operator, Const(2), Const(1)),
                            Const(1)
                        )
                    )
                ),
                Const(1)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1 || 0 && 2;
            }
            """,
            GetExpected(new BinaryNode(
                LogicalOrNode.Operator,
                Const(1),
                new BinaryNode(LogicalAndNode.Operator, Const(0), Const(2))
            ))
        );
        Add
        (
            """
            int main(void) {
                return 5 & 7 == 5;
            }
            """,
            GetExpected(new BitwiseNode(
                BitwiseAndNode.Operator,
                Const(5),
                new BinaryNode(EqualNode.Operator, Const(7), Const(5))
            ))
        );
        Add
        (
            """
            int main(void) {
                return 5 | 7 != 5;
            }
            """,
            GetExpected(new BitwiseNode(
                BitwiseOrNode.Operator,
                Const(5),
                new BinaryNode(NotEqualNode.Operator, Const(7), Const(5))
            ))
        );
        Add
        (
            """
            int main(void) {
                return 20 >> 4 <= 3 << 1;
            }
            """,
            GetExpected(new BinaryNode(
                LessThanOrEqualNode.Operator,
                new BitwiseNode(BitwiseRightShiftNode.Operator, Const(20), Const(4)),
                new BitwiseNode(BitwiseLeftShiftNode.Operator, Const(3), Const(1))
            ))
        );
        Add
        (
            """
            int main(void) {
                return 5 ^ 7 < 5;
            }
            """,
            GetExpected(new BitwiseNode(
                BitwiseXorNode.Operator, 
                Const(5),
                new BinaryNode(LessThanNode.Operator, Const(7), Const(5))
            ))
        );
    }
}