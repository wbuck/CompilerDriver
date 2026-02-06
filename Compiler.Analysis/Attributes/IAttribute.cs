using System.Numerics;
using NetEscapades.EnumGenerators;

namespace Compiler.Analysis.Attributes;

[EnumExtensions]
public enum AttributeType
{
    Function,
    Static,
    Local
}

public interface IAttribute
{
    public AttributeType Type { get; }
}

public sealed record FuncAttributes(bool Defined, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Function;
}

public sealed record LocalAttributes : IAttribute
{
    public static LocalAttributes Instance { get; } = new();
    private LocalAttributes() { }
    public AttributeType Type => AttributeType.Local;
}

public abstract record StaticInitValue;

public sealed record Tentative : StaticInitValue
{
    public static Tentative Instance { get; } = new();
    private Tentative()
    { }
}

public sealed record NoInitializer : StaticInitValue
{
    public static NoInitializer Instance { get; } = new();
    private NoInitializer()
    { }
}

public interface IConstantInit;
public sealed record Initial<T>(T Value) : StaticInitValue, IConstantInit where T: INumber<T>;

public sealed record StaticAttributes(StaticInitValue InitialValue, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Static;
}