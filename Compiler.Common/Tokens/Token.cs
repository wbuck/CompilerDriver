namespace Compiler.Common.Tokens;

public abstract record Token(TokenType Type, int Index = 0, int Length = 0)
{
    protected static void FindCharacter(
        ReadOnlySpan<char> haystack, char needle, in List<Token> tokens, Action<int, List<Token>> found)
    {
        for (var index = 0; index < haystack.Length; index++)
        {
            if (haystack[index] == needle)
                found(index, tokens);
        }
    }
}