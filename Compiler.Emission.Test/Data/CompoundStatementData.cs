namespace Compiler.Emission.Test.Data;

public class CompoundStatementData : DataBase
{
    public CompoundStatementData()
    {
        Add
        (
            """
            int main(void) {
                int x = 5;
                goto inner;
                {
                    int x = 0;
                    inner:
                    x = 1;
                    return x;
                }
            }
            """,
            GetExpected([
                "subq $16, %rsp",
                "movl $5, -4(%rbp)",
                "jmp .inner1",
                "movl $0, -8(%rbp)",
                ".inner1:",
                "movl $1, -8(%rbp)",
                "movl -8(%rbp), %eax"
            ])
        );
    }
    
}