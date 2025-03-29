using System.Diagnostics;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public class ArgumentListNode: List<ArgumentNode>, INode
{
    public ArgumentListNode(IEnumerable<ArgumentNode> arguments)
        : base(arguments)
    { }
    
    public ArgumentListNode(int capacity)
        : base(capacity)
    { }
    
    public ArgumentListNode()
    { }
    
    public NodeType NodeType => NodeType.ArgumentList;

    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        ArgumentListNode? arguments = null;        
        while (!tokens.IsEmpty)
        {
            if (ArgumentNode.Parse(ref tokens, fileContent) is not ArgumentNode argument)
                break;
            
            arguments ??= [];
            arguments.Add(argument);

            if (!INode.CheckTypeAndConsume(tokens, TokenType.Comma, out tokens))
                break;
        }        
        return arguments;
    }
}