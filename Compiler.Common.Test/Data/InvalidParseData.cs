namespace Compiler.Common.Test.Data;

public class InvalidParseData : TheoryData<string, string>
{
    public InvalidParseData()
    {
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
            "Unexpected token: }");
        Add("""
            int main(void) {
                return int;
            }
            """, 
            "Unexpected token: int");
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
            "Unexpected token: {");
        
    }
}