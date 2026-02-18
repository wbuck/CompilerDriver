using Compiler.Tacky.Helpers;
using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

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
                new TackyFunction("foo", false, [], [
                    Ret(Const(3)),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", true, [], [
                    new TackyFunctionCall("foo", [], Var(1)),
                    new TackyBinary(TackyAddition.Operator, Var(1), Var("bar"), Var(2)),
                    Ret(Var(2)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("bar", false, 4)
             ])
        );
        Add
        (
            """
            extern int foo;            
            int foo;            
            int foo;

            int main(void) {
                return foo;
            }
            
            int foo;
            """,
            GetExpected([
                new TackyFunction("main", true, [], [
                    Ret(Var("foo")),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("foo", true, 0)
             ])
        );
        Add
        (
            """
            static int foo = 3;

            int main(void) {
                return foo;
            }
            extern int foo;
            """,
            GetExpected([
                new TackyFunction("main", true, [], [
                    Ret(Var("foo")),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("foo", false, 3)
             ])
        );
        Add
        (
            """
            int putchar (int ch);

            int print_alphabet(void) {
                static int count = 0;
                putchar(count + 65);
                count = count + 1;
                if (count < 26) {
                    print_alphabet();
                }
                return count;
            }

            int main(void) {
                return print_alphabet();
            }
            """,
            GetExpected([
                new TackyFunction("print_alphabet", true, [], [
                    new TackyBinary(TackyAddition.Operator, Var("count.1"), Const(65), Var(1)),
                    new TackyFunctionCall("putchar", [Var(1)], Var(2)),
                    new TackyBinary(TackyAddition.Operator, Var("count.1"), Const(1), Var(3)),
                    new TackyCopy(Var(3), Var("count.1")),
                    new TackyBinary(TackyLessThan.Operator, Var("count.1"), Const(26), Var(4)),
                    new TackyJumpIfZero(Var(4), $".{TackyConstants.IF_END_LABEL}1"),
                    new TackyFunctionCall("print_alphabet", [], Var(5)),
                    new TackyLabel($".{TackyConstants.IF_END_LABEL}1"),
                    Ret(Var("count.1")),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", true, [], [
                    new TackyFunctionCall("print_alphabet", [], Var(6)),
                    Ret(Var(6)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("count.1", false, 0)
             ])
        );
        Add
        (
            """
            int foo(void) {
                static int x;
                x = x + 1;
                return x;
            }

            int main(void) {
                return foo();
            }
            """,
            GetExpected([
                new TackyFunction("foo", true, [], [
                    new TackyBinary(TackyAddition.Operator, Var("x.0"), Const(1), Var(1)),
                    new TackyCopy(Var(1), Var("x.0")),
                    Ret(Var("x.0")),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", true, [], [
                    new TackyFunctionCall("foo", [], Var(2)),
                    Ret(Var(2)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("x.0", false, 0)
             ])
        );
        Add
        (
            """
            int test_scopes(void) {
                static int i = 65;                
                {                    
                    i = i + 1;                 
                    static int i = 97;                                       
                    i = i + 1;
                }
                return 0;
            }

            int main(void) {
                return test_scopes();
            }
            """,
            GetExpected([
                new TackyFunction("test_scopes", true, [], [
                    new TackyBinary(TackyAddition.Operator, Var("i.0"), Const(1), Var(1)),
                    new TackyCopy(Var(1), Var("i.0")),
                    new TackyBinary(TackyAddition.Operator, Var("i.1"), Const(1), Var(2)),
                    new TackyCopy(Var(2), Var("i.1")),
                    Ret(Const(0)),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", true, [], [
                    new TackyFunctionCall("test_scopes", [], Var(3)),
                    Ret(Var(3)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("i.1", false, 97),
                new TackyStaticVariable("i.0", false, 65)
             ])
        );
        Add
        (
            """
            int i;

            int update_static_or_global(int update_global, int new_val)
            {
                static int i;
                if (update_global)
                {
                    extern int i;
                    i = new_val;
                }
                else                 
                    i = new_val;
                
                return i;
            }

            int main(void) {
                return update_static_or_global(1, 42);
            }
            """,
            GetExpected([
                new TackyFunction("update_static_or_global", true, ["update_global.0", "new_val.1"], [
                    new TackyJumpIfZero(Var("update_global.0"), $".{TackyConstants.ELSE_LABEL}2"),
                    new TackyCopy(Var("new_val.1"), Var("i")),
                    Jump($".{TackyConstants.IF_END_LABEL}1"),
                    new TackyLabel($".{TackyConstants.ELSE_LABEL}2"),
                    new TackyCopy(Var("new_val.1"), Var("i.2")),
                    new TackyLabel($".{TackyConstants.IF_END_LABEL}1"),
                    Ret(Var("i.2")),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", true, [], [
                    new TackyFunctionCall("update_static_or_global", [Const(1), Const(42)], Var(1)),
                    Ret(Var(1)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("i.2", false, 0),
                new TackyStaticVariable("i", true, 0),
             ])
        );
        Add
        (
            """
            int foo(void) {
                static int a = 3;
                a = a * 2;
                return a;
            }

            int bar(void) {
                static int a = 4;
                a = a + 1;
                return a;
            }

            int main(void) {
                return 42;
            }
            """,
            GetExpected([
                new TackyFunction("foo", true, [], [
                    new TackyBinary(TackyMultiplication.Operator, Var("a.0"), Const(2), Var(1)),
                    new TackyCopy(Var(1), Var("a.0")),
                    Ret(Var("a.0")),
                    Ret(Const(0))
                ]),
                new TackyFunction("bar", true, [], [
                    new TackyBinary(TackyAddition.Operator, Var("a.1"), Const(1), Var(2)),
                    new TackyCopy(Var(2), Var("a.1")),
                    Ret(Var("a.1")),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", true, [], [
                    Ret(Const(42)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("a.1", false, 4),
                new TackyStaticVariable("a.0", false, 3)                
            ])
        );
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
                new TackyFunction("main", true, [], [
                    Ret(Var("foo")),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("foo", false, 4)
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
                new TackyFunction("main", true, [], [
                    new TackyCopy(Const(1), Var("outer.0")),
                    new TackyCopy(Const(0), Var("foo.1")),
                    new TackyJumpIfZero(Var("outer.0"), $".{TackyConstants.IF_END_LABEL}1"),
                    Ret(Var("foo")),
                    new TackyLabel($".{TackyConstants.IF_END_LABEL}1"),
                    Ret(Const(0)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("foo", true, 3)
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
                new TackyFunction("return_a", true, [], [
                    Ret(Var("a")),
                    Ret(Const(0))
                ]),
                new TackyFunction("main", true, [], [
                    new TackyCopy(Const(3), Var("a.0")),
                    new TackyBinary(TackyNotEqual.Operator, Var("a"), Const(5), Var(1)),
                    new TackyJumpIfZero(Var(1), $".{TackyConstants.IF_END_LABEL}1"),
                    Ret(Const(1)),
                    new TackyLabel($".{TackyConstants.IF_END_LABEL}1"),
                    new TackyCopy(Const(4), Var("a")),
                    new TackyFunctionCall("return_a", [], Var(2)),
                    new TackyBinary(TackyAddition.Operator, Var("a.0"), Var(2), Var(3)),
                    Ret(Var(3)),
                    Ret(Const(0))
                ]),
                new TackyStaticVariable("a", true, 5)
            ])
        );
    }
}
