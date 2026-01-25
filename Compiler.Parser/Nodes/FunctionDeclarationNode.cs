namespace Compiler.Parser.Nodes;

public sealed record FunctionDeclarationNode
(
    string Name, 
    string ReturnType, 
    List<string> Parameters,
    BlockNode? Body,
    StorageClass StorageClass = StorageClass.None
) : IDeclarationNode
{
    public AstNodeTag Tag => AstNodeTag.FunctionDeclaration;
}