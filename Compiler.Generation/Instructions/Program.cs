using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Compiler.Generation.Helpers;
using Compiler.Generation.Registers;
using Compiler.Tacky.Tac;

namespace Compiler.Generation.Instructions;

public record Program(List<Function> Functions): IAssembly
{
    private static readonly ImmutableArray<IReg> ArgumentRegisters =
    [
        Di.Register,
        Si.Register,
        Dx.Register,
        Cx.Register,
        R8.Register,
        R9.Register
    ];
    
    public static Program Visit(TackyProgram tacky)
    {
        var functions = tacky.Functions.Aggregate(new List<Function>(tacky.Functions.Count), (acc, func) =>
        {
            var funcAssembly = VisitFunction(func);
            
            PseudoReplacer replacer = new();            
            funcAssembly = replacer.Replace(funcAssembly);
            
            // Round the stack offset to the nearest multiple of 16 bytes
            // to make it easier during function calls.
            var stackSize = RoundToMultipleOf16(replacer.StackOffset);
            funcAssembly.Instructions.Insert(0, new AllocateStack(stackSize));
            acc.Add(funcAssembly);
            return acc;
        });
        return InvalidInstructionReplacer.Replace(new Program(functions));
    }

    private static int RoundToMultipleOf16(int value) => (value + 15) & ~0xF;
    
    private static Function VisitFunction(TackyFunction function)
    {
        var parameters = CollectionsMarshal.AsSpan(function.Parameters);
        
        var registerArgs = parameters[..Math.Min(parameters.Length, ArgumentRegisters.Length)];
        var stackArgs = parameters.Length > ArgumentRegisters.Length 
            ? parameters[ArgumentRegisters.Length..]
            : new Span<string>();

        List<IInstruction> moves = new(registerArgs.Length + stackArgs.Length);
        
        for (var i = 0; i < registerArgs.Length; ++i)        
            moves.Add(new Mov(ArgumentRegisters[i], new Pseudo(registerArgs[i])));
        
        for (var offset = 16; !stackArgs.IsEmpty; offset += 8)
        {
            moves.Add(new Mov(new Stack(offset), new Pseudo(stackArgs[0])));
            stackArgs = stackArgs[1..];
        }         
                   
        return new Function
        (
            function.Name, 
            moves.Concat(VisitInstructions(function.Instructions)).ToList()
        );
    }
    
    private static List<IInstruction> VisitInstructions(IEnumerable<ITackyInstruction> instructions)
        => instructions.SelectMany(i => i switch
        {
            TackyUnary unary => VisitUnary(unary),
            TackyReturn ret => VisitReturn(ret),
            TackyBinary binary => VisitBinary(binary),
            TackyBitwise bitwise => VisitBitwise(bitwise),
            TackyJump jump => VisitJump(jump),
            TackyJumpIfZero jumpIfZero => VisitJumpIfZero(jumpIfZero),
            TackyJumpIfNotZero jumpIfNotZero => VisitJumpIfNotZero(jumpIfNotZero),
            TackyCopy copy => VisitCopy(copy),
            TackyLabel label => VisitLabel(label),
            TackyFunctionCall call => VisitFuncCall(call),
            _ => throw new FormatException($"Unknown instruction type {i.Tag.ToStringFast()}")
        }).ToList();

    private static List<IInstruction> VisitFuncCall(TackyFunctionCall call)
    {
        List<IInstruction> instructions = [];
        var args = CollectionsMarshal.AsSpan(call.Arguments);
        
        var registerArgs = args[..Math.Min(args.Length, ArgumentRegisters.Length)];
        var stackArgs = args.Length > ArgumentRegisters.Length 
            ? args[ArgumentRegisters.Length..]
            : new Span<ITackyValue>();
        
        // If the number of stack arguments is odd, it means it's not
        // 16-byte aligned. Add 8 bytes to make it 16-byte aligned.        
        var padding = stackArgs.Length % 2 == 1 ? 8 : 0;
        if (padding > 0)
            instructions.Add(new AllocateStack(padding));

        for (var i = 0; i < registerArgs.Length; ++i)
            instructions.Add(new Mov(VisitValue(registerArgs[i]), ArgumentRegisters[i]));

        for (var i = stackArgs.Length - 1; i >= 0; --i)
        {
            var arg = VisitValue(stackArgs[i]);
            if (arg is IReg or IConstant)
            {
                instructions.Add(new Push(arg));
                continue;
            }
            instructions.Add(new Mov(arg, Ax.Register));
            instructions.Add(new Push(Ax.Register));                
        }
        
        instructions.Add(new Call(call.Identifier));
        
        var bytesToRemove = 8 * stackArgs.Length + padding;
        if (bytesToRemove > 0)
            instructions.Add(new DeallocateStack(bytesToRemove));
        
        var dest = VisitValue(call.Destination);
        instructions.Add(new Mov(Ax.Register, dest));
        return instructions;       
    }
    
    private static List<IInstruction> VisitLabel(TackyLabel label)
        => [new Label(label.Identifier)];
    
    private static List<IInstruction> VisitCopy(TackyCopy copy) 
        => [new Mov(VisitValue(copy.Source), VisitValue(copy.Destination))];
    
    private static List<IInstruction> VisitJumpIfNotZero(TackyJumpIfNotZero jump)
        => [
            new Cmp(IOperand.Zero, VisitValue(jump.Condition)),
            new JmpConditional(NotEqual.Code, jump.Target)
        ];

    private static List<IInstruction> VisitJumpIfZero(TackyJumpIfZero jump)
        => [
            new Cmp(IOperand.Zero, VisitValue(jump.Condition)),
            new JmpConditional(Equal.Code, jump.Target)
        ];
    
