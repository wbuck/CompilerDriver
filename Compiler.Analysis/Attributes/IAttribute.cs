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

public sealed record FunctionAttributes(bool Defined, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Function;
}

public sealed record LocalAttributes : IAttribute
{
    public AttributeType Type => AttributeType.Local;
}

public abstract record StaticInitValue;

public sealed record Tentative : StaticInitValue;
public sealed record NoInitializer : StaticInitValue;
public sealed record Initial<T>(T Value) :  StaticInitValue where T: INumber<T>;

public sealed record StaticAttributes(StaticInitValue InitialValue, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Static;
}