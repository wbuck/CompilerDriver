namespace Compiler.Common.Test.Data.EmitterData;

public class ValidUnaryData : DataBase
{
    public ValidUnaryData()
    {
        Add
        (
            """
            int main(void) {
                return ~-2147483647;
            }
            """,
            Emit([
                "subq $8, %rsp",
                "movl $2147483647, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "notl -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~0;
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $0, -4(%rbp)",
                "notl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~12;
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $12, -4(%rbp)",
                "notl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return -0;
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $0, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return -5;
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $5, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return -2147483647;
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $2147483647, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return -~0;
            }
            """,
            Emit([
                "subq $8, %rsp",
                "movl $0, -4(%rbp)",
                "notl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "negl -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~-3;
            }
            """,
            Emit([
                "subq $8, %rsp",
                "movl $3, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "notl -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~(2);
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $2, -4(%rbp)",
                "notl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return -(-4);
            }
            """,
            Emit([
                "subq $8, %rsp",
                "movl $4, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "negl -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (-2);
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $2, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void)
            {
                return -((((10))));
            }
            """,
            Emit([
                "subq $4, %rsp",
                "movl $10, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
    }
}