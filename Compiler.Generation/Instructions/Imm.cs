using System.Numerics;

namespace Compiler.Generation.Instructions;

public sealed record Imm<T>(T Constant) : IConstant where T: INumber<T>
{
    public AssemblyTag Tag => AssemblyTag.Imm;
}