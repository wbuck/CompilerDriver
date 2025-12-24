using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class IdentifierTokenTestData : TheoryData<int, string, TokenType, string>
{
    public IdentifierTokenTestData()
    {
        // Position, Input, Expected Type, Expected Value.
        Add(0, "int main(void) ", TokenType.Keyword, Keyword.Int.ToStringFast(true));
        Add(4, "int main(void) ", TokenType.Identifier, "main");
        Add(9, "int main(void) ", TokenType.Keyword, Keyword.Void.ToStringFast(true));
        Add(0, "return add(a, b);", TokenType.Keyword, Keyword.Return.ToStringFast(true));
        Add(7, "return add(a, b);", TokenType.Identifier, "add");
        Add(11, "return add(a, b);", TokenType.Identifier, "a");
        Add(14, "return add(a, b);", TokenType.Identifier, "b");
        Add(0, "do { } while(true);", TokenType.Keyword, Keyword.Do.ToStringFast(true));
        Add(7, "do { } while(true);", TokenType.Keyword, Keyword.While.ToStringFast(true));
        Add(0, "for (int i = 0; i < 10; i++) { }", TokenType.Keyword, Keyword.For.ToStringFast(true));
        Add(31, "for (int i = 0; i < 10; i++) { break; }", TokenType.Keyword, Keyword.Break.ToStringFast(true));
        Add(31, "for (int i = 0; i < 10; i++) { continue; }", TokenType.Keyword, Keyword.Continue.ToStringFast(true));
        Add(31, "for (int i = 0; i < 10; i++) { switch(i) { case 1: break; } }", TokenType.Keyword, Keyword.Switch.ToStringFast(true));
        Add(43, "for (int i = 0; i < 10; i++) { switch(i) { case 1: break; } }", TokenType.Keyword, Keyword.Case.ToStringFast(true));
    }
}