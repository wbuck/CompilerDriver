namespace Compiler.Common.Test.Data.Emitter;

public class DataBase : TheoryData<string, string[]>
{
    protected static string[] GetExpected(string[] instructions, bool missingReturn = false)
        => missingReturn ? GetExpectedWithoutReturn(instructions) : GetExpectedWithReturn(instructions);
    
    private static string[] GetExpectedWithReturn(string[] instructions)
        => [
            ".globl main",
            "main:",
            "pushq %rbp",
            "movq %rsp, %rbp",
            .. instructions,
            "movq %rbp, %rsp",
            "popq %rbp",
            "ret",
            "movl $0, %eax",
            "movq %rbp, %rsp",
            "popq %rbp",
            "ret"
        ];
    
    private static string[] GetExpectedWithoutReturn(string[] instructions)
        => [
            ".globl main",
            "main:",
            "pushq %rbp",
            "movq %rsp, %rbp",
            .. instructions,
            "movl $0, %eax",
            "movq %rbp, %rsp",
            "popq %rbp",
            "ret"
        ];
}