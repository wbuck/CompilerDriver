namespace Compiler.Common.Ast;

public enum NodeType
{
    Program, 
    Function, 
    Return, 
    IntegerConstant,
    FloatConstant,
    Argument,
    ArgumentList,
    BlockStatement
}