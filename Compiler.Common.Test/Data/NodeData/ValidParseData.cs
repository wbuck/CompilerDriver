namespace Compiler.Common.Test.Data.NodeData;

public class ValidParseData : TheoryData<string, ExpectedProgram>
{
    public ValidParseData()
    {
        Add("""
            int main(void)
            {
                return -((((10))));
            }
            """, 
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedUnaryOperator
                                    (
                                        new ExpectedNegation(),
                                        new ExpectedIntegerConstant(10)
                                    ) 
                                )
                            ]
                        )
                    )
                ]
            )
        );
        Add("""
            int main(void) {
                return (-2);
            }
            """, 
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedUnaryOperator
                                    (
                                        new ExpectedNegation(),
                                        new ExpectedIntegerConstant(2)
                                    )  
                                )
                            ]
                        )
                    )
                ]
            )
        );
        Add("""
            int main(void) {
                return -(-4);
            }
            """, 
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedUnaryOperator
                                    (
                                        new ExpectedNegation(),
                                        new ExpectedUnaryOperator
                                        (
                                            new ExpectedNegation(),
                                            new ExpectedIntegerConstant(4)
                                        )
                                    ) 
                                )
                            ]
                        )
                    )
                ]
            )
        );
        Add("""
            int main(void) {
                return ~(2);
            }
            """, 
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedUnaryOperator
                                    (
                                        new ExpectedBitwiseComplement(),
                                        new ExpectedIntegerConstant(2)
                                    )  
                                )
                            ]
                        )
                    )
                ]
            )
        );  
        Add("""
            int main(void) {
                return 0;
            }
            """, 
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedIntegerConstant(0)
                                )
                            ]
                        )
                    )
                ]
            )
        );  
        Add("""
            int main(void) {
                return -1000;
            }
            """,
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedUnaryOperator
                                    (
                                        new ExpectedNegation(), 
                                        new ExpectedIntegerConstant(1000)
                                    )
                                )
                            ]
                        )
                    )
                ]
            )
        );
        Add("""
            int main(void) {
                return ~1000;
            }
            """,
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedUnaryOperator
                                    (
                                        new ExpectedBitwiseComplement(), 
                                        new ExpectedIntegerConstant(1000)
                                    )
                                )
                            ]
                        )
                    )
                ]
            )
        );
        Add("""
            int main(void) {
                return ~-2147483647;
            }
            """,
            new ExpectedProgram
            (
                [
                    new ExpectedFunction
                    (
                        "int", 
                        "main", 
                        new ExpectedBlockStatement
                        (
                            [
                                new ExpectedReturn
                                (
                                    new ExpectedUnaryOperator
                                    (
                                        new ExpectedBitwiseComplement(), 
                                        new ExpectedUnaryOperator
                                        (
                                            new ExpectedNegation(),
                                            new ExpectedIntegerConstant(2147483647)
                                        )
                                    )
                                )
                            ]
                        )
                    )
                ]
            )
        );
    }
}