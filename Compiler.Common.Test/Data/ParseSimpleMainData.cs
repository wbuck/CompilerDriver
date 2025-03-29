namespace Compiler.Common.Test.Data;

public class ParseSimpleMainData : TheoryData<string, int>
{
    public ParseSimpleMainData()
    {
        Add
        (
            "int    main    (   void)   {   return  0   ;   }", 
            0
        );
        Add
        (
            "   int   main    (  void)  {   return  0 ; }", 
            0
        );
        Add
        (
            """
            int main(void) {
                return 2;
            }
            """, 
            2
        );
        Add
        (
            """
            int main(void) {
                return 0;
            }
            """, 
            0
        );
        Add
        (
            "int main(void){return 0;}", 
            0
        );
        Add
        (
            """
            int main(void) {
                return 100;
            }
            """, 
            100
        );
        Add
        (
            """
            int
            main
            (
            void
            )
            {
            return
            0
            ;
            }
            """, 
            0
        ); 
    }
}