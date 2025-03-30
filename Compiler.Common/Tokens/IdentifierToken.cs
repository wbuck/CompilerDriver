using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;
public partial record IdentifierToken(int Index, int Length) 
    : Token(TokenType.Identifier, Index, Length)
{
    private static readonly HashSet<string> Keywords = [
        "auto", "break", "case", "char", "const", "continue", "default", 
        "do", "double", "else", "enum", "extern", "float", "for", "goto", 
        "if", "int", "long", "register", "return", "short", "signed", "sizeof", 
        "static", "struct", "switch", "typedef", "union", "unsigned", "void", 
        "volatile", "while"
    ];
    
    [GeneratedRegex(@"[a-zA-Z_]\w*\b", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }

    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
    {
        var lookup = Keywords.GetAlternateLookup<ReadOnlySpan<char>>();        
        foreach (var match in Pattern.EnumerateMatches(value))
        {
            var identifier = value.Slice(match.Index, match.Length);
            
            Token token = lookup.TryGetValue(identifier, out var keyword)
                ? new KeywordToken(keyword, match.Index, match.Length)
                : new IdentifierToken(match.Index, match.Length);
            
            tokens.Add(token);
        }
    }
}