using System.Runtime.InteropServices;

namespace Compiler.Common.Tacky;

public record TackyVariable : TackyValue
{
    private readonly int _varCount;
    public TackyVariable(int varCount)
    {
        Name = $"tmp.{varCount}";
        StackOffset = varCount * Marshal.SizeOf<int>();
        _varCount = varCount;
    }
    public string Name { get; }   
    public int StackOffset { get; }
    public TackyVariable Next() => new(_varCount + 1);
}