namespace Compiler.Common.Test.Data.Emitter;

public class DataBase : TheoryData<string, string[]>
{
    protected static string[] GetExpected(string[] instructions)
        => [
            ".globl main",
            "main:",
            "pushq %rbp",
            "movq %rsp, %rbp",
            .. instructions,
            "movq %rbp, %rsp",
            "popq %rbp",
            "ret"
        ];
}