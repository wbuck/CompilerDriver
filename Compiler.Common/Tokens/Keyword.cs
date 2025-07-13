using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Compiler.Common.Tokens;

[EnumExtensions]
public enum Keyword
{
    [Display(Name = "auto")]
    Auto,
    [Display(Name = "break")]
    Break,
    [Display(Name = "case")]
    Case,
    [Display(Name = "char")]
    Char,
    [Display(Name = "const")]
    Const,
    [Display(Name = "continue")]
    Continue,
    [Display(Name = "default")]
    Default,
    [Display(Name = "do")]
    Do,
    [Display(Name = "double")]
    Double,
    [Display(Name = "else")]
    Else,
    [Display(Name = "enum")]
    Enum,
    [Display(Name = "extern")]
    Extern,
    [Display(Name = "float")]
    Float,
    [Display(Name = "for")]
    For,
    [Display(Name = "goto")]
    Goto,
    [Display(Name = "if")]
    If,
    [Display(Name = "int")]
    Int,
    [Display(Name = "long")]
    Long,
    [Display(Name = "register")]
    Register,
    [Display(Name = "return")]
    Return,
    [Display(Name = "short")]
    Short,
    [Display(Name = "signed")]
    Signed,
    [Display(Name = "sizeof")]
    Sizeof,
    [Display(Name = "static")]
    Static,
    [Display(Name = "struct")]
    Struct,
    [Display(Name = "switch")]
    Switch,
    [Display(Name = "typedef")]
    Typedef,
    [Display(Name = "union")]
    Union,
    [Display(Name = "unsigned")]
    Unsigned,
    [Display(Name = "void")]
    Void,
    [Display(Name = "volatile")]
    Volatile,
    [Display(Name = "while")]
    While
}