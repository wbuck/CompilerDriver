using System.Diagnostics;
using System.Runtime.InteropServices;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record ArgumentNode(ReadOnlyMemory<char> Name, string Type) : INode
{
    public NodeType NodeType => NodeType.Argument;
    
    /*
     * <argument_list> :: = <argument>* | "void"
     * <argument> :: = <keyword><identifier>
     */
    public static List<ArgumentNode> Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (INode.CheckKeyword(tokens, "void"))
            return [];

        var shifted = tokens;
        while (!shifted.IsEmpty)
        {
            if (!INode.CheckType(shifted, TokenType.Keyword) || !INode.CheckType(shifted, TokenType.Identifier, 1))
                break;
            
            // TODO: Make sure the keyword is a valid type for a function argument.

            var shift = INode.CheckType(shifted, TokenType.Comma, 2) ? 3 : 2;
            INode.Shift(shifted, out shifted, shift);
        }

        if (tokens.Length == shifted.Length)
            return [];
        
        var count = tokens.Length - shifted.Length;
        var arguments = tokens[..count];      
        var nodes = new List<ArgumentNode>((arguments.Length - arguments.Length % 2) / 2);
        
        while(!arguments.IsEmpty)
        {
            var id = (KeywordToken)arguments[0];
            Debug.Assert(id.Keyword is not null);
            
            id = (KeywordToken)arguments[1];
            var name = fileContent.Slice(id.Index, id.Length);
            
            nodes.Add(new ArgumentNode(name, id.Keyword!));
            var shift = INode.CheckType(arguments, TokenType.Comma, 2) ? 3 : 2;
            
            INode.Shift(arguments, out arguments, shift);
        }
        
        tokens = shifted;
        return nodes;
    }
}