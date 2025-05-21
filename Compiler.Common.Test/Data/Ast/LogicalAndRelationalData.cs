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
                        Constant(10),
                        Constant(0)
                    ),
                    new BinaryNode
                    (
                        LogicalAndNode.Operator,
                        Constant(0),
                        Constant(4)
                    )
                ),
                new BinaryNode
                (
                    LogicalAndNode.Operator,
                    Constant(0),
                    Constant(0)
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
                Constant(0),
                new BinaryNode
                (
                    DivisionNode.Operator,
                    Constant(1),
                    Constant(0)
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
                Constant(1),
                new UnaryNode
                (
                    NegateNode.Operator,
                    Constant(1)
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
                        Constant(5),
                        Constant(0)
                    ),
                    Constant(1)
                ),
                Constant(0)
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
                    new UnaryNode(ComplementNode.Operator, Constant(2)),
                    new UnaryNode(NegateNode.Operator, Constant(2))
                ),
                new BinaryNode
                (
                    AdditionNode.Operator, 
                    Constant(1),
                    Constant(5)
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
                Constant(1),
                Constant(2)
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
                    Constant(3),
                    Constant(1)
                ),
                Constant(2)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1 == 1;
            }
            """,
            GetExpected(new BinaryNode(EqualNode.Operator, Constant(1), Constant(1)))
        );
        Add
        (
            """
            int main(void) {
                return 1 >= 2;
            }
            """,
            GetExpected(new BinaryNode(GreaterThanOrEqualNode.Operator, Constant(1), Constant(2)))
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
                    Constant(1),
                    Constant(1)
                ),
                new BinaryNode
                (
                    GreaterThanOrEqualNode.Operator,
                    Constant(1),
                    new UnaryNode(NegateNode.Operator, Constant(4))
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
                    Constant(1),
                    Constant(2)
                ),
                new BinaryNode
                (
                    GreaterThanNode.Operator,
                    Constant(1),
                    Constant(1)
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
            GetExpected(new BinaryNode(GreaterThanNode.Operator, Constant(15), Constant(10)))
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
                Constant(1), 
                new UnaryNode(NegateNode.Operator, Constant(1))
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
                    Constant(0),
                    Constant(2)
                ),
                new BinaryNode
                (
                    LessThanOrEqualNode.Operator, 
                    Constant(0),
                    Constant(0)
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
            GetExpected(new BinaryNode(LessThanNode.Operator, Constant(2), Constant(1)))
        );
        Add
        (
            """
            int main(void) {
                return 1 < 2;
            }
            """,
            GetExpected(new BinaryNode(LessThanNode.Operator, Constant(1), Constant(2)))
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
                Constant(0),
                new BinaryNode
                (
                    LogicalAndNode.Operator,
                    Constant(0),
                    new BinaryNode(DivisionNode.Operator, Constant(1), Constant(0))
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
            GetExpected(new BinaryNode(NotEqualNode.Operator, Constant(0), Constant(0)))
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
                new UnaryNode(NegateNode.Operator, Constant(1)), 
                new UnaryNode(NegateNode.Operator, Constant(2))
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
                new UnaryNode(NegateNode.Operator, Constant(3))
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
                new BinaryNode(SubtractionNode.Operator, Constant(3), Constant(44))
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
                new BinaryNode(SubtractionNode.Operator, Constant(4), Constant(4))
            ))
        );
        Add
        (
            """
            int main(void) {
                return !0;
            }
            """,
            GetExpected(new UnaryNode(NotNode.Operator, Constant(0)))
        );
        Add
        (
            """
            int main(void) {
                return !5;
            }
            """,
            GetExpected(new UnaryNode(NotNode.Operator, Constant(5)))
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
                        Constant(0),
                        Constant(1)
                    )
                ),
                new UnaryNode
                (
                    NegateNode.Operator,
                    new BinaryNode
                    (
                        LogicalOrNode.Operator,
                        Constant(4),
                        Constant(3)
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
                Constant(1),
                new BinaryNode
                (
                    DivisionNode.Operator, 
                    Constant(1),
                    Constant(0)
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
                        Constant(4),
                        Constant(0)
                    ),
                    new BinaryNode
                    (
                        LogicalOrNode.Operator,
                        Constant(0),
                        Constant(3)
                    )
                ),
                new BinaryNode
                (
                    LogicalOrNode.Operator,
                    Constant(5),
                    Constant(5)
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
                    Constant(1),
                    Constant(0)
                ),
                Constant(0)
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
                Constant(2),
                new BinaryNode(GreaterThanOrEqualNode.Operator, Constant(2), Constant(0))
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
                new BinaryNode(EqualNode.Operator, Constant(2), Constant(2)),
                Constant(0)
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
                    new BinaryNode(EqualNode.Operator, Constant(0), Constant(0)),
                    new BinaryNode
                    (
                        EqualNode.Operator,
                        Constant(3),
                        new BinaryNode
                        (
                            GreaterThanNode.Operator,
                            new BinaryNode(AdditionNode.Operator, Constant(2), Constant(1)),
                            Constant(1)
                        )
                    )
                ),
                Constant(1)
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
                Constant(1),
                new BinaryNode(LogicalAndNode.Operator, Constant(0), Constant(2))
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
                Constant(5),
                new BinaryNode(EqualNode.Operator, Constant(7), Constant(5))
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
                Constant(5),
                new BinaryNode(NotEqualNode.Operator, Constant(7), Constant(5))
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
                new BitwiseNode(BitwiseRightShiftNode.Operator, Constant(20), Constant(4)),
                new BitwiseNode(BitwiseLeftShiftNode.Operator, Constant(3), Constant(1))
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
                Constant(5),
                new BinaryNode(LessThanNode.Operator, Constant(7), Constant(5))
            ))
        );
    }
}