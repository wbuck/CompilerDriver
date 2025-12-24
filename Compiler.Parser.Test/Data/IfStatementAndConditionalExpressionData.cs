using Compiler.Parser.Nodes;

namespace Compiler.Parser.Test.Data;

public class IfStatementAndConditionalExpressionData : DataBase
{
    public IfStatementAndConditionalExpressionData()
    {
        Add
        (
            """
            int main(void) {
                if (1 + 2 == 4)
                    return 5;
            }
            """,
            GetExpected
            (
                new IfNode
                (
                    new BinaryNode
                    (
                        EqualNode.Operator,
                        new BinaryNode(AdditionNode.Operator, Const(1), Const(2)),
                        Const(4)
                    ),
                    Ret(Const(5))
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                if (a)
                    return 1;
                else
                    return 2;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new IfNode
                (
                    Var("a"), 
                    Ret(Const(1)),
                    Ret(Const(2))
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 1;
                if (a)
                    b = 1;
                else if (~b)
                    b = 2;
                return b;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new VariableDeclarationNode("b", Const(1)),
                new IfNode
                (
                    Var("a"),
                    new ExpressionNode(new AssignmentNode(Var("b"), Const(1))),
                    new IfNode
                        (
                            new UnaryNode(ComplementNode.Operator, Var("b")),
                            new ExpressionNode(new AssignmentNode(Var("b"), Const(2)))
                        )
                ),
                Ret(Var("b"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                if ((a = 1))
                    if (a == 1)
                        a = 3;
                    else
                        a = 4;
            
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new IfNode
                (
                    new AssignmentNode(Var("a"), Const(1)),
                    new IfNode
                    (
                        new BinaryNode(EqualNode.Operator, Var("a"), Const(1)),
                        new ExpressionNode(new AssignmentNode(Var("a"), Const(3))),
                        new ExpressionNode(new AssignmentNode(Var("a"), Const(4)))
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
                if (!a)
                    if (3 / 4)
                        a = 3;
                    else
                        a = 8 / 2;
            
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new IfNode
                (
                    new UnaryNode(NotNode.Operator, Var("a")),
                    new IfNode
                    (
                        new BinaryNode(DivisionNode.Operator, Const(3), Const(4)),
                        new ExpressionNode(new AssignmentNode(Var("a"), Const(3))),
                        new ExpressionNode
                             (
                                new AssignmentNode
                                (
                                    Var("a"), 
                                    new BinaryNode(DivisionNode.Operator, Const(8), Const(2))
                                )
                             )
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
                if (0)
                    if (0)
                        a = 3;
                    else
                        a = 4;
                else
                    a = 1;
            
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new IfNode
                (
                    Const(0),
                    new IfNode
                    (
                        Const(0),
                        new ExpressionNode(new AssignmentNode(Var("a"), Const(3))),
                        new ExpressionNode(new AssignmentNode(Var("a"), Const(4)))
                    ),
                    new ExpressionNode(new AssignmentNode(Var("a"), Const(1)))
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 0;
                if (a)
                    b = 1;
                else if (b)
                    b = 2;
                return b;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(1)),
                new VariableDeclarationNode("b", Const(0)),
                new IfNode
                (
                    Var("a"),
                    new ExpressionNode(new AssignmentNode(Var("b"), Const(1))),
                    new IfNode
                    (
                        Var("b"),
                        new ExpressionNode(new AssignmentNode(Var("b"), Const(2)))
                    )
                ),
                Ret(Var("b"))
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 0;
                if (0)
                    ;
                else
                    x = 1;
                return x;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("x", Const(0)),
                new IfNode
                (
                    Const(0), 
                    NullNode.Statement,
                    new ExpressionNode(new AssignmentNode(Var("x"), Const(1)))
                ),
                Ret(Var("x"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 0;
            
                if (a)
                    a = 2;
                else
                    a = 3;
            
                if (b)
                    b = 4;
                else
                    b = 5;
            
                return a + b;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new VariableDeclarationNode("b", Const(0)),
                new IfNode
                (
                    Var("a"),
                    new ExpressionNode(new AssignmentNode(Var("a"), Const(2))),
                    new ExpressionNode(new AssignmentNode(Var("a"), Const(3)))
                ),
                new IfNode
                (
                    Var("b"),
                    new ExpressionNode(new AssignmentNode(Var("b"), Const(4))),
                    new ExpressionNode(new AssignmentNode(Var("b"), Const(5)))
                ),
                Ret(new BinaryNode(AdditionNode.Operator, Var("a"), Var("b")))
            )
        );
        Add
        (
            """
            int main(void) {
                if (1)
                    return c;
                int c = 0;
            }
            """,
            GetExpected(
                new IfNode(Const(1), Ret(Var("c"))),
                new VariableDeclarationNode("c", Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                a = 1 ? 2 : 3;
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new ExpressionNode(new AssignmentNode
                (
                    Var("a"), 
                    new ConditionalNode(Const(1), Const(2), Const(3))
                )),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 10;
                int y = 0;
                y = (x = 5) ? x : 2;
                return y;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("x", Const(10)),
                new VariableDeclarationNode("y", Const(0)),
                new ExpressionNode
                (
                    new AssignmentNode
                    (
                        Var("y"), 
                        new ConditionalNode
                            (
                                new AssignmentNode(Var("x"), Const(5)), 
                                Var("x"), 
                                Const(2))
                    )
                ),
                Ret(Var("y"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1 ? 2 ? 3 : 4 : 5;
                return 0;
            }
            """,
            GetExpected(
                new VariableDeclarationNode
                (
                    "a",
                    new ConditionalNode
                    (
                        Const(1),
                        new ConditionalNode(Const(2), Const(3), Const(4)),
                        Const(5)
                    )
                ),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                int flag = 0;
            
                return a > b ? 5 : flag ? 6 : 7;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(1)),
                new VariableDeclarationNode("b", Const(2)),
                new VariableDeclarationNode("flag", Const(0)),
                Ret
                (
                    new ConditionalNode
                    (
                        new BinaryNode(GreaterThanNode.Operator, Var("a"), Var("b")),
                        Const(5),
                        new ConditionalNode(Var("flag"), Const(6), Const(7))
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int flag = 1;
                int a = 0;
                flag ? a = 1 : (a = 0);
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("flag", Const(1)),
                new VariableDeclarationNode("a", Const(0)),
                new ExpressionNode(new ConditionalNode
                (
                    Var("flag"),
                    new AssignmentNode(Var("a"), Const(1)),
                    new AssignmentNode(Var("a"), Const(0))
                )),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a != 2 ? a = 2 : 0;
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(1)),
                new ExpressionNode(new ConditionalNode
                (
                    new BinaryNode(NotEqualNode.Operator, Var("a"), Const(2)),
                    new AssignmentNode(Var("a"), Const(2)),
                    Const(0)
                )),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1 ? 3 % 2 : 4;
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", new ConditionalNode
                    (
                        Const(1),
                        new BinaryNode(RemainderNode.Operator, Const(3), Const(2)),
                        Const(4)
                    )),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return a || 0 ? 20 : 0;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(10)),
                Ret
                (
                    new ConditionalNode
                    (
                        new BinaryNode(LogicalOrNode.Operator, Var("a"), Const(0)),
                        Const(20),
                        Const(0)
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                return 0 ? 1 : 0 || 2;
            }
            """,
            GetExpected(
                Ret
                (
                    new ConditionalNode
                    (
                        Const(0),
                        Const(1),
                        new BinaryNode(LogicalOrNode.Operator, Const(0), Const(2))
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 0;
                a ? (b = 1) : (b = 2);
                return b;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                new VariableDeclarationNode("b", Const(0)),
                new ExpressionNode
                (
                    new ConditionalNode
                    (
                        Var("a"),
                        new AssignmentNode(Var("b"), Const(1)),
                        new AssignmentNode(Var("b"), Const(2))
                    )
                ),
                Ret(Var("b"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                return a > -1 ? 4 : 5;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(0)),
                Ret
                (
                    new ConditionalNode
                    (
                        new BinaryNode
                            (
                                GreaterThanNode.Operator, 
                                Var("a"), 
                                new UnaryNode(NegateNode.Operator, Const(1))
                            ),
                        Const(4),
                        Const(5)
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                int b = 1;
                a > b ? a = 1 : a = 0;
                return a;
            }
            """,
            GetExpected(
                new VariableDeclarationNode("a", Const(2)),
                new VariableDeclarationNode("b", Const(1)),
                new ExpressionNode
                (
                    new AssignmentNode
                    (
                        new ConditionalNode
                        (
                            new BinaryNode(GreaterThanNode.Operator, Var("a"), Var("b")),
                            new AssignmentNode(Var("a"), Const(1)),
                            Var("a")
                        ),
                        Const(0)
                    )
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                return a > 0 ? 1 : 2;
                int a = 5;
            }
            """,
            GetExpected(                
                Ret
                (
                    new ConditionalNode
                    (
                        new BinaryNode(GreaterThanNode.Operator, Var("a"), Const(0)),
                        Const(1),
                        Const(2)
                    )
                ),
                new VariableDeclarationNode("a", Const(5))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = -1;           
                if (++a)
                    return 0;
                else if (++a)
                    return 1;
                return 0;
            
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", new UnaryNode(NegateNode.Operator, Const(1))),
                new IfNode
                (
                    new UnaryNode(PrefixIncrementNode.Operator, Var("a")),
                    Ret(Const(0)),
                    new IfNode
                         (
                             new UnaryNode(PrefixIncrementNode.Operator, Var("a")),
                             Ret(Const(1))
                         )
                ),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 1;
                goto post_declaration;
                int i = (x = 0);
            post_declaration:
                i = 5;
                return 0;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("x", Const(1)),
                new GotoNode("post_declaration"),
                new VariableDeclarationNode("i", new AssignmentNode(Var("x"), Const(0))),
                new LabelNode
                (
                    "post_declaration",
                    new ExpressionNode(new AssignmentNode(Var("i"), Const(5)))
                ),                
                Ret(Const(0))
            )
        );        
        Add
        (
            """
            int main(void) {
                if (0)
                label:
                    return 5;
                goto label;
                return 0;
            }
            """,
            GetExpected
            (
                new IfNode
                (
                    Const(0),
                    new LabelNode("label", Ret(Const(5))
                )),
                new GotoNode("label"),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {                
                int ident = 5;
                goto ident;
                return 0;
            ident:
                return ident;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("ident", Const(5)),
                new GotoNode("ident"),
                Ret(Const(0)),
                new LabelNode("ident", Ret(Var("ident")))
            )
        );
        Add
        (
            """
            int main(void) {
                goto _main;
                return 0;
                _main:
                    return 1;
            }
            """,
            GetExpected
            (
                new GotoNode("_main"),
                Ret(Const(0)),
                new LabelNode("_main", Ret(Const(1)))
            )
        );
        Add
        (
            """
            int main(void) {
                goto main;
                return 5;
            main:
                return 0;
            }
            """,
            GetExpected
            (
                new GotoNode("main"),
                Ret(Const(5)),
                new LabelNode("main", Ret(Const(0)))
            )
        );
        Add
        (
            """
            int main(void) {
                goto label;
                return 0;
            label:
                return 1;
            }
            """,
            GetExpected
            (
                new GotoNode("label"),
                Ret(Const(0)),
                new LabelNode("label", Ret(Const(1)))
            )
        );
        Add
        (
            """
            int main(void) {
                goto labelB;
            
                labelA:
                    labelB:
                        return 5;
                return 0;
            }
            """,
            GetExpected
            (
                new GotoNode("labelB"),
                new LabelNode("labelA", new LabelNode("labelB", Ret(Const(5)))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
            label_if:
                if (a)
                    goto label_expression;
                else
                    goto label_empty;
            
            label_goto:
                goto label_return;
            
                if (0)
                label_expression:
                    a = 0;
            
                goto label_if;
            
            label_return:
                return a;
            
            label_empty:;
                a = 100;
                goto label_goto;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(1)),
                new LabelNode
                (
                    "label_if", 
                    new IfNode
                    (
                        Var("a"), 
                        new GotoNode("label_expression"),
                        new GotoNode("label_empty")
                    )
                ),
                new LabelNode("label_goto", new GotoNode("label_return")),
                new IfNode
                (
                    Const(0), 
                    new LabelNode
                    (
                        "label_expression",
                        new ExpressionNode(new AssignmentNode(Var("a"), Const(0)))
                    )
                ),
                new GotoNode("label_if"),
                new LabelNode("label_return", Ret(Var("a"))),
                new LabelNode("label_empty", NullNode.Statement),
                new ExpressionNode(new AssignmentNode(Var("a"), Const(100))),
                new GotoNode("label_goto")
            )
        );
        Add
        (
            """
            int main(void) {
                goto _foo_1_;
                return 0;
            _foo_1_:
                return 1;
            }
            """,
            GetExpected
            (
                new GotoNode("_foo_1_"),
                Ret(Const(0)),
                new LabelNode("_foo_1_", Ret(Const(1)))
            )
        );
    }
}