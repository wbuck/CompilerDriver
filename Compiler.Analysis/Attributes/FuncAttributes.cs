namespace Compiler.Analysis.Attributes;

public sealed record FuncAttributes(bool Defined, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Function;
}