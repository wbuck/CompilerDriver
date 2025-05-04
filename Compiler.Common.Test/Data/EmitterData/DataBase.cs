namespace Compiler.Common.Test.Data.EmitterData;

public class DataBase : TheoryData<string, string[]>
{
    protected static string[] Emit(string[] instructions)
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