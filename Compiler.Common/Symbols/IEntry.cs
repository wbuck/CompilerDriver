namespace Compiler.Common.Symbols;

public interface IEntry
{
    string Name { get; }
    IType Type { get; }
    IAttribute Attributes { get; }

    public static TAttribute GetAttribute<TAttribute>(IEntry entry) 
        where TAttribute : class, IAttribute
        => (TAttribute)entry.Attributes;
}