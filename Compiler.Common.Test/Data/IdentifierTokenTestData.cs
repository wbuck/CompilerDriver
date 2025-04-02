using Compiler.Common.Tokens;

namespace Compiler.Common.Test.Data;

public class IdentifierTokenTestData : TheoryData<int, string, TokenType, string>
{
    public IdentifierTokenTestData()
    {
        // Offset, Input, Expected Type, Expected Value.
        Add(0, "int main(void) ", TokenType.Keyword, "int");
        Add(4, "int main(void) ", TokenType.Identifier, "main");
        Add(9, "int main(void) ", TokenType.Keyword, "void");
        Add(0, "return add(a, b);", TokenType.Keyword, "return");
        Add(7, "return add(a, b);", TokenType.Identifier, "add");
        Add(11, "return add(a, b);", TokenType.Identifier, "a");
        Add(14, "return add(a, b);", TokenType.Identifier, "b");
    }
}