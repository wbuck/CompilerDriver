namespace Compiler.Parser.Nodes;

public sealed record VariableNode(string Identifier) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Variable;
}