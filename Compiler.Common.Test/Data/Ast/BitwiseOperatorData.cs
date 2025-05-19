using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.Ast;

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
            GetExpected(new BitwiseNode(BitwiseAndNode.Operator, Constant(3), Constant(5)))
        );
        Add
        (
            """
            int main(void) {
                return 1 | 2;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseOrNode.Operator, Constant(1), Constant(2)))
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
                new BitwiseNode(BitwiseRightShiftNode.Operator, Constant(80), Constant(2)), 
                new BitwiseNode
                    (
                        BitwiseXorNode.Operator,
                        Constant(1),
                        new BitwiseNode
                            (
                                BitwiseAndNode.Operator,
                                Constant(5),
                                new BitwiseNode
                                    (
                                        BitwiseLeftShiftNode.Operator,
                                        Constant(7),
                                        Constant(1)
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
                new BitwiseNode(BitwiseRightShiftNode.Operator, Constant(33), Constant(2)), 
                Constant(1)
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
                new BitwiseNode(BitwiseLeftShiftNode.Operator, Constant(33), Constant(4)), 
                Constant(2)
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
                        Constant(40), 
                        new BinaryNode(AdditionNode.Operator, Constant(4), Constant(12))
                    ), 
                Constant(1)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 35 << 2;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseLeftShiftNode.Operator, Constant(35), Constant(2)))
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
                new UnaryNode(NegateNode.Operator, Constant(5)), 
                Constant(30)
            ))
        );
        Add
        (
            """
            int main(void) {
                return 1000 >> 4;
            }
            """,
            GetExpected(new BitwiseNode(BitwiseRightShiftNode.Operator, Constant(1000), Constant(4)))
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
                        Constant(4), 
                        new BinaryNode(MultiplicationNode.Operator, Constant(2), Constant(2))
                    ),
                new BitwiseNode
                    (
                        BitwiseRightShiftNode.Operator, 
                        Constant(100),
                        new BinaryNode(AdditionNode.Operator, Constant(1), Constant(2))
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
            GetExpected(new BitwiseNode(BitwiseXorNode.Operator, Constant(7), Constant(1)))
        );
    }
}