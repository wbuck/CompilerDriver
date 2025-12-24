namespace Compiler.Parser.Nodes;

public sealed record FunctionCallNode
(
    string Identifier, 
    List<IExpressionNode> Args
) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.FunctionCall;
}