namespace Compiler.Common.Generation;

public record Program(Function Function) : IBuild
{
    public string Build()
    => Function.Build();
}