using System.Numerics;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public partial record NumericConstantToken(int Index, int Length) : IToken
{
    public TokenType Type => TokenType.NumericConstant;
    
    // [GeneratedRegex(@"((?:[+]?[-]?|[-]?[+]?)(?:[0-9]*?[.])?[0-9]+\b)", RegexOptions.Multiline)]
    [GeneratedRegex(@"(?:[0-9]*?[.])?[0-9]+\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static IToken? Parse(ref ReadOnlySpan<char> value, int offset)
    {
        var enumerator = Pattern.EnumerateMatches(value);
        if (!enumerator.MoveNext() || enumerator.Current.Index != 0) 
            return null;
        
        var match = enumerator.Current;
        value = value[match.Length..];
        return new NumericConstantToken(match.Index + offset, match.Length);       
    }
}