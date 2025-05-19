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
    BitwiseComplement,
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
    Unknown
}