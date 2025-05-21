namespace Compiler.Common.Tacky;

public class LabelGenerator
{
    private int _count = 1;

    public string GetNextLabel(ReadOnlySpan<char> name)
        => $".{name}{_count++}";
}