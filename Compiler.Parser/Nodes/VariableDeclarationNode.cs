namespace Compiler.Parser.Nodes;

public sealed record VariableDeclarationNode
(
    string Identifier, 
    IExpressionNode? Initializer = null,
    StorageClass StorageClass = StorageClass.None
) : IDeclarationNode, IForLoopInitializer
{
    public AstNodeTag Tag => AstNodeTag.VariableDeclaration;
}