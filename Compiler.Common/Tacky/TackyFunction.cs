namespace Compiler.Common.Tacky;

public record TackyFunction
(
    string Name, 
    List<TackyInstruction> Instructions
) : TackyBase;