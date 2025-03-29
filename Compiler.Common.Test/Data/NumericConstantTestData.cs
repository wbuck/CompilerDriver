using Compiler.Common.Tokens;

namespace Compiler.Common.Test.Data;

public class NumericConstantTestData : TheoryData<int, string, TokenType, string>
{
    public NumericConstantTestData()
    {
        // Offset, Input, Expected Type, Expected Value.
        Add(7, "return 2 + 10; ", TokenType.NumericConstant, "2");
        Add(11, "return 2 + 10; ", TokenType.NumericConstant, "10");
        Add(0, "1000000", TokenType.NumericConstant, "1000000");
        Add(0, "-23", TokenType.NumericConstant, "-23");
        Add(7, "return -2.11 + 10; ", TokenType.NumericConstant, "-2.11");
        Add(0, "+-.23", TokenType.NumericConstant, "+-.23");
    }
}