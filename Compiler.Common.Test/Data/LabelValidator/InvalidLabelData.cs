namespace Compiler.Common.Test.Data.LabelValidator;

public class InvalidLabelData : TheoryData<string, string>
{
    public InvalidLabelData()
    {    
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch (a) {
                    case 1: goto foo;
                    default: return 0;
                }
                return 0;
            }
            """,
            "Label 'foo' used but not defined"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch(a + 1) {
                    case 0:
                        a = 4;
                        continue;
                    default: a = 1;
                }
                return a;
            }
            """,
            "continue statement not within a loop"
        );
        Add
        (
            """
            int main(void) {
                label: break;
                return 0;
            }
            """,
            "break statement not within loop or switch"
        );
        Add
        (
            """
            int main(void) {
                do {
                lbl:
                    return 1;
                lbl:
                    return 2;
                } while (1);
                return 0;
            }
            """,
            "Duplicate label: lbl"
        );
        Add
        (
            """
            int main(void) {
                    int a = 1;
            label:
            
                switch (a) {
                    case 1:
                        return 0;
                    default:
                    label:
                        return 1;
                }
                return 0;
            }
            """,
            "Duplicate label: label"
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                switch(a) {
                    case 0: return 0;
                    default: return 1;
                    case 2: return 2;
                    default: return 2;
                }
            }
            """,
            "multiple default labels in one switch"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                switch (a) {
                    case 1:
                    for (int i = 0; i < 10; i = i + 1) {
                        continue;
                        while(1)
                        default:;
                    }
                    case 2:
                    return 0;
                    default:;
                }
                return 0;
            }
            """,
            "multiple default labels in one switch"
        );
        Add
        (
            """
            int main(void) {
                switch(4) {
                    case 2 + 3: return 0;
                    case 4 * 5 - 15: return 1;
                }
            }
            """,
            "duplicate case value: 5"
        );
        Add
        (
            """
            int main(void) {
                switch(4) {
                    case 5: return 0;
                    case 4: return 1;
                    case 5: return 0;
                    default: return 2;
                }
            }
            """,
            "duplicate case value: 5"
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                switch (a) {
                    case 1: {
                        if(1) {
                            case 1:
                            return 0;
                        }
                    }
                }
                return 0;
            }
            """,
            "duplicate case value: 1"
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
            label:
                switch (a) {
                    case 1:
                    case 1:
                        break;
                }
                return 0;
            }
            """,
            "duplicate case value: 1"
        );
        Add
        (
            """
            int main(void) {
                {
                    default: return 0;
                }
            }
            """,
            "default statement not within switch"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch(a + 1) {
                    case 0:
                        a = 1;
                        
                    default: continue;
                }
                return a;
            }
            """,
            "continue statement not within a loop"
        );
        Add
        (
            """
            int main(void) {
                for (int i = 0; i < 10; i = i + 1) {
                    case 0: return 1;
                }
                return 9;
            }
            """,
            "case statement not within switch"
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                switch(a + 1) {
                    case 0:
                        continue;
                    default: a = 1;
                }
                return a;
            }
            """,
            "continue statement not within a loop"
        );
        Add
        (
            """
            int main(void) {
                {
                    int a;
                    continue;
                }
                return 0;
            }
            """,
            "continue statement not within a loop"
        );
        Add
        (
            """
            int main(void) {
                if (1)
                    break;
            }
            """,
            "break statement not within loop or switch"
        );
        Add
        (
            """
            int main(void) {
                do {
                lbl:
                    return 1;
                lbl:
                    return 2;
                } while (1);
                return 0;
            }
            """,
            "Duplicate label: lbl"
        );
        Add
        (
            """
            int main(void) {
                {
                    int a;
                    continue;
                }
                return 0;
            }
            """,
            "continue statement not within a loop"
        );
        Add
        (
            """
            int main(void) {
                if (1)
                    break;
            }
            """,
            "break statement not within loop or switch"
        );
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