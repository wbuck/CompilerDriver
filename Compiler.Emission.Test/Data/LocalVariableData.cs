namespace Compiler.Emission.Test.Data;

public class LocalVariableData : DataBase
{
    public LocalVariableData()
    {
        Add
        (
            """
            int main(void) {
                int first_variable = 1;
                int second_variable = 2;
                return first_variable + second_variable;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "movl $2, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -12(%rbp)",
                "movl -8(%rbp), %r10d",
                "addl %r10d, -12(%rbp)",
                "movl -12(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 2147483646;
                int b = 0;
                int c = a / 6 + !b;
                return c * 2 == a - 1431655762;
            }
            """,
            GetExpected([
                "subq $48, %rsp",
                "movl $2147483646, -4(%rbp)",
                "movl $0, -8(%rbp)",
                "movl -4(%rbp), %eax",
                "cdq",
                "movl $6, %r10d",
                "idivl %r10d",
                "movl %eax, -12(%rbp)",
                "cmpl $0, -8(%rbp)",
                "movl $0, -16(%rbp)",
                "sete -16(%rbp)",
                "movl -12(%rbp), %r10d",
                "movl %r10d, -20(%rbp)",
                "movl -16(%rbp), %r10d",
                "addl %r10d, -20(%rbp)",
                "movl -20(%rbp), %r10d",
                "movl %r10d, -24(%rbp)",
                "movl -24(%rbp), %r10d",
                "movl %r10d, -28(%rbp)",
                "movl -28(%rbp), %r11d",
                "imull $2, %r11d",
                "movl %r11d, -28(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -32(%rbp)",
                "subl $1431655762, -32(%rbp)",
                "movl -32(%rbp), %r10d",
                "cmpl %r10d, -28(%rbp)",
                "movl $0, -36(%rbp)",
                "sete -36(%rbp)",
                "movl -36(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = a = 5;
                return a;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $5, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int var0;
                var0 = 2;
                return var0;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a;
                int b = a = 0;
                return b;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a;
                a = 0 || 5;
                return a;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $5, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -4(%rbp)",
                "jmp .OR_END_L2",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -4(%rbp)",
                ".OR_END_L2:",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
            
            }
            """,
            GetExpected([
                "subq $0, %rsp"
            ], missingReturn: true)
        );
        Add
        (
            """
            int main(void) {
                int a = -2593;
                a = a % 3;
                int b = -a;
                return b;
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $2593, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax",
                "cdq",
                "movl $3, %r10d",
                "idivl %r10d",
                "movl %edx, -12(%rbp)",
                "movl -12(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "movl -8(%rbp), %r10d",
                "movl %r10d, -16(%rbp)",
                "negl -16(%rbp)",
                "movl -16(%rbp), %r10d",
                "movl %r10d, -20(%rbp)",
                "movl -20(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int return_val = 3;
                int void2 = 2;
                return return_val + void2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $3, -4(%rbp)",
                "movl $2, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -12(%rbp)",
                "movl -8(%rbp), %r10d",
                "addl %r10d, -12(%rbp)",
                "movl -12(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                a = a + 5;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $3, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "addl $5, -8(%rbp)",
                "movl -8(%rbp), %r10d",
                "movl %r10d, -4(%rbp)"
            ], missingReturn: true)
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 0;
                a = 3 * (b = a);
                return a + b;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "movl $0, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "movl $3, -12(%rbp)",
                "movl -12(%rbp), %r11d",
                "imull -8(%rbp), %r11d",
                "movl %r11d, -12(%rbp)",
                "movl -12(%rbp), %r10d",
                "movl %r10d, -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -16(%rbp)",
                "movl -8(%rbp), %r10d",
                "addl %r10d, -16(%rbp)",
                "movl -16(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                0 || (a = 1);
                return a;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, -4(%rbp)",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $1, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -8(%rbp)",
                "jmp .OR_END_L2",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -8(%rbp)",
                ".OR_END_L2:",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                ;
            }
            """,
            GetExpected(["subq $0, %rsp"], missingReturn: true)
        );
        Add
        (
            """
            int main(void) {
                ;
                return 0;
            }
            """,
            GetExpected([
                "subq $0, %rsp",
                "movl $0, %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                return a;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                0 && (a = 5);
                return a;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, -4(%rbp)",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $5, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -8(%rbp)",
                "jmp .AND_END_L2",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -8(%rbp)",
                ".AND_END_L2:",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                1 || (a = 1);
                return a;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, -4(%rbp)",
                "movl $1, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $1, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -8(%rbp)",
                "jmp .OR_END_L2",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -8(%rbp)",
                ".OR_END_L2:",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                2 + 2;
                return 0;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, -4(%rbp)",
                "addl $2, -4(%rbp)",
                "movl $0, %eax"
            ])
        );
        Add
        (
            """
            int main(void) {            
                int a = 1;
                int b = 2;
                return a = b = 4;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "movl $2, -8(%rbp)",
                "movl $4, -8(%rbp)",
                "movl -8(%rbp), %r10d",
                "movl %r10d, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0 && a;
                return a;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "cmpl $0, -4(%rbp)",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -8(%rbp)",
                "jmp .AND_END_L2",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -8(%rbp)",
                ".AND_END_L2:",
                "movl -8(%rbp), %r10d",
                "movl %r10d, -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
    }
}