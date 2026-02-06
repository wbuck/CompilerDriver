namespace Compiler.Analysis.Test.Data.TypeChecker;

public class InvalidData : TheoryData<string, string>
{
    public InvalidData()
    {
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
            "called object type 'int' is not a function"
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
            "too few arguments to function call, expected 2, have 1"
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
            "too many arguments to function call, expected 1, have 2"
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