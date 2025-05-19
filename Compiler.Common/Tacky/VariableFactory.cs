namespace Compiler.Common.Tacky;

public class VariableFactory
{
    private TackyVariable? _variable;

    public TackyVariable GetNextVariable()
    {
        if (_variable is null)
        {
            _variable = new TackyVariable(1);
            return _variable;
        }
        _variable = _variable.Next();
        return _variable;
    }
}