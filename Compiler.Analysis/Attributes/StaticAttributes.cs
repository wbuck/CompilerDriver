namespace Compiler.Analysis.Attributes;

public sealed record StaticAttributes(StaticInitValue InitialValue, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Static;
}