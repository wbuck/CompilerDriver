using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Annotation;

public static class LabelAnnotation
{
    public static ProgramNode Annotate(ProgramNode node)
    {
        node = LoopLabelAnnotation.Annotate(node);
        node = SwitchLabelAnnotation.Annotate(node);
        return node;
    }
}