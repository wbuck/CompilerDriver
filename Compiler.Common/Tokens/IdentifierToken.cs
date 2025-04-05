using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;


public partial record IdentifierToken : IToken
{
    private static readonly HashSet<string> Keywords = [
        "auto", "break", "case", "char", "const", "continue", "default", 
        "do", "double", "else", "enum", "extern", "float", "for", "goto", 
        "if", "int", "long", "register", "return", "short", "signed", "sizeof", 
        "static", "struct", "switch", "typedef", "union", "unsigned", "void", 
        "volatile", "while"
    ];
    
    public TokenType Type { get; }
    public int Index { get; }
    public int Length { get; }
    public string? Keyword { get; }
    
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
            ? new IdentifierToken(match.Index + offset, match.Length, keyword)
            : new IdentifierToken(match.Index + offset, match.Length);
    }
    
    private IdentifierToken(int index, int length)
        : this(TokenType.Identifier, index, length)
    { }

    private IdentifierToken(int index, int length, string keyword)
        : this(TokenType.Keyword, index, length, keyword)
    { }
    
    private IdentifierToken(TokenType type, int index, int length, string? keyword = null)
    {
        Type = type;
        Index = index;
        Length = length;
        Keyword = keyword;
    }
}