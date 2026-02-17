namespace Compiler.Common.Symbols;

public readonly record struct FuncType(int ParamCount) : IType
{
    public string TypeName => $"func({ParamCount})";
}