    private static List<IInstruction> VisitJump(TackyJump jump) 
        => [new Jmp(jump.Target)];

    private static IEnumerable<IInstruction> VisitBitwise(TackyBitwise bitwise)
    {
        var dest = VisitValue(bitwise.Destination);
        return [
            new Mov(VisitValue(bitwise.Lhs), dest),
            new Bitwise(GetBitwiseOperator(bitwise.Operator), VisitValue(bitwise.Rhs), dest)
        ];
    }
    private static IEnumerable<IInstruction> VisitBinary(TackyBinary binary)
    {
        return binary switch
        {
            { Operator: TackyDivision or TackyRemainder } => GetDivOrRemainderInstructions(binary),
            _ when IsRelational(binary.Operator) => GetRelationalInstructions(binary), 
            _ => GetArithmeticInstructions(binary)
        };

        static IEnumerable<IInstruction> GetRelationalInstructions(TackyBinary binary)
        {
            var dest = VisitValue(binary.Destination);
            return [
                new Cmp(VisitValue(binary.Rhs), VisitValue(binary.Lhs)),
                new Mov(IOperand.Zero, dest),
                new SetConditional(GetConditionCode(binary.Operator), dest),
            ];
        }
        
        static IEnumerable<IInstruction> GetDivOrRemainderInstructions(TackyBinary binary) =>
        [
            new Mov(VisitValue(binary.Lhs), Ax.Register),
            Cdq.Instruction,
            new Div(VisitValue(binary.Rhs)),
            new Mov(GetDivResultRegister(binary.Operator), VisitValue(binary.Destination))
        ];

        static IEnumerable<IInstruction> GetArithmeticInstructions(TackyBinary binary)
        {
            var dest = VisitValue(binary.Destination);
            return [
                new Mov(VisitValue(binary.Lhs), dest),
                new Binary(GetBinaryOperator(binary.Operator), VisitValue(binary.Rhs), dest)
            ];
        }
        
        static bool IsRelational(ITackyBinaryOperator binary) =>
            binary is TackyLessThan 
                or TackyLessThanOrEqual 
                or TackyGreaterThan 
                or TackyGreaterThanOrEqual 
                or TackyEqual 
                or TackyNotEqual;
        
        static IReg GetDivResultRegister(ITackyBinaryOperator binary) =>
            binary is TackyDivision 
                ? Ax.Register 
                : Dx.Register;
    }

    private static IConditionCode GetConditionCode(ITackyBinaryOperator op)
        => op switch
        {
            TackyEqual => Equal.Code,
            TackyNotEqual => NotEqual.Code,
            TackyLessThan => LessThan.Code,
            TackyLessThanOrEqual => LessThanOrEqual.Code,
            TackyGreaterThan => GreaterThan.Code,
            TackyGreaterThanOrEqual => GreaterThanOrEqual.Code,
            _ => throw new FormatException($"Unknown condition code type {op.Tag.ToStringFast()}")
        };

    private static IEnumerable<IInstruction> VisitUnary(TackyUnary unary)
    {
        return unary switch
        {
            { Operator: TackyNot } => GetNotInstructions(unary),
            _ => GetOtherInstructions(unary)
        };

        static IEnumerable<IInstruction> GetNotInstructions(TackyUnary unary)
        {
            var dest = VisitValue(unary.Destination);
            return [
                new Cmp(IOperand.Zero, VisitValue(unary.Source)),
                new Mov(IOperand.Zero, dest),
                new SetConditional(Equal.Code, dest),
            ];
        }

        static IEnumerable<IInstruction> GetOtherInstructions(TackyUnary unary)
        {
            var dest = VisitValue(unary.Destination);
            return [
                new Mov(VisitValue(unary.Source), dest),
                new Unary(GetUnaryOperator(unary.Operator), dest)
            ];  
        }            
    }

    private static IOperand VisitValue(ITackyValue value)
        => value switch
        {
            TackyConstant<int> integer => new Imm<int>(integer.Value),
            TackyConstant<double> floating => new Imm<double>(floating.Value),
            TackyVariable variable => new Pseudo(variable.Identifier),
            _ => throw new FormatException($"Unknown operand type {value.Tag.ToStringFast()}")
        };
    
    private static List<IInstruction> VisitReturn(TackyReturn @return)
        => [new Mov(VisitValue(@return.Value), Ax.Register), Ret.Instruction];

    private static IBitwiseOperator GetBitwiseOperator(ITackyBitwiseOperator bitwise)
        => bitwise switch
        {
            TackyBitwiseAnd => BitwiseAnd.Operator,
            TackyBitwiseOr => BitwiseOr.Operator,
            TackyBitwiseXor => BitwiseXor.Operator,
            TackyLeftShift => BitwiseLeftShift.Operator,
            TackyRightShift => BitwiseRightShift.Operator,
            _ => throw new FormatException($"Unknown bitwise operator type {bitwise.Tag.ToStringFast()}")
        };
    
    private static IBinaryOperator GetBinaryOperator(ITackyBinaryOperator binary)
        => binary switch
        {
            TackyAddition => Add.Operator,
            TackySubtraction => Sub.Operator,
            TackyMultiplication => Mult.Operator,            
            _ => throw new FormatException($"Unknown binary operator type {binary.Tag.ToStringFast()}")
        };
    
    private static IUnaryOperator GetUnaryOperator(ITackyUnaryOperator unary)
        => unary switch
        {
            TackyNegate => Neg.Operator,
            TackyComplement => Not.Operator,            
            _ => throw new FormatException($"Unknown unary operator type {unary.Tag.ToStringFast()}")
        };
    
    public AssemblyTag Tag => AssemblyTag.Program;
}