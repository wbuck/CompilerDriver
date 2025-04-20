using Compiler.Common.Generation;

namespace Compiler.Common.Test.Data.AssemblyData;

public class AssemblyValidData : TheoryData<string, Program>
{
    public AssemblyValidData()
    {
        Add
        (
            """
            int main(void) {
                return 42;
            }
            """,
            Create("main", [
                new AllocateStack(0),
                new Mov(new Imm<int>(42), new Ax()),
                new Ret()
            ])
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
                    new AllocateStack(4),
                    new Mov(new Imm<int>(42), new Stack(4)),
                    new Unary(new Not(), new Stack(4)),
                    new Mov(new Stack(4), new Ax()),
                    new Ret()
                ])
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
                new AllocateStack(4),
                new Mov(new Imm<int>(42), new Stack(4)),
                new Unary(new Neg(), new Stack(4)),
                new Mov(new Stack(4), new Ax()),
                new Ret()
            ])
        );        
        Add
        (
            """
            int get(void) {
                return ~-42;
            }
            """,
            Create("get",
            [
                new AllocateStack(8),
                new Mov(new Imm<int>(42), new Stack(4)),
                new Unary(new Neg(), new Stack(4)),
                new Mov(new Stack(4), new R10()),
                new Mov(new R10(), new Stack(8)),
                new Unary(new Not(), new Stack(8)),
                new Mov(new Stack(8), new Ax()),
                new Ret()
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
                new AllocateStack(8),
                new Mov(new Imm<int>(42), new Stack(4)),
                new Unary(new Not(), new Stack(4)),
                new Mov(new Stack(4), new R10()),
                new Mov(new R10(), new Stack(8)),
                new Unary(new Not(), new Stack(8)),
                new Mov(new Stack(8), new Ax()),
                new Ret()
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~-(-42);
            }
            """,
            Create("main",
            [
                new AllocateStack(12),
                new Mov(new Imm<int>(42), new Stack(4)),
                new Unary(new Not(), new Stack(4)),
                new Mov(new Stack(4), new R10()),
                new Mov(new R10(), new Stack(8)),
                new Unary(new Not(), new Stack(8)),
                new Mov(new Stack(8), new R10()),
                new Mov(new R10(), new Stack(12)),
                new Unary(new Neg(), new Stack(12)),
                new Mov(new Stack(12), new Ax()),
                new Ret()
            ])
        );
    }
    
    
    
    private static Program Create(string functionName, List<IInstruction> instructions) =>
        new(new(functionName, instructions));
}