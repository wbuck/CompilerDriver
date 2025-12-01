
using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.LabelAnnotation;


public class LoopData : DataBase
{
    public LoopData()
    {
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
                new DeclarationNode("a.0", Const(10)),
                new WhileNode
                (
                    new AssignmentNode(Var("a.0"), Const(1)),
                    Break(".while1"),
                    ".while1"
                ),
                Ret(Var("a.0"))
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
                new DeclarationNode("a.0", Const(10)),
                new DoWhileNode
                (
                    Break(".do_while1"),
                    new AssignmentNode(Var("a.0"), Const(1)),
                    ".do_while1"
                ),
                Ret(Var("a.0"))
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
                new DeclarationNode("i.0", Const(0)),
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
                                     new UnaryNode(PrefixIncrementNode.Operator, Var("i.0")),
                                     Const(10)
                                 ),
                                 Break(".while1")
                             )
                         ),
                    ".while1"
                ),
                new DeclarationNode("j.1", Const(10)),
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
                                new UnaryNode(PrefixDecrementNode.Operator, Var("j.1")),
                                Const(0)
                            ),
                            Break(".while2")
                        )
                    ),
                    ".while2"
                ),
                new DeclarationNode
                (
                    "result.2",
                    new BinaryNode
                    (
                        LogicalAndNode.Operator,
                        new BinaryNode
                            (
                                EqualNode.Operator,
                                Var("j.1"),
                                new UnaryNode(NegateNode.Operator, Const(1))
                            ),
                        new BinaryNode
                        (
                            EqualNode.Operator,
                            Var("i.0"),
                            Const(11)
                        )
                    )
                ),
                Ret(Var("result.2"))
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
                new DeclarationNode("b.0", Const(20)),
                new ForNode
                (
                    new AssignmentNode(Var("b.0"), new UnaryNode(NegateNode.Operator, Const(20))),
                    new BinaryNode(LessThanNode.Operator, Var("b.0"), Const(0)),
                    new AssignmentNode
                        (
                            Var("b.0"),
                            new BinaryNode(AdditionNode.Operator, Var("b.0"), Const(1))
                        ),
                    Compound(Break(".for1")),
                    ".for1"
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
                new DeclarationNode("sum.0", Const(0)),
                new ForNode
                (
                    new DeclarationNode("i.1", Const(0)),
                    new BinaryNode(LessThanNode.Operator, Var("i.1"), Const(10)),
                    null,
                    Compound
                        (
                            new ExpressionNode(new UnaryNode(PrefixIncrementNode.Operator, Var("i.1"))),
                            new IfNode
                            (
                                new BinaryNode(RemainderNode.Operator, Var("i.1"), Const(2)),
                                Continue(".for1")
                            ),
                            new ExpressionNode
                            (
                                new AssignmentNode
                                (
                                    Var("sum.0"),
                                    new BinaryNode(AdditionNode.Operator, Var("sum.0"), Var("i.1"))
                                )
                            )
                        ),
                    ".for1"
                ),
                Ret(Var("sum.0"))
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
                new DeclarationNode("a.0", Const(0)),
                new ForNode
                (
                    null,
                    null,
                    null,
                    Compound
                        (
                            new ExpressionNode(new UnaryNode(PrefixIncrementNode.Operator, Var("a.0"))),
                            new IfNode
                            (
                                new BinaryNode(GreaterThanNode.Operator, Var("a.0"), Const(3)),
                                Break(".for1")
                            )
                        ),
                    ".for1"
                ),
                Ret(Var("a.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 100;
                while (1) {
                    while (1) {
                        if (--x <= 0) {
                            break;
                        }
                    }
                    break;
                }
                return x;            
            }
            """,
            GetExpected
            (
                new DeclarationNode("x.0", Const(100)),
                new WhileNode
                (
                    Const(1),
                    Compound
                        (
                            new WhileNode
                            (
                                Const(1),
                                Compound
                                    (
                                        new IfNode
                                        (
                                            new BinaryNode
                                            (
                                                LessThanOrEqualNode.Operator, 
                                                new UnaryNode(PrefixDecrementNode.Operator, Var("x.0")),
                                                Const(0)
                                            ),
                                            Compound(Break(".while2"))
                                        )
                                    ),
                                ".while2"
                            ),
                            Break(".while1")
                        ),
                    ".while1"
                ),
                Ret(Var("x.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 100;
                while (1) {
                    while (1) {
                        if (--x <= 0) {
                            continue;
                        }
                    }
                    continue;
                }
                return x;            
            }
            """,
            GetExpected
            (
                new DeclarationNode("x.0", Const(100)),
                new WhileNode
                (
                    Const(1),
                    Compound
                    (
                        new WhileNode
                        (
                            Const(1),
                            Compound
                            (
                                new IfNode
                                (
                                    new BinaryNode
                                    (
                                        LessThanOrEqualNode.Operator, 
                                        new UnaryNode(PrefixDecrementNode.Operator, Var("x.0")),
                                        Const(0)
                                    ),
                                    Compound(Continue(".while2"))
                                )
                            ),
                            ".while2"
                        ),
                        Continue(".while1")
                    ),
                    ".while1"
                ),
                Ret(Var("x.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int ans = 0;
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        if ((i / 2) * 2 == i)
                            break;
                        else
                            ans = ans + i;
                return ans;
            }            
            """,
            GetExpected
            (
                new DeclarationNode("ans.0", Const(0)),
                new ForNode
                (
                    new DeclarationNode("i.1", Const(0)),
                    new BinaryNode(LessThanNode.Operator, Var("i.1"), Const(10)),
                    new UnaryNode(PostfixIncrementNode.Operator, Var("i.1")),
                    new ForNode
                    (
                        new DeclarationNode("j.2", Const(0)),
                        new BinaryNode(LessThanNode.Operator, Var("j.2"), Const(10)),
                        new UnaryNode(PostfixIncrementNode.Operator, Var("j.2")),
                        new IfNode
                        (
                            new BinaryNode
                            (
                                EqualNode.Operator,
                                new BinaryNode
                                (
                                    MultiplicationNode.Operator,
                                    new BinaryNode
                                    (
                                        DivisionNode.Operator,
                                        Var("i.1"),
                                        Const(2)
                                    ),
                                    Const(2)
                                ),
                                Var("i.1")
                            ),
                            Break(".for2"),
                            new ExpressionNode
                                (
                                    new AssignmentNode
                                    (
                                        Var("ans.0"), 
                                        new BinaryNode(AdditionNode.Operator, Var("ans.0"), Var("i.1"))
                                    )
                                )
                        ),
                        ".for2"
                    ),
                    ".for1"
                ),
                Ret(Var("ans.0"))
            )
        );
    }   
}