using System.Numerics;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public partial record IntegralConstant<T>(T Value, int Length)
    : Token(TokenType.Constant, Length) where T : IBinaryInteger<T>
{
    [GeneratedRegex(@"[0-9]+\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
    {
        foreach (var match in Pattern.EnumerateMatches(value))
        {
            tokens.Add(new IdentifierToken(match.Index, match.Length));
        }
    }
}