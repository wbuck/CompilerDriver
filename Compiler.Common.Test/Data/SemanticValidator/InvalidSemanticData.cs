namespace Compiler.Common.Test.Data.SemanticValidator;

public class InvalidSemanticData : TheoryData<string, string>
{
    public InvalidSemanticData()
    {
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
    }
}