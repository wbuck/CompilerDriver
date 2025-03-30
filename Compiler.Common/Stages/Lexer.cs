using Compiler.Common.Tokens;

namespace Compiler.Common.Stages;

public static class Lexer
{
    public static List<IToken> Lex(ReadOnlySpan<char> fileContent)
    {
        List<IToken> tokens = [];
        
        Parse<OpenBraceToken>(fileContent, tokens);
        Parse<CloseBraceToken>(fileContent, tokens);
        Parse<OpenParenthesisToken>(fileContent, tokens);
        Parse<CloseParenthesisToken>(fileContent, tokens);
        Parse<CommaToken>(fileContent, tokens);
        Parse<IdentifierToken>(fileContent, tokens);
        Parse<SemiColonToken>(fileContent, tokens);
        Parse<IntegralConstantToken>(fileContent, tokens);
        
        tokens.Sort((t1, t2) => t1.Index.CompareTo(t2.Index));
        
        return tokens;
        
        static void Parse<T>(ReadOnlySpan<char> fileContent, in List<IToken> tokens) where T : IToken
            => T.Parse(fileContent, tokens);
    }

    
}