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
    
    [GeneratedRegex(@"[a-zA-Z_]\w*\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static void Parse(ReadOnlySpan<char> value, in List<IToken> tokens)
    {
        var lookup = Keywords.GetAlternateLookup<ReadOnlySpan<char>>();        
        foreach (var match in Pattern.EnumerateMatches(value))
        {
            var identifier = value.Slice(match.Index, match.Length);
            
            IToken token = lookup.TryGetValue(identifier, out var keyword)
                ? new IdentifierToken(match.Index, match.Length, keyword)
                : new IdentifierToken(match.Index, match.Length);
            
            tokens.Add(token);
        }
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