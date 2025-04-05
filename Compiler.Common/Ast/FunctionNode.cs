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
    
   
    /*
     * <function> ::= "int" <identifier> "(" "void" ")" "{" <statement> "}"
     */
    public static FunctionNode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckTypeAndConsume(tokens, TokenType.Keyword, out var shifted))
            return null;
        
        if (!CheckTypeAndConsume(shifted, TokenType.Identifier, out shifted))
            return null;
        
        if (!CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
            return null;
        
        var arguments = ArgumentNode.Parse(ref shifted, fileContent);
        
        if (!CheckTypeAndConsume(shifted, TokenType.CloseParenthesis, out shifted))
            return null;
        
        if (!CheckTypeAndConsume(shifted, TokenType.OpenBrace, out shifted))
            return null;

        // TODO: parse statement
        
        return null;
    }
}