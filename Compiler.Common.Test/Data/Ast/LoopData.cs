using Compiler.Common.Ast;
using Compiler.Common.Generation;

namespace Compiler.Common.Test.Data.Ast;

public class LoopData : DataBase
{
    public LoopData()
    {
        Add
        (
            """
            int main(void) {
                int a = 0;
            
                while (a < 5)
                    a = a + 2;
            
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(0)),
                new WhileNode
                (
                    new BinaryNode(LessThanNode.Operator, Var("a"), Const(5)),
                    new ExpressionNode
                    (
                        new AssignmentNode
                        (
                            Var("a"), 
                            new BinaryNode(AdditionNode.Operator, Var("a"), Const(2))
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
                int a = 10;
                while ((a = 1))
                    break;
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(10)),
                new WhileNode
                (
                    new AssignmentNode(Var("a"), Const(1)),
                    new BreakNode()
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                do
                    break;
                while ((a = 1));
                return a;
            }            
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(10)),
                new DoWhileNode
                (
                    new BreakNode(),
                    new AssignmentNode(Var("a"), Const(1))
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                do {
                    a = a * 2;
                } while(a < 11);
            
                return a;
            }           
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(1)),
                new DoWhileNode
                (
                    Compound
                         (
                             new ExpressionNode
                             (
                                 new AssignmentNode
                                 (
                                     Var("a"), 
                                     new BinaryNode(MultiplicationNode.Operator, Var("a"), Const(2))
                                 )
                             )
                         ),
                    new BinaryNode(LessThanNode.Operator, Var("a"), Const(11))
                ),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int i = 2147483642;
                do ; while ((i = i - 5) >= 256);
            
                return i;
            }
            """,
            GetExpected
            (
                new DeclarationNode("i", Const(2147483642)),
                new DoWhileNode
                (
                    NullNode.Statement, 
                    new BinaryNode
                    (
                        GreaterThanOrEqualNode.Operator, 
                        new AssignmentNode
                            (
                                Var("i"), 
                                new BinaryNode(SubtractionNode.Operator, Var("i"), Const(5))
                            ),
                        Const(256)
                    )
                ),
                Ret(Var("i"))
            )
        );
        Add
        (
            """
            int main(void) {
                int i = 0;
                while (1) {
                    if (++i > 10)
                        break;
                }
                int j = 10;
                while (1) {
                    if (--j < 0)
                        break;
                }
                int result = j == -1 && i == 11;
                return result;
            }
            """,
            GetExpected
            (
                new DeclarationNode("i", Const(0)),
                new WhileNode
                (
                    Const(1),
                    Compound
                         (
                             new IfNode
                             (
                                 new BinaryNode
                                 (
                                     GreaterThanNode.Operator,
                                     new UnaryNode(PrefixIncrementNode.Operator, Var("i")),
                                     Const(10)
                                 ),
                                 new BreakNode()
                             )                             
                         )
                ),
                new DeclarationNode("j", Const(10)),
                new WhileNode
                (
                    Const(1),
                    Compound
                    (
                        new IfNode
                        (
                            new BinaryNode
                            (
                                LessThanNode.Operator,
                                new UnaryNode(PrefixDecrementNode.Operator, Var("j")),
                                Const(0)
                            ),
                            new BreakNode()
                        )                             
                    )
                ),
                new DeclarationNode
                (
                    "result", 
                    new BinaryNode
                    (
                        LogicalAndNode.Operator, 
                        new BinaryNode
                            (
                                EqualNode.Operator, 
                                Var("j"), 
                                new UnaryNode(NegateNode.Operator, Const(1))
                            ),
                        new BinaryNode
                        (
                            EqualNode.Operator, 
                            Var("i"), 
                            Const(11)
                        )
                    )
                ),
                Ret(Var("result"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 12345;
                int i;
            
                for (i = 5; i >= 0; i = i - 1)
                    a = a / 3;
            
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(12345)),
                new DeclarationNode("i"),
                new ForNode
                (
                    new AssignmentNode(Var("i"), Const(5)),
                    new BinaryNode(GreaterThanOrEqualNode.Operator, Var("i"), Const(0)),
                    new AssignmentNode
                        (
                            Var("i"), 
                            new BinaryNode(SubtractionNode.Operator, Var("i"), Const(1))
                        ),
                    new ExpressionNode
                    (
                        new AssignmentNode
                        (
                            Var("a"), 
                            new BinaryNode(DivisionNode.Operator, Var("a"), Const(3))
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
                int shadow = 1;
                int acc = 0;
                for (int shadow = 0; shadow < 10; shadow++) {
                    acc = acc + shadow;
                }
                return acc;
            }            
            """,
            GetExpected
            (
                new DeclarationNode("shadow", Const(1)),
                new DeclarationNode("acc", Const(0)),
                new ForNode
                (
                    new DeclarationNode("shadow", Const(0)),
                    new BinaryNode(LessThanNode.Operator, Var("shadow"), Const(10)),
                    new UnaryNode(PostfixIncrementNode.Operator, Var("shadow")),
                    Compound
                        (
                            new ExpressionNode
                            (
                                new AssignmentNode
                                (
                                    Var("acc"), 
                                    new BinaryNode(AdditionNode.Operator, Var("acc"), Var("shadow"))
                                )
                            )
                        )
                ),
                Ret(Var("acc"))
            )
        );
        Add
        (
            """
            int main(void) {
                int b = 20;
                for (b = -20; b < 0; b = b + 1) {
                    break;
                }            
                return 0;
            }
            """,
            GetExpected
            (
                new DeclarationNode("b", Const(20)),
                new ForNode
                (
                    new AssignmentNode(Var("b"), new UnaryNode(NegateNode.Operator, Const(20))),
                    new BinaryNode(LessThanNode.Operator, Var("b"), Const(0)),
                    new AssignmentNode
                        (
                            Var("b"), 
                            new BinaryNode(AdditionNode.Operator, Var("b"), Const(1))
                        ),
                    Compound(new BreakNode())
                ),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int b = 20;
                for (b = -20; b < 0; b = b + 1) {
                    continue;
                }            
                return 0;
            }
            """,
            GetExpected
            (
                new DeclarationNode("b", Const(20)),
                new ForNode
                (
                    new AssignmentNode(Var("b"), new UnaryNode(NegateNode.Operator, Const(20))),
                    new BinaryNode(LessThanNode.Operator, Var("b"), Const(0)),
                    new AssignmentNode
                    (
                        Var("b"), 
                        new BinaryNode(AdditionNode.Operator, Var("b"), Const(1))
                    ),
                    Compound(new ContinueNode())
                ),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int sum = 0;
                for (int i = 0; i < 10;) {
                    ++i;
                    if (i % 2)
                        continue;
                    sum = sum + i;
                }
                return sum;
            }
            """,
            GetExpected
            (
                new DeclarationNode("sum", Const(0)),
                new ForNode
                (
                    new DeclarationNode("i", Const(0)),
                    new BinaryNode(LessThanNode.Operator, Var("i"), Const(10)),
                    null,
                    Compound
                        (
                            new ExpressionNode(new UnaryNode(PrefixIncrementNode.Operator, Var("i"))),
                            new IfNode
                            (
                                new BinaryNode(RemainderNode.Operator, Var("i"), Const(2)),
                                new ContinueNode()
                            ),
                            new ExpressionNode
                            (
                                new AssignmentNode
                                (
                                    Var("sum"), 
                                    new BinaryNode(AdditionNode.Operator, Var("sum"), Var("i"))
                                )
                            )
                        )                    
                ),
                Ret(Var("sum"))
            )
        );
        Add
        (
            """
            int main(void) {
                return 0;;;
            }
            """,
            GetExpected
            (
                Ret(Const(0)),
                NullNode.Statement,
                NullNode.Statement
            )
        );
        Add
        (
            """
            int main(void) {
                for (int i = 400; ; i = i - 100)
                    if (i == 100)
                        return 0;
            }
            
            """,
            GetExpected
            (
                new ForNode
                (
                    new DeclarationNode("i", Const(400)),
                    null,
                    new AssignmentNode
                        (
                            Var("i"),
                            new BinaryNode(SubtractionNode.Operator, Var("i"), Const(100))                            
                        ),
                    new IfNode
                    (
                        new BinaryNode(EqualNode.Operator, Var("i"), Const(100)),
                        Ret(Const(0))
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = -2147483647;
                for (; a % 5 != 0;) {
                    a = a + 1;
                }
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", new UnaryNode(NegateNode.Operator, Const(2147483647))),
                new ForNode
                (
                    null,
                    new BinaryNode
                        (
                            NotEqualNode.Operator, 
                            new BinaryNode(RemainderNode.Operator, Var("a"), Const(5)), 
                            Const(0)
                        ),
                    null,
                    Compound
                        (
                            new ExpressionNode
                            (
                                new AssignmentNode
                                (
                                    Var("a"), 
                                    new BinaryNode(AdditionNode.Operator, Var("a"), Const(1))
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
                for (; ; ) {
                    ++a;
                    if (a > 3)
                        break;
                }
            
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(0)),
                new ForNode
                (
                    null,
                    null,
                    null,
                    Compound
                        (
                            new ExpressionNode(new UnaryNode(PrefixIncrementNode.Operator, Var("a"))),
                            new IfNode
                            (
                                new BinaryNode(GreaterThanNode.Operator, Var("a"), Const(3)),
                                new BreakNode()
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
                int i = 100;
                int sum = 0;
                do sum += 2;
                while (i -= 1);
                return i;
            }
            """,
            GetExpected
            (
                new DeclarationNode("i", Const(100)),
                new DeclarationNode("sum", Const(0)),
                new DoWhileNode
                (
                    new ExpressionNode(new AdditionAssignmentNode(Var("sum"), Const(2))),
                    new SubtractionAssignmentNode(Var("i"), Const(1))
                ),
                Ret(Var("i"))
            )
        );
        Add
        (
            """
            int main(void) {
                int i = 1;
                for (i *= -1; i >= -100; i -=3)
                    ;
                return i;
            }
            """,
            GetExpected
            (
                new DeclarationNode("i", Const(1)),
                new ForNode
                (
                    new MultiplicationAssignmentNode
                    (
                        Var("i"), 
                        new UnaryNode(NegateNode.Operator, Const(1))
                    ),
                    new BinaryNode
                        (
                            GreaterThanOrEqualNode.Operator, 
                            Var("i"), 
                            new UnaryNode(NegateNode.Operator, Const(100))
                        ),
                    new SubtractionAssignmentNode(Var("i"), Const(3)),
                    NullNode.Statement
                ),
                Ret(Var("i"))
            )
        );
        Add
        (
            """
            int main(void) {
                int i = 1;
                do {
                while_start:
                    i = i + 1;
                    if (i < 10)
                        goto while_start;
            
                } while (0);
                return i;
            }
            """,
            GetExpected
            (
                new DeclarationNode("i", Const(1)),
                new DoWhileNode
                (
                    Compound
                        (
                            new LabelNode
                            (
                                "while_start",
                                new ExpressionNode
                                (
                                    new AssignmentNode
                                    (
                                        Var("i"), 
                                        new BinaryNode(AdditionNode.Operator, Var("i"), Const(1))
                                    )
                                )
                            ),
                            new IfNode
                            (
                                new BinaryNode(LessThanNode.Operator, Var("i"), Const(10)),
                                new GotoNode("while_start")
                            )
                        ),
                    Const(0)
                ),
                Ret(Var("i"))
            )
        );
        Add
        (
            """
            int main(void) {
                int result = 0;
                goto label;
                while (0)
                label: { result = 1; }
            
                return result;
            }
            """,
            GetExpected
            (
                new DeclarationNode("result", Const(0)),
                new GotoNode("label"),
                new WhileNode
                (
                    Const(0),
                    new LabelNode
                    (
                        "label", 
                        Compound
                            (
                                new ExpressionNode(new AssignmentNode(Var("result"), Const(1)))
                            )
                    )
                ),
                Ret(Var("result"))
            )
        );
        Add
        (
            """
            int main(void) {
                int i = 0;
                goto target;
                for (i = 5; i < 10; ++i)
                target:
                    if (i == 0)
                        return 1;
                return 0;
            }
            """,
            GetExpected
            (
                new DeclarationNode("i", Const(0)),
                new GotoNode("target"),
                new ForNode
                (
                    new AssignmentNode(Var("i"), Const(5)),
                    new BinaryNode(LessThanNode.Operator, Var("i"), Const(10)),
                    new UnaryNode(PrefixIncrementNode.Operator, Var("i")),
                    new LabelNode
                    (
                        "target",
                        new IfNode
                        (
                            new BinaryNode(EqualNode.Operator, Var("i"), Const(0)),
                            Ret(Const(1))
                        )
                    )
                ),
                Ret(Const(0))
            )
        );
    }   
}