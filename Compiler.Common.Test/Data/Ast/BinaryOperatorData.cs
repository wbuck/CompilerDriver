using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.Ast;

public class BinaryOperatorData : DataBase
{ 
    public BinaryOperatorData()
    {        
        Add
        (
            """
            int main(void)
            {
                return ~(1 + 1);
            }
            """,
            GetExpected
            (
                new UnaryNode(
                    ComplementNode.Operator, 
                    new BinaryNode(AdditionNode.Operator, Const(1), Const(1)))
            )
        );
        Add
        (
            """
            int main(void)
            {
                return ~2 + 3;
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    AdditionNode.Operator,
                    new UnaryNode(ComplementNode.Operator, Const(2)),
                    Const(3)
                )
            )
        );
        Add
        (
            """
            int main(void)
            {
                return 1 - 2;
            }
            """,
            GetExpected(new BinaryNode(SubtractionNode.Operator, Const(1), Const(2)))
        );
        Add
        (
            """
            int main(void)
            {
                return 2- -1;
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    SubtractionNode.Operator,
                    Const(2),
                    new UnaryNode(NegateNode.Operator, Const(1))
                )
            )
        );
        Add
        (
            """
            int main(void)
            {
                return 2 + 3 * 4;
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    AdditionNode.Operator,
                    Const(2),
                    new BinaryNode(MultiplicationNode.Operator, Const(3), Const(4))
                )
            )
        );
        Add
        (
            """
            int main(void)
            {
                return 2 * (3 + 4);
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    MultiplicationNode.Operator, 
                    Const(2), 
                    new BinaryNode(AdditionNode.Operator, Const(3), Const(4))
                )
            )
        );
        Add
        (
            """
            int main(void)
            {
                return 2 * 3;
            }
            """,
            GetExpected(new BinaryNode(MultiplicationNode.Operator, Const(2), Const(3)))
        );
        Add
        (
            """
            int main(void)
            {
                return 4 % 2;
            }
            """,
            GetExpected(new BinaryNode(RemainderNode.Operator, Const(4), Const(2)))
        );
        Add
        (
            """
            int main(void)
            {
                return 4 / 2;
            }
            """,
            GetExpected(new BinaryNode(DivisionNode.Operator, Const(4), Const(2)))
        );
        Add
        (
            """
            int main(void)
            {
                return (-12) / 5;
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    DivisionNode.Operator,
                    new UnaryNode(NegateNode.Operator, Const(12)),
                    Const(5)
                )
            )
        ); 
        Add
        (            
            """
            int main(void)
            {
                return 1 - 2 - 3;
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    SubtractionNode.Operator,
                    new BinaryNode
                    (
                        SubtractionNode.Operator,
                        Const(1),
                        Const(2)
                    ),
                    Const(3)
                )
            )
        );          
        Add
        (
            """
            int main(void) {
                return 1 + 2;
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    AdditionNode.Operator, 
                    Const(1), 
                    Const(2)
                )
            )
        );
        Add
        (
            """
            int main(void) {
                return 6 / 3 / 2;
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    DivisionNode.Operator, 
                    new BinaryNode
                    (
                        DivisionNode.Operator,
                        Const(6),
                        Const(3)
                    ), 
                    Const(2)
                )
            )
        );
        Add
        (
            """
            int main(void) {
                return (3 / 2 * 4) + (5 - 4 + 3);
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    AdditionNode.Operator, 
                    new BinaryNode
                    (
                        MultiplicationNode.Operator,
                        new BinaryNode
                        (
                            DivisionNode.Operator,
                            Const(3),
                            Const(2)
                        ),
                        Const(4)
                    ),
                    new BinaryNode
                    (
                        AdditionNode.Operator,
                        new BinaryNode
                        (
                            SubtractionNode.Operator,
                            Const(5),
                            Const(4)
                        ),
                        Const(3)
                    )
                )
            )
        );
        Add
        (
            """
            int main(void)
            {
                return 5 * 4 / 2 - 
                    3 % (2 + 1);
            }
            """,
            GetExpected
            (
                new BinaryNode
                (
                    SubtractionNode.Operator,
                    new BinaryNode
                    (
                        DivisionNode.Operator,
                        new BinaryNode
                        (
                            MultiplicationNode.Operator, 
                            Const(5), 
                            Const(4)
                        ),
                        Const(2)
                    ),
                    new BinaryNode
                    (
                        RemainderNode.Operator,
                        Const(3),
                        new BinaryNode
                        (
                            AdditionNode.Operator,
                            Const(2),
                            Const(1)
                        )                            
                    )
                )
            )
        ); 
    }
}