namespace Compiler.Common.Tokens;

public record KeywordToken(string Value, int Index, int Length)
    : Token(TokenType.Keyword, Length)
{
    private static readonly HashSet<string> Keywords = [
        "auto", "break", "case", "char", "const", "continue", "default", 
        "do", "double", "else", "enum", "extern", "float", "for", "goto", 
        "if", "int", "long", "register", "return", "short", "signed", "sizeof", 
        "static", "struct", "switch", "typedef", "union", "unsigned", "void", 
        "volatile", "while"
    ];

    public static void Parse(ReadOnlySpan<char> value, in List<Token> tokens)
    {
        var lookup = Keywords.GetAlternateLookup<ReadOnlySpan<char>>();
        
        for (var index = 0; index < value.Length; index++)
        {
            if (tokens[index] is not IdentifierToken token)
                continue;
            
            var identifier = value[token.Index..(token.Index + token.Length)];

            if (lookup.TryGetValue(identifier, out var keyword))            
                tokens[index] = new KeywordToken(keyword, token.Index, token.Length);            
        }
    }
}