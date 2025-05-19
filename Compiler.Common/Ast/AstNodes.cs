using System.Collections.Frozen;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;
using Compiler.Common.Tokens;
using NetEscapades.EnumGenerators;

namespace Compiler.Common.Ast;

[EnumExtensions]
public enum AstNodeTag
{
    Program,
    Function,
    Return,
    Constant,
    Unary,     
    Negate,
    Complement,
    Binary,
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Remainder,
    Bitwise,
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor,
    LeftShift,
    RightShift
}

public interface IAstNodeTag
{
    AstNodeTag Tag { get; }
}

public record FunctionNode(string Name, string ReturnType, IStatementNode Body) : IAstNodeTag
{
    public AstNodeTag Tag => AstNodeTag.Function;
}

public interface IStatementNode : IAstNodeTag;
public record ReturnNode(IExpressionNode Expression) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Return;
}

public interface IExpressionNode : IAstNodeTag;

public interface IConstantNode : IExpressionNode;
public record ConstantNode<T>(T Value) : IConstantNode where T : INumber<T>
{
    public AstNodeTag Tag => AstNodeTag.Constant;
}

public record UnaryNode(IUnaryOperatorNode Operator, IExpressionNode Expression) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Unary;
}
public record BinaryNode(IBinaryOperatorNode Operator, IExpressionNode Lhs, IExpressionNode Rhs) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Binary;
}
public record BitwiseNode(IBitwiseOperatorNode Operator, IExpressionNode Lhs, IExpressionNode Rhs) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Bitwise;
}

public interface IBitwiseOperatorNode : IAstNodeTag;
public sealed record BitwiseAndNode : IBitwiseOperatorNode
{
    public static BitwiseAndNode Operator { get; } = new();
    private BitwiseAndNode() { }
    public AstNodeTag Tag => AstNodeTag.BitwiseAnd;
}
public sealed record BitwiseOrNode : IBitwiseOperatorNode
{
    public static BitwiseOrNode Operator { get; } = new();
    private BitwiseOrNode() { }
    public AstNodeTag Tag => AstNodeTag.BitwiseOr;
}
public sealed record BitwiseXorNode : IBitwiseOperatorNode
{
    public static BitwiseXorNode Operator { get; } = new();
    private BitwiseXorNode() { }
    public AstNodeTag Tag => AstNodeTag.BitwiseOr;
}
public sealed record BitwiseLeftShiftNode : IBitwiseOperatorNode
{
    public static BitwiseLeftShiftNode Operator { get; } = new();
    private BitwiseLeftShiftNode() { }
    public AstNodeTag Tag => AstNodeTag.LeftShift;
}
public sealed record BitwiseRightShiftNode : IBitwiseOperatorNode
{
    public static BitwiseRightShiftNode Operator { get; } = new();
    private BitwiseRightShiftNode() { }
    public AstNodeTag Tag => AstNodeTag.RightShift;
}


public interface IUnaryOperatorNode : IAstNodeTag;
public record NegateNode : IUnaryOperatorNode
{
    public static NegateNode Operator { get; } = new();
    private NegateNode() { }
    public AstNodeTag Tag => AstNodeTag.Negate;
}
public record ComplementNode : IUnaryOperatorNode
{
    public static ComplementNode Operator { get; } = new();
    private ComplementNode() { }
    public AstNodeTag Tag => AstNodeTag.Complement;
}

public interface IBinaryOperatorNode : IAstNodeTag;
public record AdditionNode : IBinaryOperatorNode
{
    public static AdditionNode Operator { get; } = new();
    private AdditionNode() { }
    public AstNodeTag Tag => AstNodeTag.Addition;
}
public record SubtractionNode : IBinaryOperatorNode
{
    public static SubtractionNode Operator { get; } = new();
    private SubtractionNode() { }
    public AstNodeTag Tag => AstNodeTag.Subtraction;
}
public record MultiplicationNode : IBinaryOperatorNode
{
    public static MultiplicationNode Operator { get; } = new();
    private MultiplicationNode() { }
    public AstNodeTag Tag => AstNodeTag.Multiplication;
}
public record DivisionNode : IBinaryOperatorNode
{
    public static DivisionNode Operator { get; } = new();
    private DivisionNode() { }
    public AstNodeTag Tag => AstNodeTag.Division;
}
public record RemainderNode : IBinaryOperatorNode
{
    public static RemainderNode Operator { get; } = new();
    private RemainderNode() { }
    public AstNodeTag Tag => AstNodeTag.Remainder;
}


