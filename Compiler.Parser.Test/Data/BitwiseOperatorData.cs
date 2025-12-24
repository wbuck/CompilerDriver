using Compiler.Parser.Nodes;

namespace Compiler.Parser.Test.Data;

public class BitwiseOperatorData : DataBase
{
    public BitwiseOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 3 & 5;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseAndNode.Operator, Const(3), Const(5)))
        );
        Add
        (
            """
            int main(void) {
                return 1 | 2;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseOrNode.Operator, Const(1), Const(2)))
        );
        Add
        (
            """
            int main(void) {
                return 80 >> 2 | 1 ^ 5 & 7 << 1;
            }
            """,
            GetExpected(new BitwiseNode
            (
                BitwiseOrNode.Operator, 
                new BitwiseNode(BitwiseRightShiftNode.Operator, Const(80), Const(2)), 
                new BitwiseNode
                    (
                        BitwiseXorNode.Operator,
                        Const(1),
                        new BitwiseNode
                            (
                                BitwiseAndNode.Operator,
                                Const(5),
                                new BitwiseNode
                                    (
                                        BitwiseLeftShiftNode.Operator,
                                        Const(7),
                                        Const(1)
                                    )
                            )
                    )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 33 >> 2 << 1;
            }
            """,
            GetExpected(new BitwiseNode
            (
                BitwiseLeftShiftNode.Operator, 
                new BitwiseNode(BitwiseRightShiftNode.Operator, Const(33), Const(2)), 
                Const(1)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 33 << 4 >> 2;
            }
            """,
            GetExpected(new BitwiseNode
            (
                BitwiseRightShiftNode.Operator, 
                new BitwiseNode(BitwiseLeftShiftNode.Operator, Const(33), Const(4)), 
                Const(2)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 40 << 4 + 12 >> 1;
            }
            """,
            GetExpected(new BitwiseNode
            (
                BitwiseRightShiftNode.Operator, 
                new BitwiseNode
                    (
                        BitwiseLeftShiftNode.Operator, 
                        Const(40), 
                        new BinaryNode(AdditionNode.Operator, Const(4), Const(12))
                    ), 
                Const(1)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 35 << 2;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseLeftShiftNode.Operator, Const(35), Const(2)))
        );
        Add
        (
            """
            int main(void) {
                return -5 >> 30;
            }
            """,
            GetExpected(new BitwiseNode
            (
                BitwiseRightShiftNode.Operator, 
                new UnaryNode(NegateNode.Operator, Const(5)), 
                Const(30)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1000 >> 4;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseRightShiftNode.Operator, Const(1000), Const(4)))
        );
        Add
        (
            """
            int main(void) {
                return (4 << (2 * 2)) + (100 >> (1 + 2));
            }
            """,
            GetExpected(new BinaryNode
            (
                AdditionNode.Operator,
                new BitwiseNode
                    (
                        BitwiseLeftShiftNode.Operator, 
                        Const(4), 
                        new BinaryNode(MultiplicationNode.Operator, Const(2), Const(2))
                    ),
                new BitwiseNode
                    (
                        BitwiseRightShiftNode.Operator, 
                        Const(100),
                        new BinaryNode(AdditionNode.Operator, Const(1), Const(2))
                    )
            ))
        );
        Add
        (
            """
            int main(void) {
                return 7 ^ 1;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseXorNode.Operator, Const(7), Const(1)))
        );
    }
}