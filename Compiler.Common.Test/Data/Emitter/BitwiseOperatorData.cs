namespace Compiler.Common.Test.Data.Emitter;

public class BitwiseOperatorData : DataBase
{
    public BitwiseOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 3 & 5;
            }
            """,
            GetExpected([
                "subq $4, %rsp",
                "movl $3, -4(%rbp)",
                "andl $5, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 | 2;
            }
            """,
            GetExpected([
                "subq $4, %rsp",
                "movl $1, -4(%rbp)",
                "orl $2, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 80 >> 2 | 1 ^ 5 & 7 << 1;
            }
            """,
            GetExpected([
                "subq $20, %rsp",
                "movl $80, -4(%rbp)",
                "sarl $2, -4(%rbp)",
                "movl $7, -8(%rbp)",
                "sall $1, -8(%rbp)",
                "movl $5, -12(%rbp)",
                "movl -8(%rbp), %r10d",
                "andl %r10d, -12(%rbp)",
                "movl $1, -16(%rbp)",
                "movl -12(%rbp), %r10d",
                "xorl %r10d, -16(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -20(%rbp)",
                "movl -16(%rbp), %r10d",
                "orl %r10d, -20(%rbp)",
                "movl -20(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 >> 2 << 1;
            }
            """,
            GetExpected([
                "subq $8, %rsp",
                "movl $33, -4(%rbp)",
                "sarl $2, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "sall $1, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 << 4 >> 2;
            }
            """,
            GetExpected([
                "subq $8, %rsp",
                "movl $33, -4(%rbp)",
                "sall $4, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "sarl $2, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 40 << 4 + 12 >> 1;
            }
            """,
            GetExpected([
                "subq $12, %rsp",
                "movl $4, -4(%rbp)",
                "addl $12, -4(%rbp)",
                "movl $40, -8(%rbp)",
                "movl -4(%rbp), %ecx",
                "sall %cl, -8(%rbp)",
                "movl -8(%rbp), %r10d",
                "movl %r10d, -12(%rbp)",
                "sarl $1, -12(%rbp)",
                "movl -12(%rbp), %eax"
            ])
        );   
        Add
        (
            """
            int main(void) {
                return 35 << 2;
            }
            """,
            GetExpected([
                "subq $4, %rsp",
                "movl $35, -4(%rbp)",
                "sall $2, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return -5 >> 30;
            }
            """,
            GetExpected([
                "subq $8, %rsp",
                "movl $5, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "sarl $30, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])            
        );
        Add
        (
            """
            int main(void) {
                return 1000 >> 4;
            }
            """,
            GetExpected([
                "subq $4, %rsp",
                "movl $1000, -4(%rbp)",
                "sarl $4, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (4 << (2 * 2)) + (100 >> (1 + 2));
            }
            """,
            GetExpected([
                "subq $20, %rsp",
                "movl $2, -4(%rbp)",
                "movl -4(%rbp), %r11d",
                "imull $2, %r11d",
                "movl %r11d, -4(%rbp)",
                "movl $4, -8(%rbp)",
                "movl -4(%rbp), %ecx",
                "sall %cl, -8(%rbp)",
                "movl $1, -12(%rbp)",
                "addl $2, -12(%rbp)",
                "movl $100, -16(%rbp)",
                "movl -12(%rbp), %ecx",
                "sarl %cl, -16(%rbp)",
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
                return 7 ^ 1;
            }
            """,
            GetExpected([
                "subq $4, %rsp",
                "movl $7, -4(%rbp)",
                "xorl $1, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
    }
}