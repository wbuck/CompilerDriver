using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public class ValidBinaryOperationData : TheoryData<string, ProgramNode>
{ 
    public ValidBinaryOperationData()
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
                new UnaryNode(ComplementNode.Operator, new BinaryNode(AdditionNode.Operator, Constant(1), Constant(1)))
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
                    new UnaryNode(ComplementNode.Operator, Constant(2)),
                    Constant(3)
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
            GetExpected(new BinaryNode(SubtractionNode.Operator, Constant(1), Constant(2)))
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
                    Constant(2),
                    new UnaryNode(NegateNode.Operator, Constant(1))
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
                    Constant(2),
                    new BinaryNode(MultiplicationNode.Operator, Constant(3), Constant(4))
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
                    Constant(2), 
                    new BinaryNode(AdditionNode.Operator, Constant(3), Constant(4))
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
            GetExpected(new BinaryNode(MultiplicationNode.Operator, Constant(2), Constant(3)))
        );
        Add
        (
            """
            int main(void)
            {
                return 4 % 2;
            }
            """,
            GetExpected(new BinaryNode(RemainderNode.Operator, Constant(4), Constant(2)))
        );
        Add
        (
            """
            int main(void)
            {
                return 4 / 2;
            }
            """,
            GetExpected(new BinaryNode(DivisionNode.Operator, Constant(4), Constant(2)))
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
                    new UnaryNode(NegateNode.Operator, Constant(12)),
                    Constant(5)
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
                        Constant(1),
                        Constant(2)
                    ),
                    Constant(3)
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
                    Constant(1), 
                    Constant(2)
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
                        Constant(6),
                        Constant(3)
                    ), 
                    Constant(2)
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
                            Constant(3),
                            Constant(2)
                        ),
                        Constant(4)
                    ),
                    new BinaryNode
                    (
                        AdditionNode.Operator,
                        new BinaryNode
                        (
                            SubtractionNode.Operator,
                            Constant(5),
                            Constant(4)
                        ),
                        Constant(3)
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
                            Constant(5), 
                            Constant(4)
                        ),
                        Constant(2)
                    ),
                    new BinaryNode
                    (
                        RemainderNode.Operator,
                        Constant(3),
                        new BinaryNode
                        (
                            AdditionNode.Operator,
                            Constant(2),
                            Constant(1)
                        )                            
                    )
                )
            )
        ); 
    }
    
    private static ConstantNode<int> Constant(int value) =>
        new(value);
    
    private static ProgramNode GetExpected(IExpressionNode expression) =>
        new(new FunctionNode("main", "int", new ReturnNode(expression)));

}