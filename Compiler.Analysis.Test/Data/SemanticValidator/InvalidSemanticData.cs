namespace Compiler.Analysis.Test.Data.SemanticValidator;

public class InvalidSemanticData : TheoryData<string, string>
{
    public InvalidSemanticData()
    {   
        Add
        (
            """
            int main(void) {
                int x = 1;
                a:
                x = x + 1;
                a();
                return x;            
            }
            """,
            "error: use of undeclared identifier 'a'"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                x() += 1;
                return 0;
            }
            """,
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {                
                x()--;
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {                
                ++x();
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int foo(int a);
            
            int main(void) {
                return foo(3);
            }
            
            int foo(int x) {
                return a;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                return foo(3);
            }
            
            int foo(int a) {
                return 1;
            }
            """,
            "error: use of undeclared identifier 'foo'"
        );
        Add
        (
            """
            int main(void) {
                int foo = 1;
                int foo(void);
                return foo;
            }
            
            int foo(void) {
                return 1;
            }
            """,
            "redefinition of 'foo'"
        );
        Add
        (
            """
            int foo(int a) {
                int a = 5;
                return a;
            }
            
            int main(void) {
                return foo(3);
            }
            """,
            "Duplicate variable declaration: a"
        );
        Add
        (
            """
            int main(void) {
                int foo(void);
                int foo = 1;
                return foo;
            }
            
            int foo(void) {
                return 1;
            }
            """,
            "Duplicate variable declaration: foo"
        );
        Add
        (
            """
            int foo(int a, int a) {
                return a;
            }
            
            int main(void) {
                return foo(1, 2);
            }
            """,
            "error: redefinition of parameter 'a'"
        );
        Add
        (
            """
            int main(void) {
                int foo(void) {
                    return 1;
                }
                return foo();
            }
            """,
            "function definition is not allowed here"
        );
        Add
        (
            """
            int foo(int a, int a);
            
            int main(void) {
                return foo(1, 2);
            }
            
            int foo(int a, int b) {
                return a + b;
            }
            """,
            "error: redefinition of parameter 'a'"
        );
        Add
        (
            """
            int x(void);
            
            int main(void) {
                x() = 1;
                return 0;
            }
            """,
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch (1) {
                    case a++: break;
                }
            }
            """,
            "case label does not reduce to an integer constant"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch (1) {
                    case ++a: break;
                }
            }
            """,
            "case label does not reduce to an integer constant"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch (1) {
                    case 12 * (2 + ~-!a): break;
                }
            }
            """,
            "case label does not reduce to an integer constant"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch(a + 1) {
                    case 0: return 0;
                    case a: return 1;
                    case 1: return 2;
                }
            }
            """,
            "case label does not reduce to an integer constant"
        );        
        Add
        (
            """
            int main(void) {
                int a = 10;
                switch (a) {
                    case 1:
                        break;
            
                    default:
                        return b;
                        break;
                }
                return 0;
            }
            """,
            "Undeclared variable: b"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                switch (a) {
                    case 1:
                        return b;
                        break;
            
                    default:
                        break;
                }
                return 0;
            }
            """,
            "Undeclared variable: b"
        );
        Add
        (
            """
            int main(void) {
                switch(a) {
                    case 1: return 0;
                    case 2: return 1;
                }
                return 0;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                switch (a) {
                    int b = 2;
                    case 0:
                        a = 3;
                        int b = 2;
                }
                return 0;
            }
            """,
            "Duplicate variable declaration: b"
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                switch (a) {
                    case 1:;
                        int b = 10;
                        break;
            
                    case 2:;
                        int b = 11;
                        break;
            
                    default:
                        break;
                }
                return 0;
            }
            """,
            "Duplicate variable declaration: b"
        );
        Add
        (
            """
            int main(void)
            {
                for (i = 0; i < 1; i = i + 1)
                {
                    return 0;
                }
            }
            """,
            "Undeclared variable: i"
        );
        Add
        (
            """
            int main(void) {
                do {
                    int a = a + 1;
                } while (a < 100);
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                for (i = 0; i < 1; i = i + 1) {
                    return 0;
                }
            }
            """,
            "Undeclared variable: i"
        );
        Add
        (
            """
            int main(void) {
                do {
                    int a = a + 1;
                } while (a < 100);
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                int a;
                {
                    b = 10;
                }
                int b;
                return b;
            }
            """,
            "Undeclared variable: b"
        );
        Add
        (
            """
            int main(void) {
                {
                    int a = 2;
                }
                return a;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                {
                    int a;
                    int a;
                }
            }            
            """,
            "Duplicate variable declaration: a"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                {
                    a = 5;
                }
                int a = 2;
                return a;
            }
            """,
            "Duplicate variable declaration: a"
        );
        Add
        (
            """
            int main(void) {
                goto(a);
            a:
                return 0;
            }
            """,
            "Expected token Identifier but found ("
        );
        Add
        (
            """
            label:
            int main(void) {
                return 0;
            }
            """,
            "error: type specifier missing"
        );
        Add
        (
            """
            int main(void) {
                1 && label: 2;
            }
            """,
            "Expected ';' but found ':'"
        );
        Add
        (
            """
            int main(void) {
                return: return 0;
            }
            """,
            "Expected expression but found ':'"
        );
        Add
        (
            """
            int main(void) {
                goto;
            lbl:
                return 0;
            }
            """,
            "Expected token Identifier but found ;"
        );
        Add
        (
            """
            int main(void) {
                foo:
            }
            """,
            "A label can only be part of a statement"
        );
        Add
        (
            """
            int main(void) {
            label:
                int a = 0;
                return 0;
            }
            """,
            "A label can only be part of a statement"
        );
        Add
        (
            """
            int main(void) {
                int x = 0;
                a:
                x = a;
                return 0;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
            lbl:
                return a;
                return 0;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                return a > 0 ? 1 : 2;
                int a = 5;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                int b = 1;
                a > b ? a = 1 : a = 0;
                return a;
            }
            """,
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                if (1)
                    return c;
                int c = 0;
            }
            """,
            "Undeclared variable: c"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                (a += 1) -= 2;
            }
            """,
            "Expression must be modifiable lvalue"
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
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return a++--;
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                (a = 4)++;
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int main(void) {
                return --3;
            }
            """,
            "error: expression is not assignable"
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
            "error: expression is not assignable"
        );
        Add
        (
            """
            int main(void){
                return a >> 2;
            }
            """,
            "Undeclared variable: a"
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
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                a += 1;
                return 0;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                a--;
                return 0;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                a++;
                return 0;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return a++--;
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return a--++;
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return --a++;
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return --!a;
            }
            """,
            "error: expression is not assignable"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return ++!a;
            }
            """,
            "error: expression is not assignable"
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
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                (a += 1) -= 2;
            }
            """,
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                a = 1 + 2;
                int a;
                return a;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                !a = 3;
                return a;
            }
            """,
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                a + 3 = 4;
                return a;
            }
            """,
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                a = 3 * b = a;
            }
            """,
            "Expression must be modifiable lvalue"
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int a = 2;
                return a;
            }
            """,
            "Duplicate variable declaration: a"
        );
        Add
        (
            """
            int main(void) {
                return 0 && a;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                return a < 5;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                return -a;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                return a;
            }
            """,
            "Undeclared variable: a"
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                return a;
                int a = 1;
                return a;
            }
            """,
            "Duplicate variable declaration: a"
        );
        Add
        (
            """
            int main(void) {
            label1:;
                int a = 10;
            label2:;
                int a = 11;
                return 1;
            }
            """,
            "Duplicate variable declaration: a"
        );
        Add
        (
            """
            int main(void) {
                int x = 0;
                if (x != 0) {
                    return_y:
                    return y;
                }
                int y = 4;
                goto return_y;
            }
            """,
            "Undeclared variable: y"
        );
    }
}