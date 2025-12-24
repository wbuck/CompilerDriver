using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Compiler.Generation.Instructions;
using Compiler.Common.Extensions;
using Compiler.Generation.Registers;

namespace Compiler.Emission;

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
        program.Functions.ForEach(f => Emit(f, builder));
        return builder.ToString();
    }

    private static void Emit(Function function, StringBuilder builder)
    {
        builder.AppendLine($".globl _{function.Name}", 2)
               .AppendLine($"_{function.Name}:")
               .AppendLine("pushq %rbp", 2)
               .AppendLine("movq %rsp, %rbp", 2);
        
        Emit(function.Instructions, builder, 2);
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
                case DeallocateStack stack:
                    builder.AppendLine($"addq ${stack.Offset}, %rsp", indent);
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
                case Bitwise { Operator: BitwiseLeftShift or BitwiseRightShift, Source: Cx } bitwise:
                    builder.AppendLine($"{GetBitwiseOperator(bitwise.Operator)} {GetOperand(bitwise.Source, ByteSize.One)}, {GetOperand(bitwise.Destination)}", indent);
                    break;
                case Bitwise bitwise:
                    builder.AppendLine($"{GetBitwiseOperator(bitwise.Operator)} {GetOperand(bitwise.Source)}, {GetOperand(bitwise.Destination)}", indent);
                    break;
                case Cmp cmp:
                    builder.AppendLine($"cmpl {GetOperand(cmp.Source)}, {GetOperand(cmp.Destination)}", indent);
                    break;
                case Jmp jmp:
                    builder.AppendLine($"jmp {jmp.Target}", indent);
                    break;
                case JmpConditional jmp:
                    builder.AppendLine($"j{GetCode(jmp.Code)} {jmp.Target}", indent);
                    break;
                case SetConditional set:
                    builder.AppendLine($"set{GetCode(set.Code)} {GetOperand(set.Operand, ByteSize.One)}", indent);
                    break;
                case Push push:
                    builder.AppendLine($"pushq {GetOperand(push.Operand, ByteSize.Eight)}", indent);
                    break;
                case Call call:
                    var instruction = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        ? $"call {call.Identifier}@PLT"
                        : $"call _{call.Identifier}";
                    builder.AppendLine(instruction, indent);
                    break;
                case Label label:
                    builder.AppendLine($"{label.Identifier}:");
                    break;
                case Ret:
                    builder.AppendLine("movq %rbp, %rsp", 2)
                        .AppendLine("popq %rbp", 2)
                        .AppendLine("ret");
                    break;                
                default:
                    throw new FormatException($"Unexpected instruction: {i.Tag.ToStringFast()}"); 
            }   
        });

    private static string GetCode(IConditionCode code) => 
        code.Tag.ToStringFast(true);

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

    private static string GetOperand(IOperand operand, ByteSize size = ByteSize.Four) => operand switch
    {
        Ax when size is ByteSize.Eight => "%rax",
        Ax when size is ByteSize.Four => "%eax",
        Ax when size is ByteSize.One => "%al",
        Dx when size is ByteSize.Eight => "%rdx",
        Dx when size is ByteSize.Four => "%edx",
        Dx when size is ByteSize.One => "%dl",
        Cx when size is ByteSize.Eight => "%rcx",
        Cx when size is ByteSize.Four => "%ecx",
        Cx when size is ByteSize.One => "%cl",
        Di when size is ByteSize.Eight => "%rdi",
        Di when size is ByteSize.Four => "%edi",
        Di when size is ByteSize.One => "%dil",
        Si when size is ByteSize.Eight => "%rsi",
        Si when size is ByteSize.Four => "%esi",
        Si when size is ByteSize.One => "%sil",
        R8 when size is ByteSize.Eight => "%r8",
        R8 when size is ByteSize.Four => "%r8d",
        R8 when size is ByteSize.One => "%r8b",
        R9 when size is ByteSize.Eight => "%r9",
        R9 when size is ByteSize.Four => "%r9d",
        R9 when size is ByteSize.One => "%r9b",
        R10 when size is ByteSize.Eight => "%r10",
        R10 when size is ByteSize.Four => "%r10d",
        R10 when size is ByteSize.One => "%r10b",
        R11 when size is ByteSize.Eight => "%r11",
        R11 when size is ByteSize.Four => "%r11d",
        R11 when size is ByteSize.One => "%r11b",
        Stack stack => $"{stack.Offset}(%rbp)",
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