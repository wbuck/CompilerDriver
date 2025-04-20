using System.Numerics;
using Compiler.Common.Tacky;

namespace Compiler.Common.Generation;


public interface IOperand;
public interface IReg : IOperand;
public record Ax : IReg;
public record R10 : IReg;

public record Imm<T>(T Constant) : IOperand where T: INumber<T>;
public record Pseudo(string Identifier, int StackOffset): IOperand;
public record Stack(int Offset) : IOperand;

public interface IUnaryOperator;
public record Neg : IUnaryOperator;
public record Not : IUnaryOperator;

public interface IInstruction;
public record Mov(IOperand Source, IOperand Destination) : IInstruction;
public record Unary(IUnaryOperator Operator, IOperand Operand) : IInstruction;
public record AllocateStack(int Offset) : IInstruction;
public record Ret : IInstruction;

public record Function(string Name, List<IInstruction> Instructions);

public record Program(Function Function)
{
    public static Program Visit(TackyProgram tacky)
    {
        PseudoReplacer replacer = new();
        
        var program = replacer.Replace(new(Visit(tacky.Function)));
        program.Function.Instructions.Insert(0, new AllocateStack(replacer.StackOffset));
        
        return StackReplacer.Replace(program);
    }

    private static Function Visit(TackyFunction function)
        => new(function.Name, Visit(function.Instructions));

    private static List<IInstruction> Visit(IEnumerable<TackyInstruction> instructions)
        => instructions.SelectMany(i => i switch
        {
            TackyUnary unary => Visit(unary),
            TackyReturn ret => Visit(ret),
            _ => throw new FormatException($"Unknown instruction type {i.GetType().Name}")
        }).ToList();

    private static IEnumerable<IInstruction> Visit(TackyUnary unary)
    {
        var dest = Visit(unary.Destination);
        return [
            new Mov(Visit(unary.Source), dest),
            new Unary(Visit(unary.Operator), dest)
        ];
    }

    private static IOperand Visit(TackyValue value)
        => value switch
        {
            TackyIntegerConstant integer => new Imm<int>(integer.Value),
            TackyFloatConstant floating => new Imm<double>(floating.Value),
            TackyVariable variable => new Pseudo(variable.Name, variable.StackOffset),
            _ => throw new FormatException($"Unknown operand type {value.GetType().Name}")
        };
    
    private static List<IInstruction> Visit(TackyReturn ret)
    {
        if (ret.ValueOrInstruction is null)
            return [new Ret()];
        
        List<IInstruction> instructions = [];
        switch (ret.ValueOrInstruction)
        {
            case TackyUnary unary:
                instructions.AddRange(Visit(unary));
                instructions.Add(new Mov(instructions.OfType<Unary>().Last().Operand, new Ax()));
                break;
            case TackyValue value:
                instructions.Add(new Mov(Visit(value), new Ax()));
                break;
        }        
        instructions.Add(new Ret());  
        return instructions;
    }

    private static IUnaryOperator Visit(TackyUnaryOperator unary)
        => unary switch
        {
            TackyNegation => new Neg(),
            TackyBitwiseComplement => new Not(),
            _ => throw new FormatException($"Unknown unary operator type {unary.GetType().Name}")
        };
   

    
}