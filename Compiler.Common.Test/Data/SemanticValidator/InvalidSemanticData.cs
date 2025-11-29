namespace Compiler.Common.Test.Data.SemanticValidator;

public class InvalidSemanticData : TheoryData<string, string>
{
    public InvalidSemanticData()
    {                
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
            "Expected return type but found 'label'"
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
            "An lvalue is required as decrement operand"
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                (a = 4)++;
            }
            """,
            "An lvalue is required as increment operand"
        );
        Add
        (
            """
            int main(void) {
                return --3;
            }
            """,
            "An lvalue is required as decrement operand"
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
            "An lvalue is required as increment operand"
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
            "An lvalue is required as decrement operand"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return a--++;
            }
            """,
            "An lvalue is required as increment operand"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return --a++;
            }
            """,
            "An lvalue is required as decrement operand"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return --!a;
            }
            """,
            "An lvalue is required as decrement operand"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return ++!a;
            }
            """,
            "An lvalue is required as increment operand"
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