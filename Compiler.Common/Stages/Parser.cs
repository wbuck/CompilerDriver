using System.Runtime.InteropServices;
using Compiler.Common.Nodes;
using Compiler.Common.Tokens;

namespace Compiler.Common.Stages;

public sealed class Parser
{
    private readonly ReadOnlyMemory<char> _fileContent;

    public Parser(ReadOnlyMemory<char> fileContent)
    {
        _fileContent = fileContent;
    }
    
    public Node? Parse(List<IToken> tokens)
    {
        try
        {
            var input = CollectionsMarshal.AsSpan(tokens);
            var nodes = new List<Node>();
       
            while (!input.IsEmpty)
            {
                nodes.Add(ParseFunction(ref input));
            }
            return new RootNode(nodes);
        }
        catch (Exception x)
        {
            Console.Error.WriteLine(x);
        }
        return null;        
    }

    private FunctionNode ParseFunction(ref Span<IToken> tokens)
    {
        Check("int", TokenType.Keyword, ConsumeFirst(ref tokens));
        var name = ParseIdentifier(ref tokens);
        
        Check("(", TokenType.OpenParenthesis, ConsumeFirst(ref tokens));
        Check("void", TokenType.Keyword, ConsumeFirst(ref tokens));
        Check(")", TokenType.CloseParenthesis, ConsumeFirst(ref tokens));
        Check("{", TokenType.OpenBrace, ConsumeFirst(ref tokens));
        
        var body = ParseStatement(ref tokens);
        
        Check("}", TokenType.CloseBrace, ConsumeFirst(ref tokens));
        
        return new FunctionNode(name.ToString(), body);
    }

    private ReadOnlySpan<char> ParseIdentifier(ref Span<IToken> tokens)
    {
        var token = Check(TokenType.Identifier, ConsumeFirst(ref tokens));
        return _fileContent.Slice(token.Index, token.Length).Span;
    }

    private ReturnNode ParseStatement(ref Span<IToken> tokens)
    {
        Check("return", TokenType.Keyword, ConsumeFirst(ref tokens));
        var expression = ParseExpression(ref tokens);
        Check(";", TokenType.Semicolon, ConsumeFirst(ref tokens));
        return new ReturnNode(expression);
    }
    
    private IntegralConstantNode<int> ParseExpression(ref Span<IToken> tokens)
    {
        var token = ConsumeFirst(ref tokens);
        var value = _fileContent.Slice(token.Index, token.Length).Span;

        return !int.TryParse(value, out var result)
            ? throw new ApplicationException($"Invalid token {value}")
            : new IntegralConstantNode<int>(result);
    }

    private static IToken ConsumeFirst(ref Span<IToken> tokens)
    {
        if (tokens.IsEmpty)        
            throw new ApplicationException("Expected at least 1 token");

        var token = tokens[0];
        tokens = tokens[1..];
        
        return token;
    }

    private IToken Check(in ReadOnlySpan<char> expectedValue, TokenType expectedType, IToken token)
    {
        var actual = _fileContent.Span.Slice(token.Index, token.Length);
        return expectedType != token.Type || !expectedValue.SequenceEqual(actual)
            ? throw new ApplicationException($"Expected {expectedValue} but found {actual}") 
            : token;
    }
    
    private static IToken Check(TokenType expectedType, IToken token)
        => expectedType != token.Type 
            ? throw new ApplicationException($"Expected {expectedType} but found {token.Type}") 
            : token;
}