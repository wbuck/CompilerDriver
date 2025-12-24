using System.Text.RegularExpressions;

namespace Compiler.Lexer.Tokens;

public sealed partial record IdentifierToken(int Index, int Length) : IToken
{
    private static readonly HashSet<string> Keywords = [..KeywordExtensions.GetValues().Select(k => k.ToStringFast(true))];
    
    public TokenType Type => TokenType.Identifier;
    
    [GeneratedRegex(@"[a-zA-Z_]\w*\b", RegexOptions.Singleline)]
    private static partial Regex Pattern { get; }

    public static IToken? Parse(ref ReadOnlySpan<char> value, int offset)
    {
        var lookup = Keywords.GetAlternateLookup<ReadOnlySpan<char>>();
        var enumerator = Pattern.EnumerateMatches(value);

        if (!enumerator.MoveNext() || enumerator.Current.Index != 0) 
            return null;
        
        var match = enumerator.Current;
        var identifier = value.Slice(match.Index, match.Length);
            
        value = value[match.Length..];
        return lookup.TryGetValue(identifier, out var keyword)
            ? new KeywordToken(match.Index + offset, match.Length, KeywordExtensions.Parse(keyword, true))
            : new IdentifierToken(match.Index + offset, match.Length);
    }
}