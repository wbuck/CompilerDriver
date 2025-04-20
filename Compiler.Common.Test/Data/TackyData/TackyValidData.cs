using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.TackyData;

public class TackyValidData : TheoryData<string, TackyProgram>
{
    public TackyValidData()
    {
        Add
        (
            """
            int main(void) {
                return 42;
            }
            """,
            Create("main", [new TackyReturn(new TackyIntegerConstant(42))])
        );
        Add
        (
            """
            int main(void) {
                return -42;
            }
            """,
            Create("main", 
                [
                    new TackyUnary(new TackyNegation(), new TackyIntegerConstant(42), new TackyVariable(1)),
                    new TackyReturn(new TackyVariable(1)),
                ])
        );
        Add
        (
            """
            int main(void) {
                return ~42;
            }
            """,
            Create("main", 
            [
                new TackyUnary(new TackyBitwiseComplement(), new TackyIntegerConstant(42), new TackyVariable(1)),
                new TackyReturn(new TackyVariable(1)),
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~-42;
            }
            """,
            Create("main", 
            [
                new TackyUnary(new TackyNegation(), new TackyIntegerConstant(42), new TackyVariable(1)),
                new TackyUnary(new TackyBitwiseComplement(), new TackyVariable(1), new TackyVariable(2)),
                new TackyReturn(new TackyVariable(2)),
            ])
        );
        Add
        (
            """
            int main(void) {
                return -(-42);
            }
            """,
            Create("main", 
            [
                new TackyUnary(new TackyNegation(), new TackyIntegerConstant(42), new TackyVariable(1)),
                new TackyUnary(new TackyNegation(), new TackyVariable(1), new TackyVariable(2)),
                new TackyReturn(new TackyVariable(2)),
            ])
        );
    }
    
    private static TackyProgram Create(string functionName, List<TackyInstruction> instructions) =>
        new(new(functionName, instructions));
}