public record ProgramNode(FunctionNode Function) : IAstNodeTag
{
    private static readonly FrozenDictionary<TokenType, int> Precedence = new Dictionary<TokenType, int>
    {    
        [TokenType.BitwiseOr] = 25,
        [TokenType.BitwiseXor] = 30,
        [TokenType.BitwiseAnd] = 35,
        [TokenType.LeftShift] = 40,
        [TokenType.RightShift] = 40,
        [TokenType.Plus] = 45,
        [TokenType.Minus] = 45,        
        [TokenType.Asterisk] = 50,
        [TokenType.ForwardSlash] = 50,
        [TokenType.Percent] = 50
    }.ToFrozenDictionary();
    
    public AstNodeTag Tag => AstNodeTag.Program;
    
    public static ProgramNode Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (ParseFunction(ref tokens, fileContent) is not { } function)
            throw new FormatException($"Expected function but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
        return !tokens.IsEmpty
            ? throw new FormatException($"Unexpected token: {ReadTokenValue(tokens, fileContent.Span)}")
            : new ProgramNode(function);
    }    

    private static FunctionNode ParseFunction(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var shifted = tokens;
        if (GetTokenAndConsume<KeywordToken>(ref shifted) is not { } keyword)
            throw new FormatException($"Expected return type but found '{ReadTokenValue(tokens, fileContent.Span)}'");

        if (GetTokenAndConsume<IdentifierToken>(ref shifted) is not { } id)
            throw new FormatException($"Expected function identifier but found '{ReadTokenValue(shifted, fileContent.Span)}'");

        if (!CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
        {
            var expected = TokenType.OpenParenthesis.ToStringFast(true);
            throw new FormatException($"Expected '{expected}' but found '{ReadTokenValue(shifted, fileContent.Span)}'");
        }

        AssertKeywordAndConsume(shifted, "void", fileContent.Span, out shifted);  
        AssertTypeAndConsume(shifted, TokenType.CloseParenthesis, fileContent.Span, out shifted);
        AssertTypeAndConsume(shifted, TokenType.OpenBrace, fileContent.Span, out shifted);
        
        if (ParseStatement(ref shifted, fileContent) is not { } body)
            throw new FormatException($"Unexpected token: {ReadTokenValue(shifted, fileContent.Span)}");
                
        AssertTypeAndConsume(shifted, TokenType.CloseBrace, fileContent.Span, out shifted);
        
        tokens = shifted;        
        var name = fileContent.Slice(id.Index, id.Length);
        var returnType = fileContent.Slice(keyword.Index, keyword.Length);
        
        return new FunctionNode(name.ToString(), returnType.ToString(), body);
    }

    private static IStatementNode? ParseStatement(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (tokens.IsEmpty)
            return null;
        
        if (ParseReturn(ref tokens, fileContent) is not { } @return)
            throw new FormatException($"Expected 'return' but found '{ReadTokenValue(tokens, fileContent.Span)}'");

        return @return;
    }

    private static ReturnNode? ParseReturn(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, "return", out tokens))
            return null;
        
