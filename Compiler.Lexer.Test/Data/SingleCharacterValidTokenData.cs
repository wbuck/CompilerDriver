using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class SingleCharacterValidTokenData : TheoryData<string, Type, TokenType>
{
    public SingleCharacterValidTokenData()
    {                                        
        Add(GetValue(TokenType.OpenParenthesis), typeof(OpenParenthesisToken), TokenType.OpenParenthesis);
        Add(GetValue(TokenType.CloseParenthesis), typeof(CloseParenthesisToken), TokenType.CloseParenthesis);
        Add(GetValue(TokenType.OpenBrace), typeof(OpenBraceToken), TokenType.OpenBrace);
        Add(GetValue(TokenType.CloseBrace), typeof(CloseBraceToken), TokenType.CloseBrace);
        Add(GetValue(TokenType.Comma), typeof(CommaToken), TokenType.Comma);
        Add(GetValue(TokenType.Semicolon), typeof(SemicolonToken), TokenType.Semicolon);
        Add(GetValue(TokenType.Complement), typeof(BitwiseComplementToken), TokenType.Complement);
        Add(GetValue(TokenType.Decrement), typeof(DecrementToken), TokenType.Decrement);
        Add(GetValue(TokenType.Plus), typeof(PlusToken), TokenType.Plus);
        Add(GetValue(TokenType.Minus), typeof(MinusToken), TokenType.Minus);
        Add(GetValue(TokenType.Asterisk), typeof(AsteriskToken), TokenType.Asterisk);
        Add(GetValue(TokenType.ForwardSlash), typeof(ForwardSlashToken), TokenType.ForwardSlash);
        Add(GetValue(TokenType.Percent), typeof(PercentToken), TokenType.Percent);
        Add(GetValue(TokenType.BitwiseAnd), typeof(BitwiseAndToken), TokenType.BitwiseAnd);
        Add(GetValue(TokenType.BitwiseOr), typeof(BitwiseOrToken), TokenType.BitwiseOr);
    }
    
    private static string GetValue(TokenType type)
        => type.ToStringFast(true);
}