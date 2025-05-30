namespace Compiler.Common.Test.Data.SemanticValidator;

public class InvalidSemanticData : TheoryData<string, string>
{
    public InvalidSemanticData()
    {
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
            "Invalid lvalue type found: Unary"
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
            "Invalid lvalue type found: Binary"
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
            "Invalid lvalue type found: Binary"
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