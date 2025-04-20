using Compiler.Common.Ast;

namespace Compiler.Common.Tacky;

public abstract record TackyBase
{
    public static TackyProgram Visit(INode node)
    {
        var program = AssertNodeType<ProgramNode>(node);

        if (program.Nodes is not [FunctionNode function])
            throw new FormatException("A program node only supports a single function at this time");
        
        return new(VisitFunction(function));
    }

    private static TackyFunction VisitFunction(INode node)
    {
        var function = AssertNodeType<FunctionNode>(node);
        return new(function.Name.ToString(), VisitBlockStatement(function.Body));
    }

    private static List<TackyInstruction> VisitBlockStatement(INode node)
    {
        var statement = AssertNodeType<BlockStatementNode>(node);
        return statement.Body.SelectMany(n =>
            n switch
            {
                ReturnNode returnNode =>  VisitReturn(returnNode),
                _ => throw new FormatException($"Unknown statement type {node.NodeType}")
            }
        ).ToList();        
    }

    private static List<TackyInstruction> VisitReturn(ReturnNode node)
    {        
        switch (node.Expression)
        {
            case IntegerConstantNode or FloatConstantNode:
                return [new TackyReturn(VisitConstant(node.Expression))];
            case UnaryOperatorNode unary:
                List<TackyInstruction> instructions = [];
                instructions.Add(new TackyReturn(VisitUnary(unary, instructions)));
                return instructions;
            default:
                return [new TackyReturn(null)];
        }
    }

    private static TackyVariable VisitUnary(UnaryOperatorNode node, List<TackyInstruction> instructions)
    {
        TackyVariable dest;
        if (node is { Expression: IntegerConstantNode or FloatConstantNode })
        {
            dest = new TackyVariable(1);
            instructions.Add(new TackyUnary(GetOperator(node),VisitConstant(node.Expression), dest));
            return dest;
        }

        if (node is { Expression: UnaryOperatorNode unary })
        {
            var source = VisitUnary(unary, instructions);
            dest = source.Next();
            instructions.Add(new TackyUnary(GetOperator(node), source, dest));
            return dest;
        }
        
        throw new FormatException($"Unknown unary operator {node.NodeType}");

        static TackyUnaryOperator GetOperator(UnaryOperatorNode node)
            => node.Unary is BitwiseComplementNode 
                ? new TackyBitwiseComplement()
                : new TackyNegation();        
    }

    private static TackyValue VisitConstant(INode node)
        => node switch
        {
            IntegerConstantNode integer => new TackyIntegerConstant(integer.Value),
            FloatConstantNode floating => new TackyFloatConstant(floating.Value),
            _ => throw new FormatException($"Unknown constant type {node.NodeType}")
        };
    
    private static TNode AssertNodeType<TNode>(INode node) where TNode : INode
        => node is not TNode expected
            ? throw new FormatException($"Expected {typeof(TNode).Name}, but received {node.GetType().Name}")
            : expected;
}