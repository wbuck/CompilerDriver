namespace Compiler.Parser.Nodes;

public readonly record struct SwitchLabel
(
    string Label, 
    IExpressionNode? Value,
    int? CalculatedValue
);