namespace Compiler.Common.Tokens;

public interface IToken
{
    public TokenType Type { get; }
    public int Index { get; }
    public int Length { get; }
    static virtual IToken? Parse(ref ReadOnlySpan<char> value, int offset) 
        => throw new NotImplementedException();
}