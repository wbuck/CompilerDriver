namespace Compiler.Common.Tacky;

public record TackyReturn(TackyBase? ValueOrInstruction) : TackyInstruction;