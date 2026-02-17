namespace Compiler.Common.Symbols;

public sealed record FuncAttributes(bool Defined, bool Global) : IAttribute
{
    public AttributeType Type => AttributeType.Function;
}