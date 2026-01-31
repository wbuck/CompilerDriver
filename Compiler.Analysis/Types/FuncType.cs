namespace Compiler.Analysis.Types;

public readonly record struct FuncType(int ParamCount) : IType
{
    public string TypeName => $"func({ParamCount})";
}