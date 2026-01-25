using System.ComponentModel.DataAnnotations;
using NetEscapades.EnumGenerators;

namespace Compiler.Parser.Nodes;

[EnumExtensions]
public enum StorageClass
{
    None,
    [Display( Name = "static")]
    Static,
    [Display( Name = "extern")]
    Extern
}