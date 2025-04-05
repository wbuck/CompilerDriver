using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public partial record IdentifierToken(int Index, int Length) : IToken
{
    private static readonly HashSet<string> Keywords = [
        "auto", "break", "case", "char", "const", "continue", "default", 
        "do", "double", "else", "enum", "extern", "float", "for", "goto", 
        "if", "int", "long", "register", "return", "short", "signed", "sizeof", 
        "static", "struct", "switch", "typedef", "union", "unsigned", "void", 
        "volatile", "while"
    ];
    
    public TokenType Type => TokenType.Identifier;
    
    [GeneratedRegex(@"[a-zA-Z_]\w*\b", RegexOptions.Singleline)]
    private static partial Regex Pattern { get; }

    public static IToken? Parse(ReadOnlySpan<char> value, int offset)
    {
        var lookup = Keywords.GetAlternateLookup<ReadOnlySpan<char>>();
        var enumerator = Pattern.EnumerateMatches(value);

        if (!enumerator.MoveNext() || enumerator.Current.Index != 0) 
            return null;
        
        var match = enumerator.Current;
        var identifier = value.Slice(match.Index, match.Length);
            
        return lookup.TryGetValue(identifier, out var keyword)
            ? new KeywordToken(match.Index + offset, match.Length, keyword)
            : new IdentifierToken(match.Index + offset, match.Length);
    }
}