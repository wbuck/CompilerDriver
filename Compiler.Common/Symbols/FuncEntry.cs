namespace Compiler.Common.Symbols;

public readonly record struct FuncEntry
(
    string Name,
    IType Type,
    IAttribute Attributes
) : IEntry;