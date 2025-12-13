using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.Ast;

public class SwitchData : DataBase
{
    public SwitchData()
    {
        Add
        (
            """
            int main(void) {
                switch(3) {
                    case 0: return 0;
                    case 1: return 1;
                    default: return 2;
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(3),
                    Compound
                         (
                             new CaseNode(Const(0), Ret(Const(0))),                             
                             new CaseNode(Const(1), Ret(Const(1))),                             
                             new DefaultNode(Ret(Const(2)))
                         )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                switch(a) case 1: return 1;
                return 0;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(1)),
                new SwitchNode
                (
                    Var("a"),
                    new CaseNode(Const(1), Ret(Const(1)))
                ),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 4;
                switch(a)
                    return 0;
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(4)),
                new SwitchNode(Var("a"), Ret(Const(0))),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                switch(a) default: return 1;
                return 0;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(1)),
                new SwitchNode(Var("a"), new DefaultNode(Ret(Const(1)))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                switch(a) {
                    case 1:
                        return 1;
                    case 2:
                        return 9;
                    case 4:
                        a = 11;
                        break;
                    default:
                        a = 22;
                }
                return a;
            }
            """,
            GetExpected
            (
                new DeclarationNode("a", Const(0)),
                new SwitchNode
                (
                    Var("a"),
                    Compound
                         (
                             new CaseNode(Const(1), Ret(Const(1))),
                             new CaseNode(Const(2), Ret(Const(9))),
                             new CaseNode
                             (
                                 Const(4), 
                                 new ExpressionNode(new AssignmentNode(Var("a"), Const(11)))
                             ),
                             new BreakNode(),
                             new DefaultNode
                             (
                                 new ExpressionNode(new AssignmentNode(Var("a"), Const(22)))
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
                int x = 10;
                switch(x = x + 1) {
            
                }
                switch(x = x + 1)
                ;
                return x;
            }
            """,
            GetExpected
            (
                new DeclarationNode("x", Const(10)),
                new SwitchNode
                (
                    new AssignmentNode
                    (
                        Var("x"), 
                        new BinaryNode(AdditionNode.Operator, Var("x"), Const(1))
                    ), 
                    Compound()
                ),
                new SwitchNode
                (
                    new AssignmentNode
                    (
                        Var("x"), 
                        new BinaryNode(AdditionNode.Operator, Var("x"), Const(1))
                    ), 
                    NullNode.Statement
                ),
                Ret(Var("x"))
            )
        );
    }
}