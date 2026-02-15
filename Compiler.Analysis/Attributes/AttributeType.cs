using NetEscapades.EnumGenerators;

namespace Compiler.Analysis.Attributes;

[EnumExtensions]
public enum AttributeType
{
    Function,
    Static,
    Local
}