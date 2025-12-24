namespace Compiler.Emission.Test.Data;

public class FunctionData : DataBase
{
    public FunctionData()
    {
        Add
        (
            """
            int putchar(int c);
           
            int foo(int a, int b, int c, int d, int e, int f, int g, int h) {
                putchar(h);
                return a + g;
            }
            
            int main(void) {
                return foo(1, 2, 3, 4, 5, 6, 7, 65);
            }
            """,
            [
                ".globl _foo",
                "_foo:",
                "pushq %rbp",
                "movq %rsp, %rbp",
                "subq $48, %rsp",
                "movl %edi, -4(%rbp)",
                "movl %esi, -8(%rbp)",
                "movl %edx, -12(%rbp)",
                "movl %ecx, -16(%rbp)",
                "movl %r8d, -20(%rbp)",
                "movl %r9d, -24(%rbp)",
                "movl 16(%rbp), %r10d",  // move g into R10
                "movl %r10d, -28(%rbp)", // move g into Stack(-28)
                "movl 24(%rbp), %r10d",  // move h into R10
                "movl %r10d, -32(%rbp)", // move h into Stack(-32)
                "movl -32(%rbp), %edi",  // move h into EDI
                "call _putchar",         // call putchar with h
                "movl %eax, -36(%rbp)",  // move return value into Stack(-36)
                "movl -4(%rbp), %r10d",  // move a into R10
                "movl %r10d, -40(%rbp)", // move a into Stack(-40)
                "movl -28(%rbp), %r10d", // move g into R10
                "addl %r10d, -40(%rbp)", // add a + g into Stack(-40)
                "movl -40(%rbp), %eax",  // move result in Stack(-40) into EAX
                "movq %rbp, %rsp",
                "popq %rbp",
                "ret",
                "movl $0, %eax",
                "movq %rbp, %rsp",
                "popq %rbp",
                "ret",
                ".globl _main",
                "_main:",
                "pushq %rbp",
                "movq %rsp, %rbp",
                "subq $16, %rsp",
                "movl $1, %edi",
                "movl $2, %esi",
                "movl $3, %edx",
                "movl $4, %ecx",
                "movl $5, %r8d",
                "movl $6, %r9d",
                "pushq $65",
                "pushq $7",
                "call _foo",
                "addq $16, %rsp",
                "movl %eax, -4(%rbp)",
                "movl -4(%rbp), %eax",
                "movq %rbp, %rsp",
                "popq %rbp",
                "ret",
                "movl $0, %eax",
                "movq %rbp, %rsp",
                "popq %rbp",
                "ret"
            ]
        );
    }
}