using System.Runtime.InteropServices;

namespace Compiler.Common.Tacky;

public sealed class VariableFactory
{
    private int _count = 1;

    public TackyVariable GetNextVariable(string? identifier = null)
    {
        var id = identifier ?? $"tmp.{_count++}";
        return new TackyVariable(id);
    }
}