namespace Compiler.Analysis.Attributes;

public sealed record LocalAttributes : IAttribute
{
    public static LocalAttributes Instance { get; } = new();
    private LocalAttributes() { }
    public AttributeType Type => AttributeType.Local;
}