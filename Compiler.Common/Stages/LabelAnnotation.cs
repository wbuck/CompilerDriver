using Compiler.Common.Ast;

namespace Compiler.Common.Stages;

public static class LabelAnnotation
{
    public static ProgramNode Annotate(ProgramNode node)
    {
        node = LoopLabelAnnotation.Annotate(node);
        node = SwitchLabelAnnotation.Annotate(node);
        return node;
    }
}