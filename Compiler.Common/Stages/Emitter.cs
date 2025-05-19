using System.Diagnostics.CodeAnalysis;
using System.Text;
using Compiler.Common.Extensions;
using Compiler.Common.Generation;

namespace Compiler.Common.Stages;

public static class Emitter
{
    public static bool TryEmit(Program program, [NotNullWhen(true)] out string? compiled)
    {
        compiled = null;
        try
        {
            compiled = Emit(program);
            return true;
        }
        catch (FormatException ex)
        {
            PrintError(ex.Message);
            return false;
        }
    }
    
    public static string Emit(Program program)
    {
        StringBuilder builder = new();
        Emit(program.Function, builder);
        return builder.ToString();
    }

    private static void AssertType<TExpected>(IInstruction instruction) where TExpected : IInstruction
    {
        if (instruction is not TExpected)
            throw new FormatException($"Unexpected instruction: {instruction.Tag.ToStringFast()}");
    }

    private static void Emit(Function function, StringBuilder builder)
    {
        builder.AppendLine($".globl {function.Name}", 2)
               .AppendLine($"{function.Name}:")
               .AppendLine("pushq %rbp", 2)
               .AppendLine("movq %rsp, %rbp", 2);
        
        Emit(function.Instructions, builder, 2);
        
        builder.AppendLine("movq %rbp, %rsp", 2)
               .AppendLine("popq %rbp", 2);        

        AssertType<Ret>(function.Instructions.Last());
        builder.AppendLine("ret", 2);
    }

    private static void Emit(in List<IInstruction> instructions, StringBuilder builder, int indent)
        => instructions.ForEach(i =>
        {
            switch (i)
            {
                case Mov mov:
                    builder.AppendLine($"movl {GetOperand(mov.Source)}, {GetOperand(mov.Destination)}", indent);
                    break;
                case Unary unary:
                    builder.AppendLine($"{GetUnaryOperator(unary.Operator)} {GetOperand(unary.Operand)}", indent);
                    break;
                case AllocateStack stack:
                    builder.AppendLine($"subq ${stack.Offset}, %rsp", indent);
                    break;
                case Binary binary:
                    builder.AppendLine($"{GetBinaryOperator(binary.Operator)} {GetOperand(binary.Source)}, {GetOperand(binary.Destination)}", indent);
                    break;
                case Div div:
                    builder.AppendLine($"idivl {GetOperand(div.Operand)}", indent);
                    break;
                case Cdq:
                    builder.AppendLine("cdq", indent);
                    break;
                case Bitwise bitwise:
                    builder.AppendLine($"{GetBitwiseOperator(bitwise.Operator)} {GetOperand(bitwise.Source)}, {GetOperand(bitwise.Destination)}", indent);
                    break;
                case Ret:
                    break;
                default:
                    throw new FormatException($"Unexpected instruction: {i.Tag.ToStringFast()}"); 
            }   
        });

    private static string GetUnaryOperator(IUnaryOperator op) => op switch
    {
        Neg => "negl",
        Not => "notl",
        _ => throw new FormatException($"Unexpected unary operator: {op.Tag.ToStringFast()}")
    };

    private static string GetBitwiseOperator(IBitwiseOperator op) => op switch
    {
        BitwiseAnd => "andl",
        BitwiseOr => "orl",
        BitwiseXor => "xorl",
        BitwiseLeftShift => "sall",
        BitwiseRightShift => "sarl",
        _ => throw new FormatException($"Unexpected bitwise operator: {op.Tag.ToStringFast()}")
    };

    private static string GetBinaryOperator(IBinaryOperator op) => op switch
    {
        Add => "addl",
        Sub => "subl",
        Mult => "imull",
        _ => throw new FormatException($"Unexpected binary operator: {op.Tag.ToStringFast()}")  
    };

    private static string GetOperand(IOperand operand) => operand switch
    {
        Ax => "%eax",
        R10 => "%r10d",
        R11 => "%r11d",
        Dx => "%edx",
        Cx => "%ecx",
        Cl => "%cl",
        Stack stack => $"-{stack.Offset}(%rbp)",
        Imm<int> integer => $"${integer.Constant}",
        _ => throw new FormatException($"Unexpected operand: {operand.Tag.ToStringFast()}")   
    };

    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}