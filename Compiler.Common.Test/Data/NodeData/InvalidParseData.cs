
namespace Compiler.Common.Test.Data.NodeData;

public class InvalidParseData : TheoryData<string, string>
{
    public InvalidParseData()
    {
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
            "Missing ';'");        
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
            "Unexpected token: int");
        Add("""
            int main(void) {
                RETURN 0;
            }
            """, 
            "Unexpected token: RETURN");
        Add("""
            main(void) {
                return 0;
            }
            """, 
            "Unexpected token: main");
        Add("""
            int main(void) {
                returns 0;
            }
            """, 
            "Unexpected token: returns");
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
            "Expected ';' but found 'int'");
        Add("""
            int main(void){
                retur n 0;
            }
            """, 
            "Unexpected token: retur");
        Add("""
            int main )( {
                return 0;
            }
            """, 
            "Unexpected token: int");
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
            "Expected ')' but found '{'");
        
    }
}