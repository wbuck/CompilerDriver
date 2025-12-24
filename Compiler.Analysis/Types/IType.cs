namespace Compiler.Analysis.Types;

public interface IType
{
    public string TypeName { get; }
}

public readonly record struct Int : IType
{
    public static Int Instance { get; } = new();
    public string TypeName => "int";
}

public readonly record struct FuncDecl(int ParamCount) : IType
{
    public string TypeName => $"func({ParamCount})";
}