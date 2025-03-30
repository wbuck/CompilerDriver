using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public interface IToken
{
    static abstract Token Parse(ReadOnlySpan<char> value);
}

public partial record IdentifierToken(int Index, int Length) 
    : Token(TokenType.Identifier, Index, Length)
{
    [GeneratedRegex(@"[a-zA-Z_]\w*\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
    {
        foreach (var match in Pattern.EnumerateMatches(value))
        {
            tokens.Add(new IdentifierToken(match.Index, match.Length));
        }
    }
    
    
}