using Compiler.Common.Tokens;

namespace Compiler.Common.Test.Data;

public class IntegralConstantTestData : TheoryData<int, string, TokenType, string>
{
    public IntegralConstantTestData()
    {
        // Offset, Input, Expected Type, Expected Value.
        Add(7, "return 2 + 10; ", TokenType.IntegralConstant, "2");
        Add(11, "return 2 + 10; ", TokenType.IntegralConstant, "10");
        Add(0, "1000000", TokenType.IntegralConstant, "1000000");
        Add(0, "-23", TokenType.IntegralConstant, "-23");
    }
}