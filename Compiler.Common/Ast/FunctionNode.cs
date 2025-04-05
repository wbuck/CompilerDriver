using System.Diagnostics.Contracts;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record FunctionNode(string Name, Node[] Arguments, Node Body) : Node
{
    public override NodeType NodeType => NodeType.Function;
    /*
     * <argument_list> :: = <argument>* | "void"
     * <argument> :: = <keyword><identifier>
     */
    public static Node[] ParseArguments(ref Span<IToken> tokens)
    {
        if (CheckKeyword(tokens, "void"))
            return [];

        var shifted = tokens;
        while (!shifted.IsEmpty)
        {
            if (!CheckType(shifted, TokenType.Keyword) || !CheckType(shifted, TokenType.Identifier, 1))
                break;

            Shift(shifted, out shifted, 2);
        }

        if (tokens.Length == shifted.Length)
            return [];
        
        var arguments = tokens[..shifted.Length];
        while(!arguments.IsEmpty)
        {
            
        }

        return [];
    }
   
    /*
     * <function> ::= "int" <identifier> "(" "void" ")" "{" <statement> "}"
     */
    public static FunctionNode? Parse(ref Span<IToken> tokens)
    {
        if (!CheckType(tokens, TokenType.Keyword))
            return null;
        
        if (!Shift(tokens, out var shifted))
            return null;
        
        if (!CheckType(shifted, TokenType.Identifier))
            return null;
        
        if (!Shift(shifted, out shifted))
            return null;
        
        if (!CheckType(shifted, TokenType.OpenParenthesis))
            return null;
        
        if (!Shift(shifted, out shifted))
            return null;
        
        // Parse arguments.

        return null;
    }
    
    [Pure]
    private static bool CheckKeyword(in Span<IToken> tokens, ReadOnlySpan<char> keyword) => 
        !tokens.IsEmpty && 
        tokens[0] is IdentifierToken token && 
        token.Keyword.AsSpan().SequenceEqual(keyword);
    
    [Pure]
    private static bool CheckType(in Span<IToken> tokens, in TokenType tokenType, int index = 0) 
        => index > -1 && tokens.Length > index && tokens[index].Type == tokenType;

    private static bool Shift(Span<IToken> tokens, out Span<IToken> shifted, int amount = 1)
    {
        if (tokens.Length < amount)
        {
            shifted = [];
            return false;
        }
        
        shifted = tokens[amount..];
        return true;
    }
}