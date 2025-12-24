using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Test.Data.LabelAnnotation;

public class SwitchData : DataBase
{
    public SwitchData()
    {
        Add
        (
            """
            int main(void) {
                while (1) {
                    switch (1) {
                        case 1:
                            switch (1) {
                                case 1: break;
                                case 2: continue;
                            }
                        break;
                    }
                }
            }
            """,
            GetExpected
            (
                new WhileNode
                (
                    Const(1),
                    Compound
                         (
                             new SwitchNode
                             (
                                 Const(1),
                                 Compound
                                      (
                                          new CaseNode
                                          (
                                              Const(1), 
                                              new SwitchNode
                                              (
                                                  Const(1),
                                                  Compound
                                                       (
                                                           new CaseNode(Const(1), new BreakNode(".switch3"), ".switch3.case4"),
                                                           new CaseNode(Const(2), new ContinueNode(".while1"), ".switch3.case5")
                                                       ),
                                                  [
                                                      new SwitchLabel(".switch3.case4", Const(1), 1),
                                                      new SwitchLabel(".switch3.case5", Const(2), 2)
                                                  ],
                                                  ".switch3"
                                              ), 
                                              ".switch1.case2"
                                          ),
                                          new BreakNode(".switch1")
                                      ),
                                 [
                                     new SwitchLabel(".switch1.case2", Const(1), 1)
                                 ],
                                 ".switch1"
                             )
                         ),
                    ".while1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                while(1) {
                    switch (1) {
                        case 1: break;
                        case 2: continue;
                    }
                    break;
                }
            }
            """,
            GetExpected
            (
                new WhileNode
                (
                    Const(1),
                    Compound
                    (
                        new SwitchNode
                        (
                            Const(1),
                            Compound
                                 (
                                     new CaseNode(Const(1), new BreakNode(".switch1"), ".switch1.case2"),
                                     new CaseNode(Const(2), new ContinueNode(".while1"), ".switch1.case3")
                                 ),
                            [
                                new SwitchLabel(".switch1.case2", Const(1), 1),
                                new SwitchLabel(".switch1.case3", Const(2), 2)
                            ],
                            ".switch1"
                        ),
                        new BreakNode(".while1")
                    ),
                    ".while1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (1) {
                    case ~-!1: break;
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(1),
                    Compound
                         (
                             new CaseNode
                             (
                                 new UnaryNode
                                 (
                                     ComplementNode.Operator,
                                     new UnaryNode
                                     (
                                         NegateNode.Operator,
                                         new UnaryNode
                                         (
                                             NotNode.Operator, 
                                             Const(1)
                                         )
                                     )
                                 ),
                                 new BreakNode(".switch1"),
                                 ".switch1.case2"
                             )
                         ),
                    [
                        new SwitchLabel
                        (
                            ".switch1.case2", 
                            new UnaryNode
                            (
                                ComplementNode.Operator, 
                                new UnaryNode
                                (
                                    NegateNode.Operator, 
                                    new UnaryNode(NotNode.Operator, Const(1))
                                )
                            ), 
                            -1
                        )
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (1) {
                    case 12 * (2 + !3): break;
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(1),
                    Compound
                         (
                             new CaseNode
                             (
                                 new BinaryNode
                                 (
                                     MultiplicationNode.Operator,
                                     Const(12),
                                     new BinaryNode
                                         (
                                             AdditionNode.Operator,
                                             Const(2),
                                             new UnaryNode(NotNode.Operator, Const(3))
                                         )
                                 ),
                                 new BreakNode(".switch1"),
                                 ".switch1.case2"
                             )
                         ),
                    [
                        new SwitchLabel
                        (
                            ".switch1.case2", 
                            new BinaryNode
                            (
                                MultiplicationNode.Operator, 
                                Const(12), 
                                new BinaryNode
                                    (
                                        AdditionNode.Operator, 
                                        Const(2), 
                                        new UnaryNode(NotNode.Operator, Const(3)))
                                    ), 
                            24)
                    ],
                    ".switch1"
                )                
            )
        );
        Add
        (
            """
            int main(void) {
                switch (1) {
                    case 1 + -1: break;                   
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(1),
                    Compound
                    (
                        new CaseNode
                        (
                            new BinaryNode(AdditionNode.Operator, Const(1), new UnaryNode(NegateNode.Operator, Const(1))), 
                            new BreakNode(".switch1"),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel
                        (
                            ".switch1.case2", 
                            new BinaryNode(AdditionNode.Operator, Const(1), new UnaryNode(NegateNode.Operator, Const(1))), 
                            0
                        )
                    ], 
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case ~3: return -4;               
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                    (
                        new CaseNode
                        (
                            new UnaryNode(ComplementNode.Operator, Const(3)),  
                            Ret(new UnaryNode(NegateNode.Operator, Const(4))),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel(".switch1.case2", new UnaryNode(ComplementNode.Operator, Const(3)), -4)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case -1: return -1;               
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                    (
                        new CaseNode
                        (
                            new UnaryNode(NegateNode.Operator, Const(1)),  
                            Ret(new UnaryNode(NegateNode.Operator, Const(1))),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel(".switch1.case2", new UnaryNode(NegateNode.Operator, Const(1)), -1)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case !1: return 0;               
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                    (
                        new CaseNode
                        (
                            new UnaryNode(NotNode.Operator, Const(1)),  
                            Ret(Const(0)),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel(".switch1.case2", new UnaryNode(NotNode.Operator, Const(1)), 0)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case 25 % 2: return 1;               
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                    (
                        new CaseNode
                        (
                            new BinaryNode(RemainderNode.Operator, Const(25), Const(2)),  
                            Ret(Const(1)),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel(".switch1.case2", new BinaryNode(RemainderNode.Operator, Const(25), Const(2)), 1)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case 20 * 2: return 40;                  
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                    (
                        new CaseNode
                        (
                            new BinaryNode(MultiplicationNode.Operator, Const(20), Const(2)),  
                            Ret(Const(40)),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel(".switch1.case2", new BinaryNode(MultiplicationNode.Operator, Const(20), Const(2)), 40)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case 40 / 2: return 20;                   
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                    (
                        new CaseNode
                        (
                            new BinaryNode(DivisionNode.Operator, Const(40), Const(2)),  
                            Ret(Const(20)),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel(".switch1.case2", new BinaryNode(DivisionNode.Operator, Const(40), Const(2)), 20)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case 1 - 2: return -1;                  
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                    (
                        new CaseNode
                        (
                            new BinaryNode(SubtractionNode.Operator, Const(1), Const(2)),  
                            Ret(new UnaryNode(NegateNode.Operator, Const(1))),
                            ".switch1.case2"
                        )
                    ),
                    [
                        new SwitchLabel(".switch1.case2", new BinaryNode(SubtractionNode.Operator, Const(1), Const(2)), -1)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch (10) {
                    case 5 + 5: return 10;                    
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(10),
                    Compound
                         (
                             new CaseNode
                             (
                                 new BinaryNode(AdditionNode.Operator, Const(5), Const(5)),  
                                 Ret(Const(10)),
                                 ".switch1.case2"
                             )
                         ),
                    [
                        new SwitchLabel(".switch1.case2", new BinaryNode(AdditionNode.Operator, Const(5), Const(5)), 10)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                switch(3) {
                    case 0: return 0;
                    case 1: return 1;
                    case 3: return 3;
                    case 5: return 5;        
                }
            }
            """,
            GetExpected
            (
                new SwitchNode
                (
                    Const(3),
                    Compound
                         (
                             new CaseNode(Const(0), Ret(Const(0)), ".switch1.case2"),
                             new CaseNode(Const(1), Ret(Const(1)), ".switch1.case3"),
                             new CaseNode(Const(3), Ret(Const(3)), ".switch1.case4"),
                             new CaseNode(Const(5), Ret(Const(5)), ".switch1.case5")
                         ),
                    [
                        new SwitchLabel(".switch1.case2", Const(0), 0),
                        new SwitchLabel(".switch1.case3", Const(1), 1),
                        new SwitchLabel(".switch1.case4", Const(3), 3),
                        new SwitchLabel(".switch1.case5", Const(5), 5)
                    ],
                    ".switch1"
                )
            )
        );
        Add
        (
            """
            int main(void) {
                int count = 37;
                int iterations = (count + 4) / 5;
                switch (count % 5) {
                    case 0:
                        do {
                            --count;
                            case 4:
                                --count;
                            case 3:
                                --count;
                            case 2:
                                --count;
                            case 1:
                                --count;
                        } while (--iterations > 0);
                }
                return 1;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("count.0", Const(37)),
                new VariableDeclarationNode
                (
                    "iterations.1",
                    new BinaryNode
                    (
                        DivisionNode.Operator,
                        new BinaryNode(AdditionNode.Operator, Var("count.0"), Const(4)),
                        Const(5)
                    )
                ),
                new SwitchNode
                (
                    new BinaryNode(RemainderNode.Operator, Var("count.0"), Const(5)),
                    Compound
                         (
                             new CaseNode
                             (
                                 Const(0),
                                 new DoWhileNode
                                 (
                                     Compound
                                         (
                                             new ExpressionNode(new UnaryNode(PrefixDecrementNode.Operator, Var("count.0"))),
                                             new CaseNode
                                             (
                                                 Const(4), 
                                                 new ExpressionNode(new UnaryNode(PrefixDecrementNode.Operator, Var("count.0"))),
                                                 ".switch1.case3"
                                             ),
                                             new CaseNode
                                             (
                                                 Const(3), 
                                                 new ExpressionNode(new UnaryNode(PrefixDecrementNode.Operator, Var("count.0"))),
                                                 ".switch1.case4"
                                             ),
                                             new CaseNode
                                             (
                                                 Const(2), 
                                                 new ExpressionNode(new UnaryNode(PrefixDecrementNode.Operator, Var("count.0"))),
                                                 ".switch1.case5"
                                             ),
                                             new CaseNode
                                             (
                                                 Const(1), 
                                                 new ExpressionNode(new UnaryNode(PrefixDecrementNode.Operator, Var("count.0"))),
                                                 ".switch1.case6"
                                             )
                                         ),
                                     new BinaryNode
                                     (
                                         GreaterThanNode.Operator, 
                                         new UnaryNode(PrefixDecrementNode.Operator, Var("iterations.1")), 
                                         Const(0)
                                     ),
                                     ".do_while1"
                                 ),
                                 ".switch1.case2"
                             )
                         ),
                    [
                        new SwitchLabel(".switch1.case2", Const(0), 0),
                        new SwitchLabel(".switch1.case3", Const(4), 4),
                        new SwitchLabel(".switch1.case4", Const(3), 3),
                        new SwitchLabel(".switch1.case5", Const(2), 2),
                        new SwitchLabel(".switch1.case6", Const(1), 1)
                    ],
                    ".switch1"
                ),
                Ret(Const(1))
            )
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                switch(a) {
                    case 1:
                        switch(a) {
                            case 0: return 0;
                            default: return 0;
                        }
                    default: a = 2;
                }
                return a;
            }
            """,
            GetExpected
            (
                new VariableDeclarationNode("a.0", Const(0)),
                new SwitchNode
                (
                    Var("a.0"),
                    Compound
                         (
                             new CaseNode
                             (
                                 Const(1),
                                 new SwitchNode
                                 (
                                     Var("a.0"),
                                     Compound
                                     (
                                         new CaseNode(Const(0), Ret(Const(0)), ".switch3.case4"),
                                         new DefaultNode(Ret(Const(0)), ".switch3.default")
                                     ),
                                     [
                                         new SwitchLabel(".switch3.case4", Const(0), 0),
                                         new SwitchLabel(".switch3.default", null, null)
                                     ],
                                     ".switch3"
                                 ),
                                 ".switch1.case2"
                             ),
                             new DefaultNode
                             (
                                 new ExpressionNode(new AssignmentNode(Var("a.0"), Const(2))),
                                 ".switch1.default"
                             )
                         ),
                    [
                        new SwitchLabel(".switch1.case2", Const(1), 1),
                        new SwitchLabel(".switch1.default", null, null)
                    ],
                    ".switch1"
                ),
                Ret(Var("a.0"))
            )            
        );
    }
}