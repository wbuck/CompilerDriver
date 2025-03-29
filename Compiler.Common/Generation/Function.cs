using System.Text;

namespace Compiler.Common.Generation;

public record Function(ReadOnlyMemory<char> Name, List<IInstruction> Instructions) : IBuild
{
    public string Build()
    {
        StringBuilder builder = new();
        builder.AppendLine($".global _{Name}");
        builder.AppendLine($"_{Name}:");
        
        foreach (var instruction in Instructions)
            builder.AppendLine(instruction.Build());
        
        return builder.ToString();
    }
}