using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public interface IToken
{
    static abstract Token Parse(ReadOnlySpan<char> value, in List<Token> tokens);
}

public abstract record Token(TokenType Type, int Index = 0, int Length = 0);