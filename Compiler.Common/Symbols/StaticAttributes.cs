namespace Compiler.Common.Symbols;

public sealed record StaticAttributes(StaticInitValue InitialValue, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Static;
}