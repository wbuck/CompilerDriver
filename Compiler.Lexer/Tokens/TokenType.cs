using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Compiler.Lexer.Tokens;

[EnumExtensions]
public enum TokenType
{
    Keyword,
    Identifier,
    [Display(Name = "(")]
    OpenParenthesis,
    [Display(Name = ")")]
    CloseParenthesis,
    [Display(Name = "{")]
    OpenBrace,
    [Display(Name = "}")]
    CloseBrace,
    [Display(Name = ",")]
    Comma,
    [Display(Name = ";")]
    Semicolon,
    NumericConstant,
    [Display(Name = "~")]
    Complement,
    [Display(Name = "--")]
    Decrement,
    [Display(Name = "++")]
    Increment,
    [Display(Name = "+")]
    Plus,
    [Display(Name = "-")]
    Minus,
    [Display(Name = "*")]   
    Asterisk,
    [Display(Name = "/")]  
    ForwardSlash,
    [Display(Name = "%")]
    Percent,
    [Display(Name = "&")]
    BitwiseAnd,
    [Display(Name = "|")]
    BitwiseOr,
    [Display(Name = "^")]
    BitwiseXor,
    [Display(Name = "<<")]
    LeftShift,
    [Display(Name = ">>")]
    RightShift,
    [Display(Name = "!")]
    Not,
    [Display(Name = "&&")]
    LogicalAnd,
    [Display(Name = "||")]
    LogicalOr,
    [Display(Name = "==")]
    Equal,
    [Display(Name = "!=")]
    NotEqual, 
    [Display(Name = "<")]
    LessThan,
    [Display(Name = "<=")]
    LessThanOrEqual,
    [Display(Name = ">")]
    GreaterThan,
    [Display(Name = ">=")]
    GreaterThanOrEqual,
    [Display(Name = "=")]
    Assignment,
    [Display(Name = "+=")]
    AdditionAssignment,
    [Display(Name = "-=")]
    SubtractionAssignment,
    [Display(Name = "*=")]
    MultiplicationAssignment,
    [Display(Name = "/=")]
    DivisionAssignment,
    [Display(Name = "%=")]
    RemainderAssignment,
    [Display(Name = "&=")]
    BitwiseAndAssignment,
    [Display(Name = "|=")]
    BitwiseOrAssignment,
    [Display(Name = "^=")]
    BitwiseXorAssignment,
    [Display(Name = "<<=")]
    LeftShiftAssignment,
    [Display(Name = ">>=")]
    RightShiftAssignment,
    [Display(Name = "?")]
    QuestionMark,
    [Display(Name = ":")]
    Colon,   
    Unknown
}