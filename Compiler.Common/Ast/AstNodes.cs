using System.Collections.Frozen;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;
using Compiler.Common.Generation;
using Compiler.Common.Tacky;
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
    RightShift,
    Not,
    LogicalAnd,
    LogicalOr,
    Equal,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    Assignment,
    Variable,
    Declaration,
    Expression,
    Null
}

public interface IAstNodeTag
{
    AstNodeTag Tag { get; }
}

public sealed record FunctionNode(string Name, string ReturnType, List<IBlockItem> Body) : IAstNodeTag
{
    public AstNodeTag Tag => AstNodeTag.Function;
}

public interface IBlockItem : IAstNodeTag;

public interface IDeclarationNode : IBlockItem;
public sealed record DeclarationNode(string Identifier, IExpressionNode? Initializer = null) : IDeclarationNode
{
    public AstNodeTag Tag => AstNodeTag.Declaration;
}

public interface IStatementNode : IBlockItem;
public sealed record ReturnNode(IExpressionNode Expression) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Return;
}
public sealed record ExpressionNode(IExpressionNode Expression) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Expression;
}
public sealed record NullNode : IStatementNode
{
    public static NullNode Statement { get; } = new();
    private NullNode() { }
    public AstNodeTag Tag => AstNodeTag.Null;
}

public interface IExpressionNode : IAstNodeTag;
public interface IConstantNode : IExpressionNode;
public sealed record ConstantNode<T>(T Value) : IConstantNode where T : INumber<T>
{
    public AstNodeTag Tag => AstNodeTag.Constant;
}
public sealed record UnaryNode(IUnaryOperatorNode Operator, IExpressionNode Expression) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Unary;
}
public sealed record BinaryNode(IBinaryOperatorNode Operator, IExpressionNode Lhs, IExpressionNode Rhs) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Binary;
}
public sealed record BitwiseNode(IBitwiseOperatorNode Operator, IExpressionNode Lhs, IExpressionNode Rhs) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Bitwise;
}
public sealed record AssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Assignment;
}
public sealed record VariableNode(string Identifier) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Variable;
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
public sealed record NegateNode : IUnaryOperatorNode
{
    public static NegateNode Operator { get; } = new();
    private NegateNode() { }
    public AstNodeTag Tag => AstNodeTag.Negate;
}
public sealed record ComplementNode : IUnaryOperatorNode
{
    public static ComplementNode Operator { get; } = new();
    private ComplementNode() { }
    public AstNodeTag Tag => AstNodeTag.Complement;
}
public sealed record NotNode : IUnaryOperatorNode
{
    public static NotNode Operator { get; } = new();
    private NotNode() { }
    public AstNodeTag Tag => AstNodeTag.Not;
}

public interface IBinaryOperatorNode : IAstNodeTag;
public sealed record AdditionNode : IBinaryOperatorNode
{
    public static AdditionNode Operator { get; } = new();
    private AdditionNode() { }
    public AstNodeTag Tag => AstNodeTag.Addition;
}
public sealed record SubtractionNode : IBinaryOperatorNode
{
    public static SubtractionNode Operator { get; } = new();
    private SubtractionNode() { }
    public AstNodeTag Tag => AstNodeTag.Subtraction;
}
public sealed record MultiplicationNode : IBinaryOperatorNode
{
    public static MultiplicationNode Operator { get; } = new();
    private MultiplicationNode() { }
    public AstNodeTag Tag => AstNodeTag.Multiplication;
}
public sealed record DivisionNode : IBinaryOperatorNode
{
    public static DivisionNode Operator { get; } = new();
    private DivisionNode() { }
    public AstNodeTag Tag => AstNodeTag.Division;
}
public sealed record RemainderNode : IBinaryOperatorNode
{
    public static RemainderNode Operator { get; } = new();
    private RemainderNode() { }
    public AstNodeTag Tag => AstNodeTag.Remainder;
}
public sealed record LogicalAndNode : IBinaryOperatorNode
{
    public static LogicalAndNode Operator { get; } = new();
    private LogicalAndNode() { }
    public AstNodeTag Tag => AstNodeTag.LogicalAnd;
}
public sealed record LogicalOrNode : IBinaryOperatorNode
{
    public static LogicalOrNode Operator { get; } = new();
    private LogicalOrNode() { }
    public AstNodeTag Tag => AstNodeTag.LogicalOr;
}
public sealed record EqualNode : IBinaryOperatorNode
{
    public static EqualNode Operator { get; } = new();
    private EqualNode() { }
    public AstNodeTag Tag => AstNodeTag.Equal;
}
public sealed record NotEqualNode : IBinaryOperatorNode
{
    public static NotEqualNode Operator { get; } = new();
    private NotEqualNode() { }
    public AstNodeTag Tag => AstNodeTag.NotEqual;
}
public sealed record LessThanNode : IBinaryOperatorNode
{
    public static LessThanNode Operator { get; } = new();
    private LessThanNode() { }
    public AstNodeTag Tag => AstNodeTag.LessThan;
}
public sealed record LessThanOrEqualNode : IBinaryOperatorNode
{
    public static LessThanOrEqualNode Operator { get; } = new();
    private LessThanOrEqualNode() { }
    public AstNodeTag Tag => AstNodeTag.LessThanOrEqual;
}
public sealed record GreaterThanNode : IBinaryOperatorNode
{
    public static GreaterThanNode Operator { get; } = new();
    private GreaterThanNode() { }
    public AstNodeTag Tag => AstNodeTag.GreaterThan;
}
public sealed record GreaterThanOrEqualNode : IBinaryOperatorNode
{
    public static GreaterThanOrEqualNode Operator { get; } = new();
    private GreaterThanOrEqualNode() { }
    public AstNodeTag Tag => AstNodeTag.GreaterThanOrEqual;
}


