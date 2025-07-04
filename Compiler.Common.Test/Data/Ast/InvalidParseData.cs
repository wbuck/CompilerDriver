
namespace Compiler.Common.Test.Data.Ast;

public class InvalidParseData : TheoryData<string, string>
{
    public InvalidParseData()
    {
        Add("""
            int main(void) {
                int a = 0;
                if (1)
                    return 1;
                else
                    return 2;
                else
                    return 3;
            }
            """, 
            "Expected '}' but found 'else'");
        Add("""
            int main(void) {
                if 0 return 1;
            }
            """, 
            "Expected '(' but found '0'");
        Add("""
            int main(void) {
                int flag = 0;
                int a = if (flag)
                            2;
                        else
                            3;
                return a;
            }
            """, 
            "Expected ';' but found 'if'");
        Add("""
            int main(void) {
                if (0) else return 0;
            }
            """, 
            "Expected statement but found 'else'");
        Add("""
            int main(void) {
                if (5)
                    int i = 0;
            }
            """, 
            "Expected statement but found 'int'");
        Add("""
            int main(void) {
                int 10 = return 0;
            }
            """, 
            "Expected '}' but found 'int'");
        Add("""
            int main(void) {
                int a = 2
                a = a + 4;
                return a;
            }
            """, 
            "Expected ';' but found 'a'");
        Add("""
            int main(void) {
                return 1 ! = 0;
            }
            """, 
            "Expected ';' but found '!'");
        Add("""
            int main(void) {
                return 1 < = 2;
            }
            """, 
            "Expected expression but found '='");
        Add("""
            int main(void) {
                int a = 0;
                a + +;
                return a;
            }
            """, 
            "Expected expression but found '+'");
        Add("""
            int main(void) {
                int a = 0;
                a - -;
                return a;
            }
            """, 
            "Expected expression but found ';'");
        Add("""
            int main(void) {
                int a = 10;
                a =/ 1;
                return a;
            }
            """, 
            "Expected expression but found '/'");
        Add("""
            int main(void) {
                int 10 = 0;
                return 10;
            }
            """, 
            "Expected '}' but found 'int'");
        Add("""
            int main(void) {
                ints a = 1;
                return a;
            }
            """, 
            "Expected ';' but found 'a'");
        Add("""
            int main(void) {
                int foo bar = 3;
                return bar;
            }
            """, 
            "Expected ';' but found 'bar'");
        Add("""
            int main(void) {
                int return = 4;
                return return + 1;
            }
            """, 
            "Expected '}' but found 'int'");
        Add("""
            int main(void) {
                int a = 0;
                a + = 1;
                return a;
            }
            """, 
            "Expected expression but found '='");
        Add("""
            int main(void) {
                int 10 = return 0;
            }
            """, 
            "Expected '}' but found 'int'");
        Add("""
            int main(void) {
                int a = 2
                a = a + 4;
                return a;
            }
            """, 
            "Expected ';' but found 'a'");
        Add("""
            int main(void) {
                return 1 ! = 0;
            }
            """, 
            "Expected ';' but found '!'");
        Add("""
            int main(void)
            {
                return 1 < = 2;
            }
            """, 
            "Expected expression but found '='");
        Add("""
            int main(void) {
                int a = 0;
                a + +;
                return a;
            }
            """, 
            "Expected expression but found '+'");
        Add("""
            int main(void) {
                int a = 0;
                a - -;
                return a;
            }
            """, 
            "Expected expression but found ';'");
        Add("""
            int main(void) {
                int a = 10;
                a =/ 1;
                return a;
            }
            """, 
            "Expected expression but found '/'");
        Add("""
            int main(void)
            {
                int 10 = 0;
                return 10;
            }
            """, 
            "Expected '}' but found 'int'");
        Add("""
            int main(void) {
                ints a = 1;
                return a;
            }
            """, 
            "Expected ';' but found 'a'");
        Add("""
            int main(void) {
                int foo bar = 3;
                return bar;
            }
            """, 
            "Expected ';' but found 'bar'");
        Add("""
            int main(void) {
                int return = 4;
                return return + 1;
            }
            """, 
            "Expected '}' but found 'int'");
        Add("""
            int main(void) {
                int a = 0;
                a + = 1;
                return a;
            }
            """, 
            "Expected expression but found '='");
        Add("""
            int main(void)
            {
                return !10
            }
            """, 
            "Expected ';' but found '}'");
        Add("""
            int main(void) {
                return 1 || 2
            }
            """, 
            "Expected ';' but found '}'");
        Add("""
            int main(void) {
                return 2 && ~;
            }
            """, 
            "Expected expression but found ';'");
        Add("""
            int main(void) {
                return 1 < > 3;
            }
            """, 
            "Expected expression but found '>'");
        Add("""
            int main(void) {
                return <= 2;
            }
            """, 
            "Expected expression but found '<='");
        Add("""
            int main(void)
            {
                10 <= !;
            }
            """, 
            "Expected expression but found ';'");
        Add("""
            int main(void) {
                return 1 | | 2;
            }
            """, 
            "Expected expression but found '|'");
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
                return 2 (- 3);
            }
            """, 
            "Expected ';' but found '('");
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
            "Expected ';' but found '0'");        
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
            "Expected ';' but found '0'");        
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
            "Expected ';' but found 'n'");        
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