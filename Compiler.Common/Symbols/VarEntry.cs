namespace Compiler.Common.Symbols;

public readonly record struct VarEntry
(
    string Name,
    IType Type,
    IAttribute Attributes
) : IEntry;