public record ProgramNode(FunctionNode Function) : IAstNodeTag
{
    private static readonly FrozenDictionary<TokenType, int> Precedence = new Dictionary<TokenType, int>
    {         
        [TokenType.Asterisk] = 40,
        [TokenType.ForwardSlash] = 40,
        [TokenType.Percent] = 40,
        [TokenType.Plus] = 35,
        [TokenType.Minus] = 35,
        [TokenType.LeftShift] = 30,
        [TokenType.RightShift] = 30,
        [TokenType.GreaterThanOrEqual] = 25,
        [TokenType.LessThanOrEqual] = 25,
        [TokenType.LessThan] = 25,
        [TokenType.GreaterThan] = 25,
        [TokenType.Equal] = 20,
        [TokenType.NotEqual] = 20,
        [TokenType.BitwiseAnd] = 15,
        [TokenType.BitwiseXor] = 14,
        [TokenType.BitwiseOr] = 13,
        [TokenType.LogicalAnd] = 12,
        [TokenType.LogicalOr] = 11,
        [TokenType.Assignment] = 1
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

        List<IBlockItem> body = [];
        while (!CheckType(shifted, TokenType.CloseBrace))
        {
            if (ParseBlockItem(ref shifted, fileContent) is not { } item)
                break;
            
            body.Add(item);
        }
            
        //throw new FormatException($"Unexpected token: {ReadTokenValue(shifted, fileContent.Span)}");
        AssertTypeAndConsume(shifted, TokenType.CloseBrace, fileContent.Span, out shifted);
        
        tokens = shifted;        
        var returnType = fileContent.Slice(keyword.Index, keyword.Length);
        
        return new FunctionNode(GetString(id, fileContent), returnType.ToString(), body);
    }

    private static IBlockItem? ParseBlockItem(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (tokens.IsEmpty)
            return null;                

        if (ParseStatement(ref tokens, fileContent) is { } statement)
            return statement;         
        
        if (ParseDeclaration(ref tokens, fileContent) is { } declaration)
            return declaration;

        return null;
    }    

    private static DeclarationNode? ParseDeclaration(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckType(tokens, TokenType.Keyword))
            return null;

        if (!Shift(tokens, out var shifted) || !CheckType(shifted, TokenType.Identifier))
            return null;

        // TODO: Handle different type other than int.
        AssertKeywordAndConsume(tokens, "int", fileContent.Span, out tokens);
        var id = AssertTokenAndConsume<IdentifierToken>(ref tokens, TokenType.Identifier);

        if (GetTokenAndConsume<AssignmentToken>(ref tokens) is not { } keyword)
        {
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
            return new DeclarationNode(GetString(id, fileContent));            
        }
        
