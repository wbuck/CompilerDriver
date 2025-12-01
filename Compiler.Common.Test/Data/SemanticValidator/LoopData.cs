using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.SemanticValidator;

public class LoopData : DataBase
{
    public LoopData()
    {
        Add
        (
            """
            int main(void) {
                int i = 0;
                goto target;
                for (i = 5; i < 10; i = i + 1)
                target:
                    if (i == 0)
                        return 1;
                return 0;
            }
            """,
            GetExpected
            (
                new DeclarationNode("i.0", Const(0)),
                new GotoNode("target"),
                new ForNode
                (
                    new AssignmentNode(Var("i.0"), Const(5)),
                    new BinaryNode(LessThanNode.Operator, Var("i.0"), Const(10)),
                    new AssignmentNode
                        (
                            Var("i.0"),
                            new BinaryNode(AdditionNode.Operator, Var("i.0"), Const(1))
                        ),
                    new LabelNode
                    (
                        "target",
                        new IfNode
                        (
                            new BinaryNode(EqualNode.Operator, Var("i.0"), Const(0)),
                            Ret(Const(1))
                        )
                    )
                ),
                Ret(Const(0))
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
                new DeclarationNode("result.0", Const(0)),
                new GotoNode("label"),
                new WhileNode
                (
                    Const(0),
                    new LabelNode
                    (
                        "label", 
                        Compound
                        (
                            new ExpressionNode(new AssignmentNode(Var("result.0"), Const(1)))
                        )
                    )
                ),
                Ret(Var("result.0"))
            )
        );
        Add
        (
            """
            int main(void) {
                int i = 0;
                int j = 0;
                int k = 1;
                for (int i = 100; i > 0; i--) {
                    int i = 1;
                    int j = i + k;
                    k = j;
                }
            
                return 42;
            }            
            """,
            GetExpected
            (
                new DeclarationNode("i.0", Const(0)),
                new DeclarationNode("j.1", Const(0)),
                new DeclarationNode("k.2", Const(1)),
                new ForNode
                (
                    new DeclarationNode("i.3", Const(100)),
                    new BinaryNode(GreaterThanNode.Operator, Var("i.3"), Const(0)),
                    new UnaryNode(PostfixDecrementNode.Operator, Var("i.3")),
                    Compound
                        (
                            new DeclarationNode("i.4", Const(1)),
                            new DeclarationNode
                            (
                                "j.5", 
                                new BinaryNode(AdditionNode.Operator, Var("i.4"), Var("k.2"))
                            ),
                            new ExpressionNode(new AssignmentNode(Var("k.2"), Var("j.5")))
                        )
                ),
                Ret(Const(42))
            )
        );
    }
}