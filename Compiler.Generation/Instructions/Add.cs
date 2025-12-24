namespace Compiler.Generation.Instructions;

public sealed record Add : IBinaryOperator
{
    public static Add Operator { get; } = new();
    private Add() { }
    public AssemblyTag Tag => AssemblyTag.Add;  
}