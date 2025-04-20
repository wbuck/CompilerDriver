namespace Compiler.Common.Tacky;

public record TackyUnary
(
    TackyUnaryOperator Operator, 
    TackyValue Source, 
    TackyValue Destination
) : TackyInstruction;