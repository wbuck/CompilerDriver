
namespace Compiler.Common.Test.Data.NodeData;

public class InvalidParseData : TheoryData<string, string>
{
    public InvalidParseData()
    {
        Add("""
            int main(void) {
                return 2*2
            }
            """, 
            "Expected ';' but found '}'");
        Add("""
            int main(void) {
                return 1 + ;
            }
            """, 
            "Expected expression but found ';'");
        Add("""
            int main(void) {
                return 1 + 2);
            }
            """, 
            "Expected ';' but found ')'");
        Add("""
            int main(void) {
                return /3;
            }
            """, 
            "Expected expression but found '/'");
        Add("""
            int main(void) {
                return 1 + (2;)
            }
            """, 
            "Expected ')' but found ';'");
        Add("""
            int main(void) {
                return 1 + (2;)
            }
            """, 
            "Expected ')' but found ';'");
        Add("""
            int main(void) {
                return 2 (- 3);
            }
            """, 
            "Expected ';' but found '('");
        Add("""
            int main(void) {
                return 1 + (2;
            }
            """, 
            "Expected ')' but found ';'");
        Add("""
            int main(void) {
                return 1 * / 2;
            }
            """, 
            "Expected expression but found '/'");   
        Add("""
            int main(void)
            {
                return (1;
            }
            """, 
            "Expected ')' but found ';'");        
        Add("""
            int main(void) {
                return (-)3;
            }
            """, 
            "Expected expression but found ')'");        
        Add("""
            int main(void)
            {
                return -~;
            }
            """,
            "Expected expression but found ';'"); 
        Add("""
            int main(void) {
                return 4-;
            }
            """,
            "Expected expression but found ';'");        
        Add("""
            int main(void) {
                return -5
            }
            """,
            "Expected ';' but found '}'");        
        Add("""
            int main(void) {
                return ~;
            }
            """,
            "Expected expression but found ';'");        
        Add("""
            int main(void)
            {
                return (3));
            }
            """,
            "Expected ';' but found ')'");        
        Add("""
            int main(void) {
            return
            """,
            "Expected expression but found ''");        
        Add("""
            int main(void)
            {
                return 2;
            }
            foo
            """,
            "Unexpected token: foo"); 
        Add("""
            int 3 (void) {
                return 0;
            }
            """,
            "Expected function identifier but found '3'");        
        Add("""
            int main(void) {
                RETURN 0;
            }
            """,
            "Expected 'return' but found 'RETURN'");        
        Add("""
            main(void) {
                return 0;
            }
            """,
            "Expected return type but found 'main'");        
        Add("""
            int main(void) {
                returns 0;
            }
            """,
            "Expected 'return' but found 'returns'");        
        Add("""
            int main (void) {
                return 0
            }
            """,
            "Expected ';' but found '}'");        
        Add("""
            int main(void) {
                return int;
            }
            """,
            "Expected expression but found 'int'");        
        Add("""
            int main(void){
                retur n 0;
            }
            """,
            "Expected 'return' but found 'retur'");        
        Add("""
            int main )( {
                return 0;
            }
            """,
            "Expected '(' but found ')'");        
        Add("""
            int main(void) {
            return 0;
            """,
            "Missing '}'");
        Add("""
            int main( {
                return 0;
            }
            """,
            "Expected 'void' but found '{'");
    }
}