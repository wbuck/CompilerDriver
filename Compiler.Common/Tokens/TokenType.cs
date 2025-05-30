using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Compiler.Common.Tokens;

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
    Unknown
}