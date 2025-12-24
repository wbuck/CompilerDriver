using Compiler.Parser.Nodes;

namespace Compiler.Parser.Test.Data;

public class CompoundOperatorData : DataBase
{
    public CompoundOperatorData()
    {
        Add
        (
            """
            int main(void) {
                a++;
                return 0;
            }
            """,
            GetExpected
            (
                new ExpressionNode(new UnaryNode(PostfixIncrementNode.Operator, Var("a"))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                a--;
                return 0;
            }
            """,
            GetExpected
            (
                new ExpressionNode(new UnaryNode(PostfixDecrementNode.Operator, Var("a"))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                a += 1;
                return 0;
            }
            """,
            GetExpected
            (
                new ExpressionNode(new AdditionAssignmentNode(Var("a"), Const(1))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int b = 10;
                b *= a;
                return 0;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("b", Const(10)),
                new ExpressionNode(new MultiplicationAssignmentNode(Var("b"), Var("a"))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void){
                return a >> 2;
            }
            """,
            GetExpected(Ret(new BitwiseNode(BitwiseRightShiftNode.Operator, Var("a"), Const(2))))
        ); 
        Add
        (
            """
            int main(void) {
                int a = 1;
                ++(a+1);
                return 0;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(1)),
                new ExpressionNode
                (
                    new UnaryNode(PrefixIncrementNode.Operator, new BinaryNode(AdditionNode.Operator, Var("a"), Const(1)))
                ),
                Ret(Const(0))
            )
        ); 
        Add
        (
            """
            int main(void) {
                return --3;
            }
            """,
            GetExpected(Ret(new UnaryNode(PrefixDecrementNode.Operator, Const(3))))
        ); 
        Add
        (
            """
            int main(void) {
                int a = 10;
                return a++--;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(10)),
                Ret(new UnaryNode(PostfixDecrementNode.Operator, new UnaryNode(PostfixIncrementNode.Operator, Var("a"))))
            )
        ); 
        Add
        (
            """
            int main(void) {
                int a = 0;
                -a += 1;
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(0)),
                new ExpressionNode
                (
                    new AdditionAssignmentNode
                    (
                        new UnaryNode(NegateNode.Operator, Var("a")),
                        Const(1)
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
                (a = 4)++;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(0)),
                new ExpressionNode(new UnaryNode(PostfixIncrementNode.Operator, new AssignmentNode(Var("a"), Const(4))))
            )
        );        
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 0;
                a++;
                ++a;
                ++a;
                b--;
                --b;
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(0)),
                new VariableDeclarationNode("b", Const(0)),
                new ExpressionNode(new UnaryNode(PostfixIncrementNode.Operator, Var("a"))),
                new ExpressionNode(new UnaryNode(PrefixIncrementNode.Operator, Var("a"))),
                new ExpressionNode(new UnaryNode(PrefixIncrementNode.Operator, Var("a"))),
                new ExpressionNode(new UnaryNode(PostfixDecrementNode.Operator, Var("b"))),
                new ExpressionNode(new UnaryNode(PrefixDecrementNode.Operator, Var("b"))),
                Ret(Var("a"))                  
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                int b = 3 + a++;
                int c = 4 + ++b;
                return c;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(2)),
                new VariableDeclarationNode("b", new BinaryNode
                (
                    AdditionNode.Operator, 
                    Const(3), 
                    new UnaryNode(PostfixIncrementNode.Operator, Var("a"))
                )),
                new VariableDeclarationNode("c", new BinaryNode
                (
                    AdditionNode.Operator, 
                    Const(4), 
                    new UnaryNode(PrefixIncrementNode.Operator, Var("b"))
                )),
                Ret(Var("c"))                  
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                int c = -++(a);
                int d = !(b)--;
                return 42;
            }
            """,
            GetExpected
            (    
                new VariableDeclarationNode("a", Const(1)),
                new VariableDeclarationNode("b", Const(2)),
                new VariableDeclarationNode
                (
                    "c", 
                    new UnaryNode(NegateNode.Operator, new UnaryNode(PrefixIncrementNode.Operator, Var("a")))
                ),
                new VariableDeclarationNode
                (
                    "d", 
                    new UnaryNode(NotNode.Operator, new UnaryNode(PostfixDecrementNode.Operator, Var("b")))
                ),
                Ret(Const(42))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 15;
                int b = a ^ 5;
                return 1 | b;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(15)),
                new VariableDeclarationNode("b", new BitwiseNode(BitwiseXorNode.Operator, Var("a"), Const(5))),
                Ret(new BitwiseNode(BitwiseOrNode.Operator, Const(1), Var("b")))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                int b = 5;
                int c = 8;
                return a & b | c;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(3)),
                new VariableDeclarationNode("b", Const(5)),
                new VariableDeclarationNode("c", Const(8)),
                Ret
                (
                    new BitwiseNode
                    (
                        BitwiseOrNode.Operator, 
                        new BitwiseNode(BitwiseAndNode.Operator, Var("a"), Var("b")), 
                        Var("c")
                    )
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 3;
                return x << 3;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("x", Const(3)),
                Ret(new BitwiseNode(BitwiseLeftShiftNode.Operator, Var("x"), Const(3)))
            )
        );
        Add
        (
            """
            int main(void) {
                int var_to_shift = 1234;
                int x = 0;
                x = var_to_shift >> 4;
                return x;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("var_to_shift", Const(1234)),
                new VariableDeclarationNode("x", Const(0)),
                Expr
                (
                    new AssignmentNode
                    (
                        Var("x"), 
                        new BitwiseNode(BitwiseRightShiftNode.Operator, Var("var_to_shift"), Const(4))
                    )
                ),
                Ret(Var("x"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 250;
                int b = 200;
                int c = 100;
                int d = 75;
                int e = -25;
                int f = 0;
                int x = 0;
                x = a += b -= c *= d /= e %= f = -7;
                return a == 2250 && c == -1800;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(250)),
                new VariableDeclarationNode("b", Const(200)),
                new VariableDeclarationNode("c", Const(100)),
                new VariableDeclarationNode("d", Const(75)),
                new VariableDeclarationNode("e", new UnaryNode(NegateNode.Operator, Const(25))),
                new VariableDeclarationNode("f", Const(0)),
                new VariableDeclarationNode("x", Const(0)),
                Expr
                (
                    new AssignmentNode
                    (
                        Var("x"),
                        new AdditionAssignmentNode
                            (
                                Var("a"),
                                new SubtractionAssignmentNode
                                    (
                                        Var("b"),
                                        new MultiplicationAssignmentNode
                                            (
                                                Var("c"),
                                                new DivisionAssignmentNode
                                                    (
                                                        Var("d"),
                                                        new RemainderAssignmentNode
                                                            (
                                                                Var("e"),
                                                                new AssignmentNode
                                                                    (
                                                                        Var("f"),
                                                                        new UnaryNode(NegateNode.Operator, Const(7))
                                                                    )
                                                            )
                                                    )
                                            )
                                    )
                            )
                    )
                ),
                Ret
                (
                    new BinaryNode
                    (
                        LogicalAndNode.Operator,
                        new BinaryNode(EqualNode.Operator, Var("a"), Const(2250)),
                        new BinaryNode(EqualNode.Operator, Var("c"), new UnaryNode(NegateNode.Operator, Const(1800)))                        
                    )
                )               
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                int b = 12;
                a += 0 || b;
                b *= a && 0;
            
                int c = 14;
                c -= a || b;
            
                int d = 16;
                d /= c || d;
                return (a == 11 && b == 0);
            }
            """,
            GetExpected
            (     
                new VariableDeclarationNode("a", Const(10)),
                new VariableDeclarationNode("b", Const(12)),
                new ExpressionNode
                (
                    new AdditionAssignmentNode
                    (
                        Var("a"),
                        new BinaryNode(LogicalOrNode.Operator, Const(0), Var("b"))
                    )
                ),
                new ExpressionNode
                (
                    new MultiplicationAssignmentNode
                    (
                        Var("b"),
                        new BinaryNode(LogicalAndNode.Operator, Var("a"), Const(0))
                    )
                ),
                new VariableDeclarationNode("c", Const(14)),
                new ExpressionNode
                (
                    new SubtractionAssignmentNode
                    (
                        Var("c"),
                        new BinaryNode(LogicalOrNode.Operator, Var("a"), Var("b"))
                    )
                ),
                new VariableDeclarationNode("d", Const(16)),
                new ExpressionNode
                (
                    new DivisionAssignmentNode
                    (
                        Var("d"),
                        new BinaryNode(LogicalOrNode.Operator, Var("c"), Var("d"))
                    )
                ),
                Ret(new BinaryNode(LogicalAndNode.Operator, 
                    new BinaryNode(EqualNode.Operator, Var("a"), Const(11)),
                    new BinaryNode(EqualNode.Operator, Var("b"), Const(0))))
            )
        );
        Add
        (
            """
            int main(void) {
                int x = 1;
                int y = x += 3;
                return 0;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("x", Const(1)),
                new VariableDeclarationNode("y", new AdditionAssignmentNode(Var("x"), Const(3))),
                Ret(Const(0))
            )
        );
        Add
        (
            """
            int main(void) {
                int to_and = 3;
                to_and &= 6;
                return to_and;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_and", Const(3)),
                new ExpressionNode(new BitwiseAndAssignmentNode(Var("to_and"), Const(6))),
                Ret(Var("to_and"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 11;
                int b = 12;
                a &= 0 || b;
                b ^= a || 1;
            
                int c = 14;
                c |= a || b;
            
                int d = 16;
                d >>= c || d;
            
                int e = 18;
                e <<= c || d;
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(11)),
                new VariableDeclarationNode("b", Const(12)),
                new ExpressionNode(
                    new BitwiseAndAssignmentNode(
                        Var("a"), 
                        new BinaryNode(LogicalOrNode.Operator, Const(0), Var("b")))),
                new ExpressionNode(
                    new BitwiseXorAssignmentNode(
                        Var("b"), 
                        new BinaryNode(LogicalOrNode.Operator, Var("a"), Const(1)))),
                new VariableDeclarationNode("c", Const(14)),
                new ExpressionNode(
                    new BitwiseOrAssignmentNode(
                        Var("c"), 
                        new BinaryNode(LogicalOrNode.Operator, Var("a"), Var("b")))),
                new VariableDeclarationNode("d", Const(16)),
                new ExpressionNode(
                    new RightShiftAssignmentNode(
                        Var("d"), 
                        new BinaryNode(LogicalOrNode.Operator, Var("c"), Var("d")))),
                new VariableDeclarationNode("e", Const(18)),
                new ExpressionNode(
                    new LeftShiftAssignmentNode(
                        Var("e"), 
                        new BinaryNode(LogicalOrNode.Operator, Var("c"), Var("d")))),
                Ret(Var("a"))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 250;
                int b = 200;
                int c = 100;
                int d = 75;
                int e = 50;
                int f = 25;
                int g = 10;
                int h = 1;
                int j = 0;
                int x = 0;
                x = a &= b *= c |= d = e ^= f += g >>= h <<= j = 1;
                return x;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(250)),
                new VariableDeclarationNode("b", Const(200)),
                new VariableDeclarationNode("c", Const(100)),
                new VariableDeclarationNode("d", Const(75)),
                new VariableDeclarationNode("e", Const(50)),
                new VariableDeclarationNode("f", Const(25)),
                new VariableDeclarationNode("g", Const(10)),
                new VariableDeclarationNode("h", Const(1)),
                new VariableDeclarationNode("j", Const(0)),
                new VariableDeclarationNode("x", Const(0)),
                new ExpressionNode
                (
                    new AssignmentNode
                    (
                        Var("x"),
                        new BitwiseAndAssignmentNode
                            (
                                Var("a"), 
                                new MultiplicationAssignmentNode
                                    (
                                        Var("b"), 
                                        new BitwiseOrAssignmentNode
                                            (
                                                Var("c"), 
                                                new AssignmentNode
                                                    (
                                                        Var("d"),
                                                        new BitwiseXorAssignmentNode
                                                            (
                                                                Var("e"),
                                                                new AdditionAssignmentNode
                                                                    (
                                                                        Var("f"),
                                                                        new RightShiftAssignmentNode
                                                                            (
                                                                                Var("g"),
                                                                                new LeftShiftAssignmentNode
                                                                                    (
                                                                                        Var("h"),
                                                                                        new AssignmentNode(Var("j"), Const(1))
                                                                                    )
                                                                            )
                                                                    )
                                                            )
                                                    )
                                            )
                                    )
                            )
                    )
                ),
                Ret(Var("x"))
            )
        );
        Add
        (
            """
            int main(void) {
                int to_or = 1;
                to_or |= 30;
                return to_or;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_or", Const(1)),
                new ExpressionNode(new BitwiseOrAssignmentNode(Var("to_or"), Const(30))),
                Ret(Var("to_or"))                
            )
        );
        Add
        (
            """
            int main(void) {
                int to_shiftl = 3;
                to_shiftl <<= 4;
                return to_shiftl;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_shiftl", Const(3)),
                new ExpressionNode(new LeftShiftAssignmentNode(Var("to_shiftl"), Const(4))),
                Ret(Var("to_shiftl"))                
            )
        );
        Add
        (
            """
            int main(void) {
                int to_shiftr = 382574;
                to_shiftr >>= 4;
                return to_shiftr;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_shiftr", Const(382574)),
                new ExpressionNode(new RightShiftAssignmentNode(Var("to_shiftr"), Const(4))),
                Ret(Var("to_shiftr"))                
            )
        );
        Add
        (
            """
            int main(void) {
                int to_xor = 7;
                to_xor ^= 5;
                return to_xor;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_xor", Const(7)),
                new ExpressionNode(new BitwiseXorAssignmentNode(Var("to_xor"), Const(5))),
                Ret(Var("to_xor"))                
            )
        );
        Add
        (
            """
            int main(void) {
                int to_divide = 8;
                to_divide /= 4;
                return to_divide;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_divide", Const(8)),
                new ExpressionNode(new DivisionAssignmentNode(Var("to_divide"), Const(4))),
                Ret(Var("to_divide"))               
            )
        );
        Add
        (
            """
            int main(void) {
                int to_subtract = 10;
                to_subtract -= 8;
                return to_subtract;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_subtract", Const(10)),
                new ExpressionNode(new SubtractionAssignmentNode(Var("to_subtract"), Const(8))),
                Ret(Var("to_subtract"))                 
            )
        );
        Add
        (
            """
            int main(void) {
                int to_mod = 5;
                to_mod %= 3;
                return to_mod;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_mod", Const(5)),
                new ExpressionNode(new RemainderAssignmentNode(Var("to_mod"), Const(3))),
                Ret(Var("to_mod"))               
            )
        );
        Add
        (
            """
            int main(void) {
                int to_multiply = 4;
                to_multiply *= 3;
                return to_multiply;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_multiply", Const(4)),
                new ExpressionNode(new MultiplicationAssignmentNode(Var("to_multiply"), Const(3))),
                Ret(Var("to_multiply"))                
            )
        );
        Add
        (
            """
            int main(void) {
                int to_add = 0;
                to_add += 4;
                return to_add;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("to_add", Const(0)),
                new ExpressionNode(new AdditionAssignmentNode(Var("to_add"), Const(4))),
                Ret(Var("to_add"))                  
            )
        );        
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                int c = ++a;
                int d = --b;
                return (a == 2 && b == 1 && c == 2 && d == 1);
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a", Const(1)),
                new VariableDeclarationNode("b", Const(2)),
                new VariableDeclarationNode("c", new UnaryNode(PrefixIncrementNode.Operator, Var("a"))),
                new VariableDeclarationNode("d", new UnaryNode(PrefixDecrementNode.Operator, Var("b"))),
                Ret
                (
                    new BinaryNode
                    (
                       LogicalAndNode.Operator,
                       new BinaryNode
                       (
                           LogicalAndNode.Operator,
                           new BinaryNode
                           (
                               LogicalAndNode.Operator,
                               new BinaryNode
                               (
                                   EqualNode.Operator,
                                   Var("a"),
                                   Const(2)
                               ),
                               new BinaryNode
                               (
                                   EqualNode.Operator,
                                   Var("b"),
                                   Const(1)
                               )
                           ),
                           new BinaryNode
                               (
                                   EqualNode.Operator,
                                   Var("c"),
                                   Const(2)
                               )
                       ),
                       new BinaryNode
                           (
                               EqualNode.Operator,
                               Var("d"),
                               Const(1)
                           )
                    )
                )
            )
        );
    }
}