
using System.Diagnostics;
using System.Text.RegularExpressions;
using Compiler.Common.Tokens;

namespace Compiler.Common.Stages;

public static partial class Lexer
{
    [GeneratedRegex(@"\S+", RegexOptions.Singleline)]
    private static partial Regex NonWhiteSpacePattern { get; }
    
    public static List<IToken> Lex(ReadOnlySpan<char> fileContent)
    {
        List<IToken> tokens = [];
        foreach (var range in fileContent.Split('\n'))
        {                        
            var line = fileContent[range];
            var trimmed = line.TrimStart();

            while (!trimmed.IsWhiteSpace())
            {
                var offset = range.Start.Value + line.Length - trimmed.Length;

                IToken? token = null;
                if ((token = Parse<OpenBraceToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }                
                if ((token = Parse<CloseBraceToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }                
                if ((token = Parse<OpenParenthesisToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }
                if ((token = Parse<CloseParenthesisToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }
                if ((token = Parse<CommaToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }          
                if ((token = Parse<IdentifierToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                } 
                if ((token = Parse<SemiColonToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                } 
                if ((token = Parse<SemiColonToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }  
                if ((token = Parse<IntegralConstantToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }  
                if ((token = Parse<ArithmeticToken>(trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    trimmed = trimmed[token.Length..].TrimStart();
                    continue;
                }  
                
                var enumerator = NonWhiteSpacePattern.EnumerateMatches(trimmed);
                if (enumerator.MoveNext())
                {
                    trimmed = trimmed[enumerator.Current.Length ..].TrimStart();
                    Debugger.Break();
                }
            }
            
        }
        
        
        
        
        
        
        tokens.Sort((t1, t2) => t1.Index.CompareTo(t2.Index));
        return tokens;
        
        static IToken? Parse<T>(ReadOnlySpan<char> fileContent, int offset) where T : IToken
            => T.Parse(fileContent, offset);
    }

    
}