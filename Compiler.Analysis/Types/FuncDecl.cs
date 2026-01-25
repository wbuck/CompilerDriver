namespace Compiler.Analysis.Types;

public readonly record struct FuncDecl(int ParamCount) : IType
{
    public string TypeName => $"func({ParamCount})";
}