        var rhs = ParseExpression(ref tokens, fileContent);
        AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        return new DeclarationNode(GetString(id, fileContent), rhs);
    }

    private static IStatementNode? ParseStatement(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        
        if (ParseReturn(ref tokens, fileContent) is { } @return)
            return @return;
        if (ParseExpression(ref tokens, fileContent) is { } expression)
        {
            var expr = new ExpressionNode(expression);
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
            return expr;
        }
        if (CheckTypeAndConsume(tokens, TokenType.Semicolon, out tokens))
            return NullNode.Statement;

        return null;
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
        while (PeekBitwiseOrBinaryOperator(ref tokens, out var op) && Precedence[op] >= precedence)
        {
            if (lhs is null)
                throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
            if (op is TokenType.Assignment)
            {
                AssertTypeAndConsume(tokens, TokenType.Assignment, fileContent.Span, out tokens);
                
                if (ParseExpression(ref tokens, fileContent, Precedence[op]) is not { } rhs)
                    throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
                
                lhs = new AssignmentNode(lhs, rhs);
                continue;           
            }
            if (ParseBinaryOperator(ref tokens) is { } binary)
            {
                if (ParseExpression(ref tokens, fileContent, Precedence[op] + 1) is not { } rhs)
                    throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

                lhs = new BinaryNode(binary, lhs, rhs);
                continue;
            }
            if (ParseBitwiseOperator(ref tokens) is { } bitwise)
            {
                if (ParseExpression(ref tokens, fileContent, Precedence[op] + 1) is not { } rhs)
                    throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

                lhs = new BitwiseNode(bitwise, lhs, rhs);
                continue;
            }
            
            throw new FormatException($"Expected binary or bitwise operator but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        }
        return lhs;
    }

    private static bool PeekBitwiseOrBinaryOperator(ref Span<IToken> tokens, out TokenType type)
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
        if (CheckType(tokens, TokenType.LogicalAnd))
            type = TokenType.LogicalAnd;
        if (CheckType(tokens, TokenType.LogicalOr))
            type = TokenType.LogicalOr;
        if (CheckType(tokens, TokenType.Equal))
            type = TokenType.Equal;
        if (CheckType(tokens, TokenType.NotEqual))
            type = TokenType.NotEqual;
        if (CheckType(tokens, TokenType.LessThan))
            type = TokenType.LessThan;
        if (CheckType(tokens, TokenType.LessThanOrEqual))
            type = TokenType.LessThanOrEqual;
        if (CheckType(tokens, TokenType.GreaterThan))
            type = TokenType.GreaterThan;
        if (CheckType(tokens, TokenType.GreaterThanOrEqual))
            type = TokenType.GreaterThanOrEqual;
        if (CheckType(tokens, TokenType.Assignment))
            type = TokenType.Assignment;
        
        return type != TokenType.Unknown;
    }    
    
    private static IExpressionNode? ParseFactor(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (ParseConstant<int>(ref tokens, fileContent) is { } constant)
            return constant;
        
        if (ParseUnary(ref tokens, fileContent) is { } unary)
            return unary;
        
        if (ParseParenthesizedExpression(ref tokens, fileContent) is { } expression)
            return expression;
        
        if (ParseVariable(ref tokens, fileContent) is { } variable)
            return variable;

        return null;
    }

    private static VariableNode? ParseVariable(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckType(tokens, TokenType.Identifier))
            return null;
        
        var id = AssertTokenAndConsume<IdentifierToken>(ref tokens, TokenType.Identifier);
        return new VariableNode(GetString(id, fileContent));
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
        if (ParseUnaryOperator(ref tokens) is not { } op) 
            return null;
        
        if (ParseFactor(ref tokens, fileContent) is not { } factor)
            throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
        return new UnaryNode(op, factor);
    }

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
        return new ConstantNode<T>(number);
    }
    
    private static IBitwiseOperatorNode? ParseBitwiseOperator(ref Span<IToken> tokens)
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
    
    private static IBinaryOperatorNode? ParseBinaryOperator(ref Span<IToken> tokens)
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
        if (CheckTypeAndConsume(tokens, TokenType.LogicalAnd, out tokens))
            return LogicalAndNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.LogicalOr, out tokens))
            return LogicalOrNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Equal, out tokens))
            return EqualNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.NotEqual, out tokens))
            return NotEqualNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.LessThan, out tokens))
            return LessThanNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.LessThanOrEqual, out tokens))
            return LessThanOrEqualNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.GreaterThan, out tokens))
            return GreaterThanNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.GreaterThanOrEqual, out tokens))
            return GreaterThanOrEqualNode.Operator;
        
        return null;
    }
    
    private static IUnaryOperatorNode? ParseUnaryOperator(ref Span<IToken> tokens)
    {
        if (CheckTypeAndConsume(tokens, TokenType.Minus, out tokens))
            return NegateNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Complement, out tokens))
            return ComplementNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Not, out tokens))
            return NotNode.Operator;
        
        return null;
    }

    [Pure]
    private static TToken AssertTokenAndConsume<TToken>(ref Span<IToken> tokens, TokenType expected)
        where TToken : IToken
    {
        if (tokens.IsEmpty)
            throw new FormatException($"Missing expected token {expected.ToStringFast(true)}");

        var token = tokens[0];
        if (token.Type != expected)
        {
            throw new FormatException(
                $"Expected token {expected.ToStringFast(true)} but found {token.Type.ToStringFast(true)}");            
        }
                            
        tokens = tokens[1..];
        return (TToken)token;
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
    
    [Pure]
    private static string GetString(IToken token, in ReadOnlyMemory<char> fileContent)
        => fileContent.Slice(token.Index, token.Length).ToString();
}