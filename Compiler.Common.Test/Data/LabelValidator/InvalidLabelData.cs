namespace Compiler.Common.Test.Data.LabelValidator;

public class InvalidLabelData : TheoryData<string, string>
{
    public InvalidLabelData()
    {
        Add
        (
            """
            int main(void) {
                int x = 0;
                if (x) {
                    x = 5;
                    goto l;
                    return 0;
                    l:
                        return x;
                } else {
                    goto l;
                    return 0;
                    l:
                        return x;
                }
            }
            """,
            "Duplicate label: l"
        );
        Add
        (
            """
            int main(void) {
                int x = 0;
            label:
                x = 1;
            label:
                return 2;
            }
            """,
            "Duplicate label: label"
        );
        Add
        (
            """
            int main(void) {
                goto label;
                return 0;
            }
            """,
            "Label 'label' used but not defined"
        );
        Add
        (
            """
            int main(void) {
                int a;
                goto a;
                return 0;
            }
            """,
            "Label 'a' used but not defined"
        );
    }
}