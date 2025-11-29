using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.SemanticValidator;

public class CompoundStatementData : DataBase
{
    public CompoundStatementData()
    {
        Add
        (
            """
            int main(void) {
                int a = 3;
                {
                    int a = a = 4;
                }
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(3)),
                Compound(new DeclarationNode("a.1", new AssignmentNode(Var("a.1"), Const(4)))),
                Ret(Var("a.0"))
            )
        );       
        Add
        (
            """
            int main(void) {
                int a = 3;
                {
                    int a = a = 4;
                    return a;
                }
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(3)),
                Compound
                (
                    new DeclarationNode("a.1", new AssignmentNode(Var("a.1"), Const(4))),
                    Ret(Var("a.1"))
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a;
                {
                    int b = a = 1;
                }
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0"),
                Compound(new DeclarationNode("b.1", new AssignmentNode(Var("a.0"), Const(1)))),
                Ret(Var("a.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int ten = 10;
                {}
                int twenty = 10 * 2;
                {{}}
                return ten + twenty;
            }
            """,
            GetExpected
            (
                new DeclarationNode("ten.0", Const(10)),
                Compound(),
                new DeclarationNode
                (
                    "twenty.1", 
                    new BinaryNode(MultiplicationNode.Operator, Const(10), Const(2))
                ),
                Compound(Compound()),
                Ret(new BinaryNode(AdditionNode.Operator, Var("ten.0"), Var("twenty.1")))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                int b;
                {
                    a = -4;
                    int a = 7;
                    b = a + 1;
                }
                return b == 8 && a == -4;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(2)),
                new DeclarationNode("b.1"),
                Compound
                (
                   new ExpressionNode
                   (
                       new AssignmentNode
                       (
                           Var("a.0"), 
                           new UnaryNode(NegateNode.Operator, Const(4))
                       )
                   ),
                   new DeclarationNode("a.2", Const(7)),
                   new ExpressionNode
                   (
                       new AssignmentNode
                       (
                           Var("b.1"),
                           new BinaryNode(AdditionNode.Operator, Var("a.2"), Const(1))
                       )
                   )
                ),
                Ret
                (
                    new BinaryNode
                    (
                        LogicalAndNode.Operator, 
                        new BinaryNode
                        (
                            EqualNode.Operator,
                            Var("b.1"),
                            Const(8)
                        ),
                        new BinaryNode
                        (
                            EqualNode.Operator,
                            Var("a.0"),
                            new UnaryNode(NegateNode.Operator, Const(4))
                        )
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                {
                    int a = 1;
                    return a;
                }
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(2)),
                Compound
                (
                    new DeclarationNode("a.1", Const(1)),
                    Ret(Var("a.1"))
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 4;
                {
                    int x;
                }
                return x;
            }
            """,
            GetExpected
            (
                new DeclarationNode("x.0", Const(4)),
                Compound(new DeclarationNode("x.1")),
                Ret(Var("x.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                {
                    int b = 4;
                    a = b;
                }
                {
                    int b = 2;
                    a = a - b;
                }
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(0)),
                Compound
                (
                    new DeclarationNode("b.1", Const(4)),
                    new ExpressionNode(new AssignmentNode(Var("a.0"), Var("b.1")))
                ),
                Compound
                (
                    new DeclarationNode("b.2", Const(2)),
                    new ExpressionNode
                    (
                        new AssignmentNode
                        (
                            Var("a.0"), 
                            new BinaryNode(SubtractionNode.Operator, Var("a.0"), Var("b.2"))
                        )
                    )
                ),
                Ret(Var("a.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                if (a) {
                    int b = 2;
                    return b;
                } else {
                    int c = 3;
                    if (a < c) {
                        return !a;
                    } else {
                        return 5;
                    }
                }
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(0)),
                new IfNode
                (
                    Var("a.0"), 
                    Compound
                        (
                            new DeclarationNode("b.1", Const(2)),
                            Ret(Var("b.1"))
                        ),
                    Compound
                        (
                            new DeclarationNode("c.2", Const(3)),
                            new IfNode
                            (
                                new BinaryNode(LessThanNode.Operator, Var("a.0"), Var("c.2")),
                                Compound(Ret(new UnaryNode(NotNode.Operator, Var("a.0")))),
                                Compound(Ret(Const(5)))
                            )
                        )
                ),
                Ret(Var("a.0"))
            )
        );
        Add
        (
            """
            int main(void)
            {
                int x;
                {
                    x = 3;
                }
                {
                    return x;
                }
            }
            """,
            GetExpected
            (
                new DeclarationNode("x.0"),
                Compound(new ExpressionNode(new AssignmentNode(Var("x.0"), Const(3)))),
                Compound(Ret(Var("x.0")))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 5;
                if (a > 4) {
                    a -= 4;
                    int a = 5;
                    if (a > 4) {
                        a -= 4;
                    }
                }
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(5)),
                new IfNode
                (
                    new BinaryNode(GreaterThanNode.Operator, Var("a.0"), Const(4)),
                    Compound
                    (
                        new ExpressionNode(new SubtractionAssignmentNode(Var("a.0"), Const(4))),
                        new DeclarationNode("a.1", Const(5)),
                        new IfNode
                        (
                            new BinaryNode(GreaterThanNode.Operator, Var("a.1"), Const(4)),
                            Compound
                                 (
                                     new ExpressionNode(new SubtractionAssignmentNode(Var("a.1"), Const(4)))
                                 )
                        )
                    )
                ),
                Ret(Var("a.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                {
                    if (a != 0)
                        return_a:
                            return a;
                    int a = 4;
                    goto return_a;
                }
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(0)),
                Compound
                (
                    new IfNode
                    (
                        new BinaryNode(NotEqualNode.Operator, Var("a.0"), Const(0)),
                        new LabelNode("return_a", Ret(Var("a.0")))
                    ),
                    new DeclarationNode("a.1", Const(4)),
                    new GotoNode("return_a")
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 5;
                goto inner;
                {
                    int x = 0;
                    inner:
                    x = 1;
                    return x;
                }
            }
            """,
            GetExpected
            (
                new DeclarationNode("x.0", Const(5)),
                new GotoNode("inner"),
                Compound
                (
                    new DeclarationNode("x.1", Const(0)),
                    new LabelNode("inner", new ExpressionNode(new AssignmentNode(Var("x.1"), Const(1)))),
                    Ret(Var("x.1"))
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                int b = 0;
                if (a) {
                    int a = 1;
                    b = a;
                    goto end;
                }
                a = 9;
            end:
                return (a == 10 && b == 1);
            }
            """,
            GetExpected
            (
                new DeclarationNode("a.0", Const(10)),
                new DeclarationNode("b.1", Const(0)),
                new IfNode
                (
                    Var("a.0"),
                    Compound
                         (
                             new DeclarationNode("a.2", Const(1)),
                             new ExpressionNode(new AssignmentNode(Var("b.1"), Var("a.2"))),
                             new GotoNode("end")
                         )
                ),
                new ExpressionNode(new AssignmentNode(Var("a.0"), Const(9))),
                new LabelNode
                (
                    "end",
                    Ret
                    (
                        new BinaryNode
                        (
                            LogicalAndNode.Operator,
                            new BinaryNode(EqualNode.Operator, Var("a.0"), Const(10)),
                            new BinaryNode(EqualNode.Operator, Var("b.1"), Const(1))
                        )
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int sum = 0;
                if (1) {
                    int a = 5;
                    goto other_if;
                    sum = 0;
                first_if:                   
                    a = 5;
                    sum = sum + a;
                }
                if (0) {
                other_if:;
                    int a = 6;
                    sum = sum + a;
                    goto first_if;
                    sum = 0;
                }
                return sum;
            }
            """,
            GetExpected
            (
                new DeclarationNode("sum.0", Const(0)),
                new IfNode
                (
                    Const(1),
                    Compound
                    (
                        new DeclarationNode("a.1", Const(5)),
                        new GotoNode("other_if"),
                        new ExpressionNode(new AssignmentNode(Var("sum.0"), Const(0))),
                        new LabelNode
                        (
                            "first_if",
                            new ExpressionNode(new AssignmentNode(Var("a.1"), Const(5)))
                        ),
                        new ExpressionNode
                        (
                            new AssignmentNode
                            (
                                Var("sum.0"),
                                new BinaryNode
                                    (
                                        AdditionNode.Operator,
                                        Var("sum.0"),
                                        Var("a.1")
                                    )
                            )
                        )
                    )
                ),
                new IfNode
                (
                    Const(0),
                    Compound
                    (
                        new LabelNode
                        (
                            "other_if",
                            NullNode.Statement
                        ),
                        new DeclarationNode("a.2", Const(6)),                        
                        new ExpressionNode
                        (
                            new AssignmentNode
                            (
                                Var("sum.0"),
                                new BinaryNode
                                (
                                    AdditionNode.Operator,
                                    Var("sum.0"),
                                    Var("a.2")
                                )
                            )
                        ),
                        new GotoNode("first_if"),
                        new ExpressionNode(new AssignmentNode(Var("sum.0"), Const(0)))
                    )
                ),
                Ret(Var("sum.0"))
            )
        );
    }
}