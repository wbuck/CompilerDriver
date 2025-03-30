namespace Compiler.Common.Tokens;

public interface IToken
{
    public TokenType Type { get; }
    public int Index { get; }
    public int Length { get; }
    static virtual void Parse(ReadOnlySpan<char> value, in List<IToken> tokens) =>
        throw new NotImplementedException();
    
    static void FindCharacter(
        ReadOnlySpan<char> haystack, char needle, in List<IToken> tokens, Action<int, List<IToken>> found)
    {
        for (var index = 0; index < haystack.Length; index++)
        {
            if (haystack[index] == needle)
                found(index, tokens);
        }
    }
}