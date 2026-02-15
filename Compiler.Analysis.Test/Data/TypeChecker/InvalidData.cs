namespace Compiler.Analysis.Test.Data.TypeChecker;

public class InvalidData : TheoryData<string, string>
{
    public InvalidData()
    {
        Add
        (
            """
            int main(void) {
                return x;
            }            
            int x = 0;
            """,
            "Undeclared variable: x"
        );
        Add
        (
            """
            int f(int i) {
                extern int i;
                return i;
            }
            
            int main(void) {
                return 0;
            }
            """,
            "error: extern declaration of 'i' follows non-extern declaration"
        );
        Add
        (
            """
            int main(void) {
                {
                    extern int a;
                }
                return a;
            }
            
            int a = 1;
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int i = 10;
            
            int main(void) {
                extern int i;
                int i;
                return i;
            }
            """,
            "error: non-extern declaration of 'i' follows extern declaration"
        );
        Add
        (
            """
            int main(void) {
                static int x = 0;
                extern int x;
                return x;
            }
            """,
            "error: extern declaration of 'x' follows non-extern declaration"
        );
        Add
        (
            """
            int main(void) {
                int x = 3;         
                extern int x;
                return x;
            }
            """,
            "error: extern declaration of 'x' follows non-extern declaration"
        );
        Add
        (
            """
            int main(void) {
                int x = 1;
                static int x;
                return x;
            }
            """,
            "error: redefinition of 'x'"
        );
        Add
        (
            """
            int main(void) {
                static int i = 0;
            
                switch(0) {
                    case i: return 0;
                }
                return 0;
            }
            """,
            "error: expression is not an integer constant expression"
        );
        Add
        (
            """
            extern int foo;
            
            int main(void) {
                return foo();
            }
            """,
            "error: called object type 'int' is not a function"
        );
        Add
        (
            """
            int main(void) {            
                int x = 0;           
                for (static int i = 0; i < 10; i = i + 1) {
                    x = x + 1;
                }            
                return x;
            }
            """,
            "error: declaration of non-local variable in 'for' loop"
        );
        Add
        (
            """
            int main(void) {
                static int foo(void);
                return foo();
            }
            
            static int foo(void) {
                return 0;
            }
            """,
            "error: function declared in block scope cannot have 'static' storage class"
        );
        Add
        (
            """
            int foo(void) {
                return 0;
            }
            
            int main(void) {
                extern int foo;
                return 0;
            }
            """,
            "error: redefinition of 'foo' as different kind of symbol"
        );
        Add
        (
            """
            int foo(void);            
            int foo;
            
            int main(void) {
                return 0;
            }
            """,
            "error: redefinition of 'foo' as different kind of symbol"
        );
        Add
        (
            """
            int foo = 10;
            
            int main(void) {
                int foo(void);
                return 0;
            }
            """,
            "error: redefinition of 'foo' as different kind of symbol"
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                static int b = a * 2;
                return b;
            }
            """,
            "error: initializer element is not a compile-time constant"
        );
        Add
        (
            """
            int a = 10;
            int b = 1 + a;
            
            int main(void) {
                return b;
            }
            """,
            "error: initializer element is not a compile-time constant"
        );
        Add
        (
            """
            int main(void) {
                extern int i = 0;
                return i;
            }
            """,
            "error: declaration of block scope identifier with linkage cannot have an initializer"
        );
        Add
        (
            """
            int main(void) {
            
                int x = 0;           
                for (extern int i = 0; i < 10; i = i + 1) {
                    x = x + 1;
                }
            
                return x;
            }
            """,
            "error: declaration of non-local variable in 'for' loop"
        );
        Add
        (
            """
            static int foo;
            
            int main(void) {
                return foo;
            }
            
            int foo = 3;
            """,
            "error: non-static declaration of 'foo' follows static declaration"
        );
        Add
        (
            """
            int main(void) {
                int x = 3;
                {
                    extern int x;
                }
                return x;
            }
            static int x = 10;
            """,
            "error: static declaration of 'x' follows non-static declaration"
        );
        Add
        (
            """
            int foo = 3;
            
            int main(void) {
                return 0;
            }
            
            int foo = 4;
            """,
            "error: redefinition of 'foo'"
        );
        Add
        (
            """
            int foo(void);
            
            int main(void) {
                return foo();
            }            
            static int foo(void) {
                return 0;
            }
            """,
            "error: static declaration of 'foo' follows non-static declaration"
        );
        Add
        (
            """
            int main(void) {
                int foo(void);
                return foo();
            }           
            static int foo(void) {
                return 0;
            }
            """,
            "error: static declaration of 'foo' follows non-static declaration"
        );
        Add
        (
            """
            int x(void);
            int main(void) {
                int a = 10;
                a = x;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int main(void) {
                int x(void);
                x = 3;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                int x = 0;
                return x();
            }
            """,
            "error: called object type 'int' is not a function"
        );
        Add
        (
            """
            int foo(int a);
            
            int main(void) {
                return 5;
            }
            
            int foo(int a, int b) {
                return 4;
            }
            """,
            "error: conflicting types for 'foo'"
        );
        Add
        (
            """
            int bar(void);
            
            int main(void) {
                int foo(int a);
                return bar() + foo(1);
            }
            
            int bar(void) {
                int foo(int a, int b);
                return foo(1, 2);
            }
            """,
            "error: conflicting types for 'foo'"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                int a = 10 / x;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int foo(void){
                return 3;
            }
            
            int main(void) {
                int foo(void);
                return foo();
            }
            
            int foo(void){
                return 4;
            }
            """,
            "error: redefinition of 'foo'"
        );
        Add
        (
            """
            int foo(void){
                return 3;
            }
            
            int main(void) {
                return foo();
            }
            
            int foo(void){
                return 4;
            }
            """,
            "error: redefinition of 'foo'"
        );
        Add
        (
            """
            int foo(int a, int b) {
                return a + 1;
            }
            
            int main(void) {
                return foo(1);
            }
            """,
            "error: too few arguments to function call, expected 2, have 1"
        );
        Add
        (
            """
            int foo(int a) {
                return a + 1;
            }
            
            int main(void) {
                return foo(1, 2);
            }
            """,
            "error: too many arguments to function call, expected 1, have 2"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                x >> 2;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                x += 3;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                int a = 3;
                a += x;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                x++;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int x(void);
            
            int main(void){
                --x;
                return 0;
            }
            """,
            "function 'x' used as variable"
        );
        Add
        (
            """
            int main(void) {
                int f(void);
                switch (f)
                    return 0;
            }
            """,
            "function 'f' used as variable"
        );
    }
}