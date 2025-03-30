using System.Numerics;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public partial record IntegralConstant(int Index, int Length)
    : Token(TokenType.IntegralConstant, Index, Length)
{
    [GeneratedRegex(@"[0-9]+\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
    {
        foreach (var match in Pattern.EnumerateMatches(value))        
            tokens.Add(new IntegralConstant(match.Index, match.Length));        
    }
}