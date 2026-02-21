using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Compiler.Generation.Instructions;

[EnumExtensions]
public enum AssemblyTag
{
    Mov,
    Add,
    Sub,
    Mult,
    Div,
    Neg,
    Not,
    Cdq,
    Ax,
    R8,
    R9,
    R10,
    R11,
    Dx,
    Cx,
    Di,
    Si,
    Imm,
    Pseudo,
    Stack,
    Ret,
    AllocateStack,
    Binary,
    Unary,
    Bitwise,
    And,
    Or,
    Xor,
    LeftShift,
    RightShift,
    Program,
    Function,
    [Display(Name = "e")]
    Equal,
    [Display(Name = "ne")]
    NotEqual,
    [Display(Name = "l")]
    LessThan,
    [Display(Name = "le")]
    LessThanOrEqual,
    [Display(Name = "g")]
    GreaterThan,
    [Display(Name = "ge")]
    GreaterThanOrEqual,
    Cmp,
    Jmp,
    JmpConditional,
    SetConditional,
    Label,
    DeallocateStack,
    Push,
    Call,
    Data,
    StaticVariable
}