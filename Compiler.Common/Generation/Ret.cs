namespace Compiler.Common.Generation;

public record Ret : IInstruction
{
    public string Build() => "ret";
}