        if (ParseExpression(ref tokens, fileContent) is not { } expression)
            throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        
        AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        return new ReturnNode(expression);
    }

    private static IExpressionNode? ParseExpression(
        ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent, int precedence = 0)
    {
        var lhs = ParseFactor(ref tokens, fileContent);
        while (PeekOperator(ref tokens, out var type) && Precedence[type] >= precedence)
        {
            if (lhs is null)
                throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

            if (ParseBinaryOperator(ref tokens, fileContent) is { } binary)
            {
                if (ParseExpression(ref tokens, fileContent, Precedence[type] + 1) is not { } rhs)
                    throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

                lhs = new BinaryNode(binary, lhs, rhs);
                continue;
            }
            if (ParseBitwiseOperator(ref tokens, fileContent) is { } bitwise)
            {
                if (ParseExpression(ref tokens, fileContent, Precedence[type] + 1) is not { } rhs)
                    throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

                lhs = new BitwiseNode(bitwise, lhs, rhs);
                continue;
            }
            
            throw new FormatException($"Expected binary or bitwise operator but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        }
        return lhs;
    }

    private static bool PeekOperator(ref Span<IToken> tokens, out TokenType type)
    {
        type = TokenType.Unknown;        
        if (CheckType(tokens, TokenType.Plus))
            type = TokenType.Plus;    
        if (CheckType(tokens, TokenType.Minus))
            type = TokenType.Minus;     
        if (CheckType(tokens, TokenType.Asterisk))
            type = TokenType.Asterisk;
        if (CheckType(tokens, TokenType.ForwardSlash))
            type = TokenType.ForwardSlash;
        if (CheckType(tokens, TokenType.Percent))
            type = TokenType.Percent;
        if (CheckType(tokens, TokenType.BitwiseAnd))
            type = TokenType.BitwiseAnd;
        if (CheckType(tokens, TokenType.BitwiseOr))
            type = TokenType.BitwiseOr;
        if (CheckType(tokens, TokenType.BitwiseXor))
            type = TokenType.BitwiseXor;
        if (CheckType(tokens, TokenType.LeftShift))
            type = TokenType.LeftShift;
        if (CheckType(tokens, TokenType.RightShift))
            type = TokenType.RightShift;
        
        return type != TokenType.Unknown;
    }

    private static IBitwiseOperatorNode? ParseBitwiseOperator(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (CheckTypeAndConsume(tokens, TokenType.BitwiseAnd, out tokens))
            return BitwiseAndNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.BitwiseOr, out tokens))
            return BitwiseOrNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.BitwiseXor, out tokens))
            return BitwiseXorNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.LeftShift, out tokens))
            return BitwiseLeftShiftNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.RightShift, out tokens))
            return BitwiseRightShiftNode.Operator;
        
        return null;
    }
    
    private static IBinaryOperatorNode? ParseBinaryOperator(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (CheckTypeAndConsume(tokens, TokenType.Plus, out tokens))
            return AdditionNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Minus, out tokens))
            return SubtractionNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Asterisk, out tokens))
            return MultiplicationNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.ForwardSlash, out tokens))
            return DivisionNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Percent, out tokens))
            return RemainderNode.Operator;
        
        return null;
    }
    
    private static IExpressionNode? ParseFactor(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (ParseConstant<int>(ref tokens, fileContent) is { } constant)
            return constant;
        
        if (ParseUnary(ref tokens, fileContent) is { } unary)
            return unary;
        
        if (ParseParenthesizedExpression(ref tokens, fileContent) is { } expression)
            return expression;

        return null;
    }

    private static IExpressionNode? ParseParenthesizedExpression(ref Span<IToken> tokens,
        ReadOnlyMemory<char> fileContent)
    {
        if (!CheckTypeAndConsume(tokens, TokenType.OpenParenthesis, out tokens)) 
            return null;
        
        var expression = ParseExpression(ref tokens, fileContent);
        AssertTypeAndConsume(tokens, TokenType.CloseParenthesis, fileContent.Span, out tokens);
        return expression;
    }

    private static UnaryNode? ParseUnary(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (ParseNegate(ref tokens) is { } negate)
            return ParseUnaryInternal(negate, ref tokens, fileContent);;

        if (ParseComplement(ref tokens) is { } complement)
            return ParseUnaryInternal(complement, ref tokens, fileContent);

        return null;

        static UnaryNode ParseUnaryInternal(IUnaryOperatorNode op, ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
        {
            if (ParseFactor(ref tokens, fileContent) is not { } factor)
                throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
            return new UnaryNode(op, factor);
        }
    }
    
    private static NegateNode? ParseNegate(ref Span<IToken> tokens)
        => CheckTypeAndConsume(tokens, TokenType.Minus, out tokens)
            ? NegateNode.Operator
            : null;
    
    private static ComplementNode? ParseComplement(ref Span<IToken> tokens)
        => CheckTypeAndConsume(tokens, TokenType.BitwiseComplement, out tokens)
            ? ComplementNode.Operator
            : null;

    private static ConstantNode<T>? ParseConstant<T>(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent) 
        where T : INumber<T>
    {
        if (!CheckType(tokens, TokenType.NumericConstant))
            return null;

        if (GetToken<NumericConstantToken>(tokens) is not { } token)
            return null;
        
        var value = fileContent.Slice(token.Index, token.Length);

        if (!T.TryParse(value.Span, CultureInfo.InvariantCulture, out var number)) 
            return null;
        
        AssertShift(tokens, out tokens);
        return new(number);
    }
    
    [Pure]
    private static TToken? GetTokenAndConsume<TToken>(ref Span<IToken> tokens) 
        where TToken : class, IToken
    {
        if (GetToken<TToken>(tokens) is not { } token)
            return null;

        tokens = tokens[1..];
        return token;
    }
    
    [Pure]
    private static TToken? GetToken<TToken>(in Span<IToken> tokens) 
        where TToken : class, IToken
    {
        if (tokens.IsEmpty)
            return null;
        
        return tokens[0] as TToken;
    }
    
    [Pure]
    private static ReadOnlySpan<char> ReadTokenValue(in Span<IToken> tokens, in ReadOnlySpan<char> fileContent)
        => tokens.IsEmpty ? default : fileContent.Slice(tokens[0].Index, tokens[0].Length);
    
    [Pure]
    private static bool CheckKeyword(in Span<IToken> tokens, in ReadOnlySpan<char> keyword) => 
        !tokens.IsEmpty && 
        tokens[0] is KeywordToken token && 
        token.Keyword.AsSpan().SequenceEqual(keyword);

    [Pure]
    private static void AssertTypeAndConsume(
        Span<IToken> tokens, TokenType tokenType, ReadOnlySpan<char> fileContent, out Span<IToken> shifted)
    {
        if (CheckTypeAndConsume(tokens, tokenType, out shifted))
            return;
        
        if (tokens.IsEmpty)
            throw new FormatException($"Missing '{tokenType.ToStringFast(true)}'");
        
        var value = ReadTokenValue(tokens, fileContent);
        throw new FormatException($"Expected '{tokenType.ToStringFast(true)}' but found '{value}'");
    }
    
    [Pure]
    private static void AssertKeywordAndConsume(
        Span<IToken> tokens, ReadOnlySpan<char> keyword, ReadOnlySpan<char> fileContent, out Span<IToken> shifted)
    {
        if (CheckKeywordAndConsume(tokens, keyword, out shifted))
            return;
        
        throw tokens.IsEmpty 
            ? new FormatException($"Missing '{keyword}'")
            : new FormatException($"Expected '{keyword}' but found '{ReadTokenValue(tokens, fileContent)}'");                    
    }

    [Pure]
    private static bool CheckTypeAndConsume(Span<IToken> tokens, TokenType tokenType, out Span<IToken> shifted)
    {
        shifted = tokens;
        return CheckType(tokens, tokenType) && Shift(tokens, out shifted);
    }
    
    [Pure]
    private static bool CheckKeywordAndConsume(Span<IToken> tokens, ReadOnlySpan<char> keyword, out Span<IToken> shifted)
    {
        shifted = tokens;
        return CheckKeyword(tokens, keyword) && Shift(tokens, out shifted);
    }

    [Pure]
    private static bool CheckType(in Span<IToken> tokens, in TokenType tokenType, int index = 0) 
        => index > -1 && tokens.Length > index && tokens[index].Type == tokenType;

    [Pure]
    private static bool Shift(Span<IToken> tokens, out Span<IToken> shifted, int amount = 1)
    {
        if (tokens.Length < amount)
        {
            shifted = [];
            return false;
        }
        
        shifted = tokens[amount..];
        return true;
    }
    
    [Pure]
    private static void AssertShift(Span<IToken> tokens, out Span<IToken> shifted, int amount = 1)
    {
        if (tokens.Length < amount)
        {
            shifted = [];
            throw new FormatException($"Expected '{amount}' tokens but found '{tokens.Length}'");       
        }        
        shifted = tokens[amount..];
    }
}