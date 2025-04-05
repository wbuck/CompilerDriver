using System.Diagnostics;
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
    private static Node[] ParseArguments(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (CheckKeyword(tokens, "void"))
            return [];

        var shifted = tokens;
        while (!shifted.IsEmpty)
        {
            if (!CheckType(shifted, TokenType.Keyword) || !CheckType(shifted, TokenType.Identifier, 1))
                break;
            
            // TODO: Make sure the keyword is a valid type for a function argument.

            var shift = CheckType(shifted, TokenType.Comma, 2) ? 3 : 2;
            Shift(shifted, out shifted, shift);
        }

        if (tokens.Length == shifted.Length)
            return [];
        
        var count = tokens.Length - shifted.Length;
        var arguments = tokens[..count];      
        var nodes = new Node[(arguments.Length - arguments.Length % 2) / 2];   
        
        int index = 0;
        while(!arguments.IsEmpty)
        {
            var id = (IdentifierToken)arguments[0];
            var type = id.Keyword;
            Debug.Assert(type is not null);
            
            id = (IdentifierToken)arguments[1];
            var name = fileContent.Slice(id.Index, id.Length);

            nodes[index++] = new ArgumentNode(name, type);
            var shift = CheckType(arguments, TokenType.Comma, 2) ? 3 : 2;
            
            Shift(arguments, out arguments, shift);
        }
        
        tokens = shifted;
        return nodes;
    }
   
    /*
     * <function> ::= "int" <identifier> "(" "void" ")" "{" <statement> "}"
     */
    public static FunctionNode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
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
        
        var arguments = ParseArguments(ref shifted, fileContent);
        
        if (!CheckType(shifted, TokenType.CloseParenthesis))
            return null;
        
        if (!Shift(shifted, out shifted))
            return null;

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