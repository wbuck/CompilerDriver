using System.Numerics;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public partial record IntegralConstantToken(int Index, int Length) : IToken
{
    public TokenType Type => TokenType.IntegralConstant;
    
    [GeneratedRegex(@"[0-9]+\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static void Parse(ReadOnlySpan<char> value, in List<IToken> tokens)
    {
        foreach (var match in Pattern.EnumerateMatches(value))        
            tokens.Add(new IntegralConstantToken(match.Index, match.Length));        
    }
}