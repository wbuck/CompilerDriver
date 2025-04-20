namespace Compiler.Common.Test.Data.EmitterData;

public class EmitterValidData : TheoryData<string, string[]>
{
    public EmitterValidData()
    {
        Add
        (
            """
            int main(void) {
                return ~12;
            }
            """,
            GetOutput("main", [
                "subq $4, %rsp",
                "movl $12, -4(%rbp)",
                "notl -4(%rbp)",
                "movl -4(%rbp), %eax",
            ])
        );
        Add
        (
            """
            int main(void) {
                return -12;
            }
            """,
            GetOutput("main", [
                "subq $4, %rsp",
                "movl $12, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %eax",
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~-2147483647;
            }
            """,
            GetOutput("main", [
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
                return -(-4);
            }
            """,
            GetOutput("main", [
                "subq $8, %rsp",
                "movl $4, -4(%rbp)",
                "negl -4(%rbp)",
                "movl -4(%rbp), %r10d",
                "movl %r10d, -8(%rbp)",
                "negl -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
    }

    private static string[] GetOutput(string function, string[] instructions)
        => [
            $".globl {function}",
            $"{function}:",
            "pushq %rbp",
            "movq %rsp, %rbp",
            .. instructions,
            "movq %rbp, %rsp",
            "popq %rbp",
            "ret"
        ];
}