using System.Text.RegularExpressions;

namespace Compiler.Lexer.Tokens;

public sealed partial record BitwiseAndAssignmentToken(int Index) : IToken
{
    public TokenType Type => TokenType.BitwiseAndAssignment;
    public int Length => 2;
    
    [GeneratedRegex("&=", RegexOptions.Multiline)]
    private static partial Regex Pattern { get; }
    
    public static IToken? Parse(ref ReadOnlySpan<char> value, int offset)
    {
        var enumerator = Pattern.EnumerateMatches(value);
        if (!enumerator.MoveNext() || enumerator.Current.Index != 0) 
            return null;        
        
        var match = enumerator.Current;
        value = value[match.Length..];
        return new BitwiseAndAssignmentToken(match.Index + offset);       
    }
}