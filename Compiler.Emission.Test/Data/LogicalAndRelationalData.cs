namespace Compiler.Emission.Test.Data;

public class LogicalAndRelationalData : DataBase
{
    public LogicalAndRelationalData()
    {
        Add
        (
            """
            int main(void) {
                return (10 && 0) + (0 && 4) + (0 && 0);
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $10, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -4(%rbp)",
                "jmp .AND_END_L2",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -4(%rbp)",
                ".AND_END_L2:",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L3",
                "movl $4, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L3",
                "movl $1, -8(%rbp)",
                "jmp .AND_END_L4",
                ".AND_WHEN_ZERO_L3:",
                "movl $0, -8(%rbp)",
                ".AND_END_L4:",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -12(%rbp)",
                "movl -8(%rbp), %r10d",
                "addl %r10d, -12(%rbp)",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L5",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L5",
                "movl $1, -16(%rbp)",
                "jmp .AND_END_L6",
                ".AND_WHEN_ZERO_L5:",
                "movl $0, -16(%rbp)",
                ".AND_END_L6:",
                "movl -12(%rbp), %r10d",
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
                return 0 && (1 / 0);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, %eax",
                "cdq",
                "movl $0, %r10d",
                "idivl %r10d",
                "movl %eax, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -8(%rbp)",
                "jmp .AND_END_L2",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -8(%rbp)",
                ".AND_END_L2:",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 && -1;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -4(%rbp)",
                "negl -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -8(%rbp)",
                "jmp .AND_END_L2",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -8(%rbp)",
                ".AND_END_L2:",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 >= 0 > 1 <= 0;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $5, %r11d",
                "cmpl $0, %r11d",
                "movl $0, -4(%rbp)",
                "setge -4(%rbp)",
                "cmpl $1, -4(%rbp)",
                "movl $0, -8(%rbp)",
                "setg -8(%rbp)",
                "cmpl $0, -8(%rbp)",
                "movl $0, -12(%rbp)",
                "setle -12(%rbp)",
                "movl -12(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~2 * -2 == 1 + 5;
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $2, -4(%rbp)",
                "notl -4(%rbp)",
                "movl $2, -8(%rbp)",
                "negl -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -12(%rbp)",
                "movl -12(%rbp), %r11d",
                "imull -8(%rbp), %r11d",
                "movl %r11d, -12(%rbp)",
                "movl $1, -16(%rbp)",
                "addl $5, -16(%rbp)",
                "movl -16(%rbp), %r10d",
                "cmpl %r10d, -12(%rbp)",
                "movl $0, -20(%rbp)",
                "sete -20(%rbp)",
                "movl -20(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 == 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $2, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 3 == 1 != 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $3, %r11d",
                "cmpl $1, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "cmpl $2, -4(%rbp)",
                "movl $0, -8(%rbp)",
                "setne -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 == 1;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $1, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 >= 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $2, %r11d",
                "movl $0, -4(%rbp)",
                "setge -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 >= 1) + (1 >= -4);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $1, %r11d",
                "movl $0, -4(%rbp)",
                "setge -4(%rbp)",
                "movl $4, -8(%rbp)",
                "negl -8(%rbp)",
                "movl $1, %r11d",
                "cmpl -8(%rbp), %r11d",
                "movl $0, -12(%rbp)",
                "setge -12(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -16(%rbp)",
                "movl -12(%rbp), %r10d",
                "addl %r10d, -16(%rbp)",
                "movl -16(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 > 2) + (1 > 1);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $2, %r11d",
                "movl $0, -4(%rbp)",
                "setg -4(%rbp)",
                "movl $1, %r11d",
                "cmpl $1, %r11d",
                "movl $0, -8(%rbp)",
                "setg -8(%rbp)",
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
                return 15 > 10;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $15, %r11d",
                "cmpl $10, %r11d",
                "movl $0, -4(%rbp)",
                "setg -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 <= -1;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "negl -4(%rbp)",
                "movl $1, %r11d",
                "cmpl -4(%rbp), %r11d",
                "movl $0, -8(%rbp)",
                "setle -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (0 <= 2) + (0 <= 0);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, %r11d",
                "cmpl $2, %r11d",
                "movl $0, -4(%rbp)",
                "setle -4(%rbp)",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "movl $0, -8(%rbp)",
                "setle -8(%rbp)",
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
                return 2 < 1;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, %r11d",
                "cmpl $1, %r11d",
                "movl $0, -4(%rbp)",
                "setl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 < 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $2, %r11d",
                "movl $0, -4(%rbp)",
                "setl -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 || 0 && (1 / 0);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L2",
                "movl $1, %eax",
                "cdq",
                "movl $0, %r10d",
                "idivl %r10d",
                "movl %eax, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "je .AND_WHEN_ZERO_L2",
                "movl $1, -8(%rbp)",
                "jmp .AND_END_L3",
                ".AND_WHEN_ZERO_L2:",
                "movl $0, -8(%rbp)",
                ".AND_END_L3:",
                "cmpl $0, -8(%rbp)",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -12(%rbp)",
                "jmp .OR_END_L4",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -12(%rbp)",
                ".OR_END_L4:",
                "movl -12(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 != 0;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "movl $0, -4(%rbp)",
                "setne -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return -1 != -2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, -4(%rbp)",
                "negl -4(%rbp)",
                "movl $2, -8(%rbp)",
                "negl -8(%rbp)",
                "movl -8(%rbp), %r10d",
                "cmpl %r10d, -4(%rbp)",
                "movl $0, -12(%rbp)",
                "setne -12(%rbp)",
                "movl -12(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return !-3;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $3, -4(%rbp)",
                "negl -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "movl $0, -8(%rbp)",
                "sete -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return !(3 - 44);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $3, -4(%rbp)",
                "subl $44, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "movl $0, -8(%rbp)",
                "sete -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return !(4-4);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $4, -4(%rbp)",
                "subl $4, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "movl $0, -8(%rbp)",
                "sete -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return !0;
            }
            """,            
            GetExpected([
                "subq $16, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return !5;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $5, %r11d",
                "cmpl $0, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "movl -4(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~(0 && 1) - -(4 || 3);
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -4(%rbp)",
                "jmp .AND_END_L2",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -4(%rbp)",
                ".AND_END_L2:",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "notl -8(%rbp)",
                "movl $4, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L3",
                "movl $3, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L3",
                "movl $0, -12(%rbp)",
                "jmp .OR_END_L4",
                ".OR_WHEN_NOT_ZERO_L3:",
                "movl $1, -12(%rbp)",
                ".OR_END_L4:",
                "movl -12(%rbp), %r10d",
                "movl %r10d, -16(%rbp)",
                "negl -16(%rbp)",
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
                return 1 || (1 / 0);
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $1, %eax",
                "cdq",
                "movl $0, %r10d",
                "idivl %r10d",
                "movl %eax, -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -8(%rbp)",
                "jmp .OR_END_L2",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -8(%rbp)",
                ".OR_END_L2:",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (4 || 0) + (0 || 3) + (5 || 5);
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $4, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -4(%rbp)",
                "jmp .OR_END_L2",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -4(%rbp)",
                ".OR_END_L2:",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L3",
                "movl $3, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L3",
                "movl $0, -8(%rbp)",
                "jmp .OR_END_L4",
                ".OR_WHEN_NOT_ZERO_L3:",
                "movl $1, -8(%rbp)",
                ".OR_END_L4:",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -12(%rbp)",
                "movl -8(%rbp), %r10d",
                "addl %r10d, -12(%rbp)",
                "movl $5, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L5",
                "movl $5, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L5",
                "movl $0, -16(%rbp)",
                "jmp .OR_END_L6",
                ".OR_WHEN_NOT_ZERO_L5:",
                "movl $1, -16(%rbp)",
                ".OR_END_L6:",
                "movl -12(%rbp), %r10d",
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
                return (1 || 0) && 0;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L2",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L2",
                "movl $0, -4(%rbp)",
                "jmp .OR_END_L3",
                ".OR_WHEN_NOT_ZERO_L2:",
                "movl $1, -4(%rbp)",
                ".OR_END_L3:",
                "cmpl $0, -4(%rbp)",
                "je .AND_WHEN_ZERO_L1",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -8(%rbp)",
                "jmp .AND_END_L4",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -8(%rbp)",
                ".AND_END_L4:",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 >= 0;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, %r11d",
                "cmpl $0, %r11d",
                "movl $0, -4(%rbp)",
                "setge -4(%rbp)",
                "movl $2, %r11d",
                "cmpl -4(%rbp), %r11d",
                "movl $0, -8(%rbp)",
                "sete -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 || 0;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $2, %r11d",
                "cmpl $2, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -8(%rbp)",
                "jmp .OR_END_L2",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -8(%rbp)",
                ".OR_END_L2:",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return (0 == 0 && 3 == 2 + 1 > 1) + 1;
            }
            """,
            GetExpected([
                "subq $32, %rsp",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "cmpl $0, -4(%rbp)",
                "je .AND_WHEN_ZERO_L1",
                "movl $2, -8(%rbp)",
                "addl $1, -8(%rbp)",
                "cmpl $1, -8(%rbp)",
                "movl $0, -12(%rbp)",
                "setg -12(%rbp)",
                "movl $3, %r11d",
                "cmpl -12(%rbp), %r11d",
                "movl $0, -16(%rbp)",
                "sete -16(%rbp)",
                "cmpl $0, -16(%rbp)",
                "je .AND_WHEN_ZERO_L1",
                "movl $1, -20(%rbp)",
                "jmp .AND_END_L2",
                ".AND_WHEN_ZERO_L1:",
                "movl $0, -20(%rbp)",
                ".AND_END_L2:",
                "movl -20(%rbp), %r10d",
                "movl %r10d, -24(%rbp)",
                "addl $1, -24(%rbp)",
                "movl -24(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 || 0 && 2;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $1, %r11d",
                "cmpl $0, %r11d",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L2",
                "movl $2, %r11d",
                "cmpl $0, %r11d",
                "je .AND_WHEN_ZERO_L2",
                "movl $1, -4(%rbp)",
                "jmp .AND_END_L3",
                ".AND_WHEN_ZERO_L2:",
                "movl $0, -4(%rbp)",
                ".AND_END_L3:",
                "cmpl $0, -4(%rbp)",
                "jne .OR_WHEN_NOT_ZERO_L1",
                "movl $0, -8(%rbp)",
                "jmp .OR_END_L4",
                ".OR_WHEN_NOT_ZERO_L1:",
                "movl $1, -8(%rbp)",
                ".OR_END_L4:",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 & 7 == 5;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $7, %r11d",
                "cmpl $5, %r11d",
                "movl $0, -4(%rbp)",
                "sete -4(%rbp)",
                "movl $5, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "andl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 | 7 != 5;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $7, %r11d",
                "cmpl $5, %r11d",
                "movl $0, -4(%rbp)",
                "setne -4(%rbp)",
                "movl $5, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "orl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 20 >> 4 <= 3 << 1;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $20, -4(%rbp)",
                "sarl $4, -4(%rbp)",
                "movl $3, -8(%rbp)",
                "sall $1, -8(%rbp)",
                "movl -8(%rbp), %r10d",
                "cmpl %r10d, -4(%rbp)",
                "movl $0, -12(%rbp)",
                "setle -12(%rbp)",
                "movl -12(%rbp), %eax"
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 ^ 7 < 5;
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $7, %r11d",
                "cmpl $5, %r11d",
                "movl $0, -4(%rbp)",
                "setl -4(%rbp)",
                "movl $5, -8(%rbp)",
                "movl -4(%rbp), %r10d",
                "xorl %r10d, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
    }
}