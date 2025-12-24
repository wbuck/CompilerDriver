namespace Compiler.Emission.Test.Data;

public class BinaryOperatorData : DataBase
{
    public BinaryOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 1 + 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "addl $2, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 6 / 3 / 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $6, %eax",
                "cdq",
                "movl $3, %r10d",
                "idivl %r10d",
                "movl %eax, -4(%rbp)",
                "movl -4(%rbp), %eax",
                "cdq",
                "movl $2, %r10d",
                "idivl %r10d",
                "movl %eax, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (3 / 2 * 4) + (5 - 4 + 3);
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $3, %eax",
                "cdq",
                "movl $2, %r10d",
                "idivl %r10d",
                "movl %eax, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "movl -8(%rbp), %r11d",
                "imull $4, %r11d",
                "movl %r11d, -8(%rbp)",
                "movl $5, -12(%rbp)",
                "subl $4, -12(%rbp)",
                "movl -12(%rbp), %r10d",
                "movl %r10d, -16(%rbp)",
                "addl $3, -16(%rbp)",
                "movl -8(%rbp), %r10d",
                "movl %r10d, -20(%rbp)",
                "movl -16(%rbp), %r10d",
                "addl %r10d, -20(%rbp)",
                "movl -20(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 * 4 / 2 -
                    3 % (2 + 1);
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $5, -4(%rbp)",
                "movl -4(%rbp), %r11d",
                "imull $4, %r11d",
                "movl %r11d, -4(%rbp)",
                "movl -4(%rbp), %eax",
                "cdq",
                "movl $2, %r10d",
                "idivl %r10d",
                "movl %eax, -8(%rbp)",
                "movl $2, -12(%rbp)",
                "addl $1, -12(%rbp)",
                "movl $3, %eax",
                "cdq",
                "idivl -12(%rbp)",
                "movl %edx, -16(%rbp)",
                "movl -8(%rbp), %r10d",
                "movl %r10d, -20(%rbp)",
                "movl -16(%rbp), %r10d",
                "subl %r10d, -20(%rbp)",
                "movl -20(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 - 2 - 3;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "subl $2, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "subl $3, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (-12) / 5;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $12, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %eax",
                "cdq",
                "movl $5, %r10d",
                "idivl %r10d",
                "movl %eax, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 4 / 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $4, %eax",
                "cdq",
                "movl $2, %r10d",
                "idivl %r10d",
                "movl %eax, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 4 % 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $4, %eax",
                "cdq",
                "movl $2, %r10d",
                "idivl %r10d",
                "movl %edx, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 * 3;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, -4(%rbp)",
                "movl -4(%rbp), %r11d",
                "imull $3, %r11d",
                "movl %r11d, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 * (3 + 4);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $3, -4(%rbp)",
                "addl $4, -4(%rbp)",
                "movl $2, -8(%rbp)",
                "movl -8(%rbp), %r11d",
                "imull -4(%rbp), %r11d",
                "movl %r11d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 + 3 * 4;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $3, -4(%rbp)",
                "movl -4(%rbp), %r11d",
                "imull $4, %r11d",
                "movl %r11d, -4(%rbp)",
                "movl $2, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "addl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2- -1;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "negl -4(%rbp)",
                "movl $2, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "subl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 - 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "subl $2, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~2 + 3;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, -4(%rbp)",
                "notl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "addl $3, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~(1 + 1);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "addl $1, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "notl -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
    }
}