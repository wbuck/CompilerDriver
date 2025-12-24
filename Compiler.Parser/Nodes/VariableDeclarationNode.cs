namespace Compiler.Parser.Nodes;

public sealed record VariableDeclarationNode
(
    string Identifier, 
    IExpressionNode? Initializer = null
) : IDeclarationNode, IForLoopInitializer
{
    public AstNodeTag Tag => AstNodeTag.VariableDeclaration;
}