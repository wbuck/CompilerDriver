using System.Runtime.InteropServices;

namespace Compiler.Common.Tacky;

public sealed class VariableFactory
{
    private TackyVariable? _variable;

    public TackyVariable GetNextVariable()
    {
        if (_variable is null)
        {
            _variable = new TackyVariable("tmp.1", 1, 1 * Marshal.SizeOf<int>());
            return _variable;
        }
        
        var next = _variable.VariableCount + 1;
        _variable = new TackyVariable($"tmp.{next}", next, next * Marshal.SizeOf<int>());
        return _variable;
    }
}