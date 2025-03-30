using System.Text.RegularExpressions;

namespace Compiler.Common.Tokens;

public abstract record Token(TokenType Type, int Index = 0, int Length = 0)
{
    public static Token Parse(ReadOnlySpan<char> value)
    {
        throw new NotImplementedException();
    }
}