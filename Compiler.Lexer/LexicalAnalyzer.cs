using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Compiler.Lexer.Tokens;

namespace Compiler.Lexer;

public static partial class LexicalAnalyzer
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
                if ((token = Parse<IncrementToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<AdditionAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<SubtractionAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<MultiplicationAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<DivisionAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<RemainderAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<BitwiseAndAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<BitwiseOrAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<BitwiseXorAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<LeftShiftAssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<RightShiftAssignmentToken>(ref trimmed, offset)) is not null)
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
                if ((token = Parse<LogicalAndToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<LogicalOrToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<EqualToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<NotEqualToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<GreaterThanOrEqualToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<LessThanOrEqualToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }                
                if ((token = Parse<BitwiseAndToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<BitwiseOrToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<BitwiseXorToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<LeftShiftToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }  
                if ((token = Parse<RightShiftToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                } 
                if ((token = Parse<LessThanToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<GreaterThanToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<NotToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<AssignmentToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<ColonToken>(ref trimmed, offset)) is not null)
                {
                    tokens.Add(token);                    
                    continue;
                }
                if ((token = Parse<QuestionMarkToken>(ref trimmed, offset)) is not null)
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