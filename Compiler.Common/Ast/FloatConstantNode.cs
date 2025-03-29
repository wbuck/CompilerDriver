using System.Text.RegularExpressions;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record FloatConstantNode(double Value, ReadOnlyMemory<char> Original) : INode
{    
    public NodeType NodeType => NodeType.FloatConstant;
    
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!INode.CheckType(tokens, TokenType.NumericConstant))
            return null;

        if (INode.GetToken<NumericConstantToken>(tokens) is not { } token)
            return null;
        
        var value = fileContent.Slice(token.Index, token.Length);
        
        if (!double.TryParse(value.Span, out var number)) 
            return null;
        
        INode.Shift(tokens, out tokens);
        return new FloatConstantNode(number, value);
    }
}