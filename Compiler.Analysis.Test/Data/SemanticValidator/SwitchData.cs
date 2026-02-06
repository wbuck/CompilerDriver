using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Test.Data.SemanticValidator;

public class SwitchData : DataBase
{
    public SwitchData()
    {        
        Add
        (
            """
            int main(void) {
                switch (1) {
                    case 1: break;
                    case -1: break;
                    case !1: break;
                    case ~1: break;
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(1),
                    Compound
                         (
                             new CaseNode(Const(1), new BreakNode()),
                             new CaseNode(Const(-1), new BreakNode()),
                             new CaseNode(Const(0), new BreakNode()),
                             new CaseNode(Const(-2), new BreakNode())
                         )
                )
            )
        );
    }
}