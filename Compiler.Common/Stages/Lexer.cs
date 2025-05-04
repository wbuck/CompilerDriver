using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Compiler.Common.Tokens;

namespace Compiler.Common.Stages;

public static partial class Lexer
{
    [GeneratedRegex(@"\S+", RegexOptions.Singleline)]
    private static partial Regex NonWhiteSpacePattern { get; }
    
    public static bool TryTokenize(ReadOnlySpan<char> fileContent, [NotNullWhen(true)] out List<IToken>? tokens)
    {        
        tokens = [];
        foreach (var range in fileContent.Split('\n'))
        {                        
            var line = fileContent[range];
            var trimmed = line.TrimStart();

            while (!trimmed.IsWhiteSpace())
            {
                var offset = range.Start.Value + line.Length - trimmed.Length;

                IToken? token;
                if ((token = Parse<OpenBraceToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);
                    continue;
                }                
                if ((token = Parse<CloseBraceToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }                
                if ((token = Parse<OpenParenthesisToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<CloseParenthesisToken>(ref trimmed, offset)) is not null)
                {                    
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<CommaToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }          
                if ((token = Parse<IdentifierToken>(ref trimmed, offset)) is not null)
                {                    
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<SemicolonToken>(ref trimmed, offset)) is not null)
                {                    
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<SemicolonToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<NumericConstantToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<BitwiseComplementToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<DecrementToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<MinusToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<PlusToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<AsteriskToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<ForwardSlashToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<PercentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }         
                
                var enumerator = NonWhiteSpacePattern.EnumerateMatches(trimmed);
                if (enumerator.MoveNext())
                {
                    var match = enumerator.Current;
                    var unknown = trimmed.Slice(match.Index, match.Length);
                    
                    PrintError($"Unexpected token: {unknown}");
                    return false;
                }
            }     
        }
        
        tokens.Sort((t1, t2) => t1.Index.CompareTo(t2.Index));
        return true;

        static IToken? Parse<T>(ref ReadOnlySpan<char> line, int offset) where T : IToken
        {
            if (T.Parse(ref line, offset) is not { } token) 
                return null;
            
            line = line.TrimStart();
            return token;
        }
    }
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}