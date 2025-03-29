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
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {                
        if (!INode.CheckType(tokens, TokenType.Keyword) || !INode.CheckType(tokens, TokenType.Identifier, 1))
            return null;
            
        // TODO: Make sure the keyword is a valid type for a function argument.
        var keyword = INode.GetTokenAndConsume<KeywordToken>(ref tokens)!.Keyword;
        
        var identifier = INode.GetTokenAndConsume<IdentifierToken>(ref tokens)!;
        var name = fileContent.Slice(identifier.Index, identifier.Length);
        
        return new ArgumentNode(name, keyword);
    }
}