namespace Compiler.Generation.Instructions;

public interface ITopLevel : IAssembly
{
    string Name { get; }
    public bool Global { get; }
}