using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Test.Data.SemanticValidator;

public class SpecifierData : DataBase
{
    public SpecifierData()
    {
        Add
        (
            """
            int static foo(void) {
                return 3;
            }
            
            int static bar = 4;
            
            int main(void) {
                int extern foo(void);
                int extern bar;
                return foo() + bar;
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("foo", "int", [], new BlockNode([
                    Ret(Const(3))
                ]), StorageClass.Static),
                new VariableDeclarationNode("bar", Const(4), StorageClass.Static),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new FunctionDeclarationNode("foo", "int", [], null, StorageClass.Extern),
                    new VariableDeclarationNode("bar", null, StorageClass.Extern),
                    Ret(new BinaryNode(AdditionNode.Operator, new FunctionCallNode("foo", []), Var("bar")))
                ]))
            ])
        );
    }
}