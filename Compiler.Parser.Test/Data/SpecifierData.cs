using Compiler.Parser.Nodes;

namespace Compiler.Parser.Test.Data;

public class SpecifierData : DataBase
{
    public SpecifierData()
    {
        Add
        (
            """
            int a = 5;
            
            int return_a(void) {
                return a;
            }
            
            int main(void) {
                int a = 3;
                {
                    extern int a;
                    if (a != 5)
                        return 1;
                    a = 4;
                }
                return a + return_a();
            }
            """,
            GetExpected([
                new VariableDeclarationNode("a", Const(5)),
                new FunctionDeclarationNode("return_a", "int", [], new BlockNode([Ret(Var("a"))])),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new VariableDeclarationNode("a", Const(3)),
                    Compound
                    (
                        new VariableDeclarationNode("a", StorageClass: StorageClass.Extern),
                        new IfNode(new BinaryNode(NotEqualNode.Operator, Var("a"), Const(5)), Ret(Const(1))),
                        new ExpressionNode(new AssignmentNode(Var("a"), Const(4)))
                    ),
                    Ret(new BinaryNode(AdditionNode.Operator, Var("a"), new FunctionCallNode("return_a", [])))
                ]))
            ])
        );
    }
}