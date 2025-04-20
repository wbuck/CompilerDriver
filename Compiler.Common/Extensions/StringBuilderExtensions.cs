using System.Runtime.CompilerServices;
using System.Text;

namespace Compiler.Common.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendLine(this StringBuilder sb, string value, int indent)
        => sb.Append(' ', indent).AppendLine(value);
}