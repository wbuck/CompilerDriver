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
    Plus,
    Minus,
    Multiply,
    Divide,
    Equal,
    Not,
    LessThan,
    GreaterThan,
    Modulo
}