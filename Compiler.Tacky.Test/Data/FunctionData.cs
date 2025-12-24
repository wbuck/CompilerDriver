using Compiler.Tacky.Helpers;
using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

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
                new TackyFunction("main", [], [
                    Label("._label1"),
                    Label(".label_2"),
                    Ret(Const(0)),
                    Ret(Const(0))
                ]),
                new TackyFunction("main_", [], [
                    Label(".label3"),
                    Ret(Const(0)),
                    Ret(Const(0))
                ]),
                new TackyFunction("_main", [], [
                    Label(".label4"),
                    Ret(Const(0)),
                    Ret(Const(0))
                ])
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
                new TackyFunction("foo", [], [
                    Jump(".foo1"),
                    Ret(Const(0)),
                    Label(".foo1"),
                    Ret(Const(1)),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", [], [
                    new TackyFunctionCall("foo", [], Var(1)),
                    Ret(Var(1)),
                    Ret(Const(0))
                ])
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
                new TackyFunction("foo", [], [
                    Jump(".label1"),
                    Ret(Const(0)),
                    Label(".label1"),
                    Ret(Const(5)),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", [], [
                    Jump(".label2"),
                    Ret(Const(0)),
                    Label(".label2"),
                    new TackyFunctionCall("foo", [], Var(1)),
                    Ret(Var(1)),
                    Ret(Const(0))
                ])
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
                new TackyFunction("foo", [], [
                    Ret(Const(2)),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", [], [
                    new TackyCopy(Const(3), Var("x.0")),
                    new TackyFunctionCall("foo", [], Var(1)),
                    new TackyBinary(TackySubtraction.Operator, Var("x.0"), Var(1), Var("x.0")),
                    Ret(Var("x.0")),
                    Ret(Const(0))
                ])
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
                new TackyFunction("three", [], [
                    Ret(Const(3)),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", [], [
                    new TackyFunctionCall("three", [], Var(1)),
                    new TackyUnary(TackyNot.Operator, Var(1), Var(2)),
                    Ret(Var(2)),
                    Ret(Const(0))
                ])
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
                new TackyFunction("main", [], [
                    new TackyFunctionCall("f", [], Var(1)),
                    Ret(Var(1)),
                    Ret(Const(0))
                ]),
                new TackyFunction("f", [], [
                    Ret(Const(3)),
                    Ret(Const(0))
                ])
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
                new TackyFunction("main", [], [
                    new TackyCopy(Const(3), Var("foo.0")),
                    new TackyCopy(Const(4), Var("bar.1")),
                    new TackyBinary(TackyAddition.Operator, Var("foo.0"), Var("bar.1"), Var(1)),
                    new TackyBinary(TackyGreaterThan.Operator, Var(1), Const(0), Var(2)),
                    new TackyJumpIfZero(Var(2), $".{TackyConstants.IF_END_LABEL}1"),
                    new TackyFunctionCall("foo", [], Var(3)),
                    new TackyCopy(Var(3), Var("bar.1")),
                    Label($".{TackyConstants.IF_END_LABEL}1"),
                    new TackyBinary(TackyAddition.Operator, Var("foo.0"), Var("bar.1"), Var(4)),
                    Ret(Var(4)),
                    Ret(Const(0))
                ]),
                new TackyFunction("foo", [], [
                    Ret(Const(8)),
                    Ret(Const(0))
                ])
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
                new TackyFunction("main", [], [
                    new TackyFunctionCall("foo", [], Var(1)),
                    Ret(Var(1)),
                    Ret(Const(0))
                ]),
                new TackyFunction("foo", [], [
                    Ret(Const(3)),
                    Ret(Const(0))
                ])
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
                new TackyFunction("fib", [ "n.0" ], [
                   new TackyBinary(TackyEqual.Operator, Var("n.0"), Const(0), Var(1)),
                   new TackyJumpIfNotZero(Var("tmp.1"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                   new TackyBinary(TackyEqual.Operator, Var("n.0"), Const(1), Var(2)),
                   new TackyJumpIfNotZero(Var("tmp.2"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                   new TackyCopy(Const(0), Var("tmp.3")),
                   Jump($".{TackyConstants.OR_END_LABEL}4"),
                   Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                   new TackyCopy(Const(1), Var("tmp.3")),
                   Label($".{TackyConstants.OR_END_LABEL}4"),
                   new TackyJumpIfZero(Var("tmp.3"), $".{TackyConstants.ELSE_LABEL}2"),
                   Ret(Var("n.0")),
                   Jump($".{TackyConstants.IF_END_LABEL}1"),
                   Label($".{TackyConstants.ELSE_LABEL}2"),
                   new TackyBinary(TackySubtraction.Operator, Var("n.0"), Const(1), Var(4)),
                   new TackyFunctionCall("fib", [Var("tmp.4")], Var("tmp.5")),
                   new TackyBinary(TackySubtraction.Operator, Var("n.0"), Const(2), Var(6)),
                   new TackyFunctionCall("fib", [Var("tmp.6")], Var("tmp.7")),
                   new TackyBinary(TackyAddition.Operator, Var("tmp.5"), Var("tmp.7"), Var("tmp.8")),
                   Ret(Var("tmp.8")),
                   Label($".{TackyConstants.IF_END_LABEL}1"),
                   Ret(Const(0))
                ])
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
                new TackyFunction("add", [ "x.0", "y.1" ], [
                    new TackyBinary(TackyAddition.Operator, Var("x.0"), Var("y.1"), Var(1)),
                    Ret(Var(1)),
                    Ret(Const(0))
                ])
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
                new TackyFunctionCall("add", [ Const(1), Const(2) ], Var(1)),
                Ret(Var(1))
            ])
        );
    }
}