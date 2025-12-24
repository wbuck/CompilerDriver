using Compiler.Parser.Nodes;

namespace Compiler.Parser.Test.Data;

public class LocalVariableData : DataBase
{
    public LocalVariableData()
    {
        Add
        (
            """
            int main(void) {
                int first_variable = 1;
                int second_variable = 2;
                return first_variable + second_variable;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("first_variable", Const(1)),
                new VariableDeclarationNode("second_variable", Const(2)),
                Ret(new BinaryNode(AdditionNode.Operator, Var("first_variable"), Var("second_variable"))))
        );
        Add
        (
            """
            int main(void) {
                int a = 2147483646;
                int b = 0;
                int c = a / 6 + !b;
                return c * 2 == a - 1431655762;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(2147483646)),
                new VariableDeclarationNode("b", Const(0)),
                new VariableDeclarationNode
                (
                   "c", 
                   new BinaryNode
                   (
                       AdditionNode.Operator,
                       new BinaryNode(DivisionNode.Operator, Var("a"),Const(6)),
                       new UnaryNode(NotNode.Operator, Var("b"))
                   )
                ),
                Ret
                (
                    new BinaryNode
                    (
                        EqualNode.Operator,
                        new BinaryNode(MultiplicationNode.Operator, Var("c"), Const(2)),
                        new BinaryNode(SubtractionNode.Operator, Var("a"), Const(1431655762))
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = a = 5;
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", new AssignmentNode(Var("a"), Const(5))),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int var0;
                var0 = 2;
                return var0;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("var0"),
                new ExpressionNode(new AssignmentNode(Var("var0"), Const(2))),
                Ret(Var("var0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a;
                int b = a = 0;
                return b;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a"),
                new VariableDeclarationNode("b", new AssignmentNode(Var("a"), Const(0))),
                Ret(Var("b"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a;
                a = 0 || 5;
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a"),
                new ExpressionNode
                (
                    new AssignmentNode
                    (
                        Var("a"), 
                        new BinaryNode(LogicalOrNode.Operator, Const(0), Const(5))
                    )
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
            }
            """,
            GetExpected(new List<IBlockItem>())
        );
        Add
        (
            """
            int main(void) {
                int a = -2593;
                a = a % 3;
                int b = -a;
                return b;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", new UnaryNode(NegateNode.Operator, Const(2593))),
                new ExpressionNode
                (
                    new AssignmentNode
                   (
                       Var("a"), 
                       new BinaryNode
                           (
                               RemainderNode.Operator, 
                               Var("a"), 
                               Const(3)
                           )
                   )
                ),
                new VariableDeclarationNode("b", new UnaryNode(NegateNode.Operator, Var("a"))),
                Ret(Var("b"))
            )
        );
        Add
        (
            """
            int main(void) {
                int return_val = 3;
                int void2 = 2;
                return return_val + void2;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("return_val", Const(3)),
                new VariableDeclarationNode("void2", Const(2)),
                Ret(new BinaryNode(AdditionNode.Operator, Var("return_val"), Var("void2")))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                a = a + 5;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(3)),
                new ExpressionNode
                (
                    new AssignmentNode
                    (
                        Var("a"), 
                        new BinaryNode(AdditionNode.Operator, Var("a"), Const(5))
                    )
                )                
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 0;
                a = 3 * (b = a);
                return a + b;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(1)),
                new VariableDeclarationNode("b", Const(0)),
                new ExpressionNode
                (
                    new AssignmentNode
                    (
                        Var("a"),
                        new BinaryNode
                            (
                                MultiplicationNode.Operator, 
                                Const(3),
                                new AssignmentNode(Var("b"), Var("a"))
                            )
                    )
                ),
                Ret(new BinaryNode(AdditionNode.Operator, Var("a"), Var("b")))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                0 || (a = 1);
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(0)),
                new ExpressionNode
                (
                    new BinaryNode
                    (
                        LogicalOrNode.Operator,
                        Const(0),
                        new AssignmentNode(Var("a"), Const(1))
                    )
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                ;
            }
            """,
            GetExpected(NullNode.Statement)
        );
        Add
        (
            """
            int main(void) {
                ;
                return 0;
            }
            """,
            GetExpected(NullNode.Statement, Ret(Const(0)))
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(2)),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                0 && (a = 5);
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(0)),
                new ExpressionNode
                (
                    new BinaryNode
                    (
                        LogicalAndNode.Operator,
                        Const(0),
                        new AssignmentNode(Var("a"), Const(5))
                    )
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                1 || (a = 1);
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(0)),
                new ExpressionNode
                (
                    new BinaryNode
                    (
                        LogicalOrNode.Operator,
                        Const(1),
                        new AssignmentNode(Var("a"), Const(1))
                    )
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                2 + 2;
                return 0;
            }
            """,
            GetExpected
            (
                new ExpressionNode(new BinaryNode(AdditionNode.Operator, Const(2), Const(2))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
            
                int a = 1;
                int b = 2;
                return a = b = 4;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(1)),
                new VariableDeclarationNode("b", Const(2)),
                Ret
                (
                    new AssignmentNode
                    (
                        Var("a"),
                        new AssignmentNode(Var("b"), Const(4))
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0 && a;
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", new BinaryNode(LogicalAndNode.Operator, Const(0), Var("a"))),
                Ret(Var("a"))
            )
        );
    }
}