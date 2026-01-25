
namespace Compiler.Parser.Test.Data;

public class InvalidParseData : TheoryData<string, string>
{
    public InvalidParseData()
    {
        Add
        (
            """
            int bad_params(int a = 3) {
                return 1;
            }
            
            int main(void) {
                return 0;
            }
            """,
            "Expected parameter type but found '='"
        );
        Add
        (
            """
            int foo(int a, int b {
                return 0;
            }
            
            int main(void) {
                return 0;
            }
            """,
            "Expected parameter type but found '{'"
        );
        Add
        (
            """
            int foo(int a, int b, int c) {
                return a + b + c;
            }
            
            int main(void) {
                return foo(1, 2, 3,);
            }
            """,
            "Expected expression but found ')'"
        );
        Add
        (
            """
            int foo(int a,) {
                return a + 1;
            }
            
            int main(void) {
                return foo(4);
            }
            """,
            "Expected parameter type but found ')'"
        );
        Add
        (
            """
            int foo(void) = 3;
            
            int main(void) {
                return 0;
            }
            """,
            "error: illegal initializer (only variables can be initialized)"
        );
        Add
        (
            """
            int foo(void)(void);
            
            int main(void) {
                return 0;
            }
            """,
            "error: expected function body after function declarator"
        );
        Add
        (
            """
            int foo(int a) {
                return 0;
            }
            
            int main(void) {
                return foo(int a);
            }
            """,
            "Expected expression but found 'int'"
        );
        Add
        (
            """
            int foo(int x, int y) {
                return x + y;
            }
            
            int main(void) { return foo(1, 2};}
            """,
            "Expected expression but found '}'"
        );
        Add
        (
            """
            int main(void) {
                for (int f(void); ; ) {
                    return 0;
                }
            }
            """,
            "Expected ';' but found 'int'"
        );
        Add
        (
            """
            int foo(int x, int y} { return x + y; }
            
            int main(void) { return 0;}
            """,
            "Expected parameter type but found '}'"
        );
        Add
        (
            """
            int main(void) {
                return 1();
            }
            """,
            "Expected ';' but found '('"
        );
        Add
        (
            """
            int main(void) {
                switch {
                    return 0;
                }
            }
            """,
            "Expected '(' but found '{'"
        );
        Add
        (
            """
            int main(void) {
                switch 3 {
                    case 3: return 0;
                }
            }
            """,
            "Expected '(' but found '3'"
        );
        Add
        (
            """
            int main(void) {
                switch(0) {
                    case: return 0;
                }
            }
            """,
            "Expected expression but found ':'"
        );
        Add
        (
            """
            int main(void) {
                goto 3;
                switch (3) {
                    case 3: return 0;
                }
            }
            """,
            "Expected token Identifier but found NumericConstant"
        );
        Add
        (
            """
            int main(void) {
                switch(3) {
                    case 3:
                        int i = 0;
                        return i;
                }
                return 0;
            }
            """,
            "Expected statement but found 'int'"
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 0;
                do
                do_body:
                    a = a + 1;
                    b = b - 1;
                while (a < 10)
                    ;
                return 0;
            }
            """,
            "Expected 'while' but found 'b'"
        );
        Add
        (
            """
            int main(void) {
                for (int i = 0; label: i < 10; i = i + 1) {
                    ;
                }
                return 0;
            }
            """,
            "Expected ';' but found ':'"
        );
        Add
        (
            """
            int main(void) {
                for (int i += 1; i < 10; i += 1) {
                    return 0;
                }
            }
            """,
            "Expected ';' but found '+='"
        );
        Add("""
            int main(void) {
                while 1 {
                    return 0;
                }
            }             
            """,
            "Expected '(' but found '1'");
        Add("""
            int main(void) {
                while(int a) {
                    2;
                }
            }                
            """,
            "Expected expression but found '('");
        Add("""
            int main(void) {
                for (int i = 2; ))
                    int a = 0;
            }                 
            """,
            "Expected ';' but found ')'");
        Add("""
            int main(void) {
                for (int i = 0; i < 10)
                    ;
                return 0;
            }                    
            """,
            "Expected ';' but found ')'");
        Add("""
            int main(void) {
                for (2 + 2 == 4)
                    ;
                return 0;
            }            
            """,
            "Expected ';' but found ')'");
        Add("""
            int main(void) {
                for (int i = 0;)
                    ;
                return 0;
            }
            """,
            "Expected ';' but found ')'");
        Add("""
            int main(void) {
                for (; int i = 0; i = i + 1)
                    ;
                return 0;
            }
            """,
            "Expected ';' but found 'int'");
        Add("""
            int main(void) {
                for (int i = 0; i < 10; i = i + 1; )
                    ;
                return 0;
            }
            """,
            "Expected ')' but found ';'");
        Add("""
            int main(void) {
                do
                    1;
                while ();
                return 0;
            }
            """,
            "Expected expression but found '('");
        Add("""
            int main(void) {
                do {
                    4;
                } while(1)
                return 0;
            }
            """,
            "Expected ';' but found 'return'");
        Add("""
            int main(void) {
                do {
                    int a;
                }; while(1);
                return 0;
            }
            """,
            "Expected 'while' but found ';'");
        Add("""
            int main(void) {
                while (1)
                    int i = 0;
                return 0;
            }
            """,
            "Expected statement but found 'int'");
        Add("""
            int main(void) {
                int a;
                return 1 ? { a = 2 } : a = 4;
            }
            """,
            "Expected expression but found '{'");
        Add("""
            int main(void) {
                int a = 4;
                {
                    a = 5;
                    return a
                }
            }
            """,
            "Expected ';' but found '}'");
        Add("""
            int main(void) {
                if(0){
                    return 1;
                return 2;
            }
            """,
            "Missing '}'");
        Add("""
            int main(void) {
                if(0){
                    return 1;
                }}
                return 2;
            }
            """,
            "error: type specifier missing");
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
            "expected identifier before '10'");
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
            "expected identifier before '10'");
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
            "expected identifier before '='");
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
            "expected identifier before '10'");
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
            "expected identifier before '10'");
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
            "expected identifier before '='");
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
            "error: type specifier missing"); 
        Add("""
            int 3 (void) {
                return 0;
            }
            """,
            "expected identifier before '3'");        
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
            "error: type specifier missing");        
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
            "error: expected identifier or '('");        
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
            "Expected parameter type but found '{'");
    }
}