namespace Compiler.Common.Helpers;

public class LabelGenerator
{
    private int _count = 1;

    public string GetNextLabel(ReadOnlySpan<char> name)
        => name.StartsWith('.')
            ? $"{name}{_count++}"
            : $".{name}{_count++}";
}