namespace Compiler.Parser.Nodes;

public interface IAssignmentNode : IExpressionNode
{
    IExpressionNode Lhs { get; }
    IExpressionNode Rhs { get; }
    bool IsCompound { get; }
}