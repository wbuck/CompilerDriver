using NetEscapades.EnumGenerators;

namespace Compiler.Tacky.Tac;

[EnumExtensions]
public enum TackyTag
{
    Return,
    Unary,
    Binary,
    Constant,
    Variable,
    Complement,
    Negate,
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Remainder,
    Function,
    Program,
    Bitwise,
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor,
    LeftShift,
    RightShift,
    Not,
    LogicalAnd,
    LogicalOr,
    Equal,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    Copy,
    Jump,
    JumpIfZero,
    JumpIfNotZero,
    Label,
    FunctionCall
}