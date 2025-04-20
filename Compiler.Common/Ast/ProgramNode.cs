using System.Text;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record ProgramNode(List<INode> Nodes) : INode
{
    public NodeType NodeType => NodeType.Program;
    
    public static ProgramNode Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        List<INode> nodes = [];

        if (ParseFunction(ref tokens, fileContent) is { } node)
            nodes.Add(node);
             
        if (tokens.IsEmpty) 
            return new ProgramNode(nodes);

        var value = fileContent.Slice(tokens[0].Index, tokens[0].Length);
        throw new FormatException($"Unexpected token: {value}"); 
    }
    
    private static FunctionNode? ParseFunction(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
        => FunctionNode.Parse(ref tokens, fileContent) as FunctionNode;
}