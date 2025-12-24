namespace Compiler.Parser.Nodes;

public sealed record ReturnNode(IExpressionNode Expression) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Return;
}