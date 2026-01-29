using Compiler.Parser.Nodes;

namespace Compiler.Parser.Test.Data;

public class SpecifierData : DataBase
{
    public SpecifierData()
    {
        Add
        (
            """
            static int foo;
            
            int main(void) {
                return foo;
            }            
            extern int foo;            
            static int foo = 4;
            """,
            GetExpected([
                new VariableDeclarationNode("foo", StorageClass: StorageClass.Static),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([Ret(Var("foo"))])),
                new VariableDeclarationNode("foo", StorageClass: StorageClass.Extern),
                new VariableDeclarationNode("foo", Const(4), StorageClass.Static)
            ])
        );
        Add
        (
            """
            int main(void) {
                int outer = 1;
                int foo = 0;
                if (outer) {
                    extern int foo;
                    extern int foo;
                    return foo;
                }
                return 0;
            }
            
            int foo = 3;
            """,
            GetExpected([
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new VariableDeclarationNode("outer", Const(1)),
                    new VariableDeclarationNode("foo", Const(0)),
                    new IfNode(Var("outer"), Compound
                    (
                        new VariableDeclarationNode("foo", StorageClass: StorageClass.Extern),
                        new VariableDeclarationNode("foo", StorageClass: StorageClass.Extern),
                        Ret(Var("foo"))
                    )),
                    Ret(Const(0))
                ])),
                new VariableDeclarationNode("foo", Const(3))
            ])
        );
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