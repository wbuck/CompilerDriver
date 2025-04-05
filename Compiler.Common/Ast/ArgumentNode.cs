using System.Diagnostics;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record ArgumentNode(ReadOnlyMemory<char> Name, string Type) : Node
{
    public override NodeType NodeType => NodeType.Argument;
    
    public static ArgumentNode[] Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
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
        var nodes = new ArgumentNode[(arguments.Length - arguments.Length % 2) / 2];   
        
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
}