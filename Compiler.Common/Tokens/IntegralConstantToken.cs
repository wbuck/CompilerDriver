using System.Numerics;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public partial record IntegralConstantToken(int Index, int Length) : IToken
{
    public TokenType Type => TokenType.IntegralConstant;
    
    [GeneratedRegex(@"-?[0-9]+\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static IToken? Parse(ReadOnlySpan<char> value, int offset)
    {
        var enumerator = Pattern.EnumerateMatches(value);

        if (!enumerator.MoveNext() || enumerator.Current.Index != 0) 
            return null;
        
        var match = enumerator.Current;
        return new IntegralConstantToken(match.Index + offset, match.Length);       
    }
}