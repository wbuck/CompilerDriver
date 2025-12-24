using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Test.Data.TypeChecker;

public class FunctionData : DataBase
{
    public FunctionData()
    {
        Add
        (
            """
            int main(void) {
                _label:           
                label_:
                return 0;
            }
            
            int main_(void) {
                label:
                return 0;
            }
            
            int _main(void) {
                label: return 0;
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new LabelNode("_label", new LabelNode("label_", Ret(Const(0))))
                ])),
                new FunctionDeclarationNode("main_", "int", [], new BlockNode([
                    new LabelNode("label", Ret(Const(0)))
                ])),
                new FunctionDeclarationNode("_main", "int", [], new BlockNode([
                    new LabelNode("label", Ret(Const(0)))
                ]))
            ])
        );
        Add
        (
            """
            int foo(void) {
                goto foo;
                return 0;
                foo:
                    return 1;
            }
            
            int main(void) {
                return foo();
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("foo", "int", [], new BlockNode([
                    new GotoNode("foo"),
                    Ret(Const(0)),
                    new LabelNode("foo", Ret(Const(1)))
                ])),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    Ret(new FunctionCallNode("foo", []))
                ]))
            ])
        );
        Add
        (
            """
            int foo(void) {
                goto label;
                return 0;
                label:
                    return 5;
            }
            
            int main(void) {
                goto label;
                return 0;
                label:
                    return foo();
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("foo", "int", [], new BlockNode([
                    new GotoNode("label"),
                    Ret(Const(0)),
                    new LabelNode("label", Ret(Const(5)))
                ])),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new GotoNode("label"),
                    Ret(Const(0)),
                    new LabelNode("label", Ret(new FunctionCallNode("foo", [])))
                ]))
            ])
        );        
        Add
        (
            """
            int foo(void) {
                return 2;
            }
            
            int main(void) {
                int x = 3;
                x -= foo();
                return x;
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("foo", "int", [], new BlockNode([
                    Ret(Const(2))
                ])),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new VariableDeclarationNode("x.0", Const(3)),                    
                    new ExpressionNode(new SubtractionAssignmentNode(Var("x.0"), new FunctionCallNode("foo", []))),
                    Ret(Var("x.0"))
                ]))
            ])
        );        
        Add
        (
            """
            int three(void) {
                return 3;
            }
            
            int main(void) {
                return !three();
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("three", "int", [], new BlockNode([Ret(Const(3))])),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new ReturnNode(new UnaryNode(NotNode.Operator, new FunctionCallNode("three", [])))
                ]))
            ])
        );
        Add
        (
            """
            int main(void) {
                int f(void);
                int f(void);
                return f();
            }
            
            int f(void) {
                return 3;
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new FunctionDeclarationNode("f", "int", [], null),
                    new FunctionDeclarationNode("f", "int", [], null),
                    Ret(new FunctionCallNode("f", []))
                ])),
                new FunctionDeclarationNode("f", "int", [], new BlockNode([Ret(Const(3))]))
            ])
        );        
        Add
        (
            """
            int main(void) {
                int foo = 3;
                int bar = 4;
                if (foo + bar > 0) {
                    int foo(void);
                    bar = foo();
                }
                return foo + bar;
            }
            
            int foo(void) {
                return 8;
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new VariableDeclarationNode("foo.0", Const(3)),
                    new VariableDeclarationNode("bar.1", Const(4)),
                    new IfNode
                    (
                        new BinaryNode
                        (
                            GreaterThanNode.Operator, 
                            new BinaryNode(AdditionNode.Operator, Var("foo.0"), Var("bar.1")), 
                            Const(0)
                        ),
                        Compound
                             (
                                 new FunctionDeclarationNode("foo", "int", [], null),
                                 new ExpressionNode(new AssignmentNode(Var("bar.1"), new FunctionCallNode("foo", [])))
                             )
                    ),
                    Ret(new BinaryNode(AdditionNode.Operator, Var("foo.0"), Var("bar.1")))
                ])),
                new FunctionDeclarationNode("foo", "int", [], new BlockNode([Ret(Const(8))]))
            ])
        );        
        Add
        (
            """
            int foo(void);
            
            int main(void) {
                return foo();
            }
            
            int foo(void) {
                return 3;
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("foo", "int", [], null),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new ReturnNode(new FunctionCallNode("foo", []))
                ])),
                new FunctionDeclarationNode("foo", "int", [], new BlockNode([Ret(Const(3))]))
            ])
        );        
        Add
        (
            """
            int fib(int n) {
                if (n == 0 || n == 1) {
                    return n;
                } else {
                    return fib(n - 1) + fib(n - 2);
                }
            }           
            """,
            GetExpected([
                new FunctionDeclarationNode("fib", "int", ["n.0"], new BlockNode([
                    new IfNode
                    (
                        new BinaryNode
                        (
                            LogicalOrNode.Operator,
                            new BinaryNode(EqualNode.Operator, Var("n.0"), Const(0)),
                            new BinaryNode(EqualNode.Operator, Var("n.0"), Const(1))
                        ),
                        Compound(Ret(Var("n.0"))),
                        Compound
                            (
                                Ret
                                (
                                    new BinaryNode
                                    (
                                        AdditionNode.Operator,
                                        new FunctionCallNode("fib", [new BinaryNode(SubtractionNode.Operator, Var("n.0"), Const(1))]),
                                        new FunctionCallNode("fib", [new BinaryNode(SubtractionNode.Operator, Var("n.0"), Const(2))])
                                    )
                                )
                            )
                    )
                ]))                
            ])
        );        
        Add
        (
            """
            int add(int x, int y) {
                return x + y;
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("add", "int", ["x.0", "y.1"], new BlockNode([
                    new ReturnNode(new BinaryNode(AdditionNode.Operator, Var("x.0"), Var("y.1")))
                ]))
            ])
        );        
        Add
        (
            """
            int add(int x, int y);
            
            int main(void) {
                return add(1, 2);
            }
            """,
            GetExpected([
                new FunctionDeclarationNode("add", "int", ["x.0", "y.1"], null),
                new FunctionDeclarationNode("main", "int", [], new BlockNode([
                    new ReturnNode
                    (
                        new FunctionCallNode("add", [Const(1), Const(2)])
                    )
                ]))
            ])
        );
    }
}