using System.Collections.Frozen;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;
using Compiler.Lexer.Tokens;

namespace Compiler.Parser.Nodes;

public record ProgramNode(List<FunctionDeclarationNode> Functions) : IAstNodeTag
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
        [TokenType.QuestionMark] = 3,
        [TokenType.Assignment] = 1,
        [TokenType.AdditionAssignment] = 1,
        [TokenType.SubtractionAssignment] = 1,
        [TokenType.MultiplicationAssignment] = 1,
        [TokenType.DivisionAssignment] = 1,
        [TokenType.RemainderAssignment] = 1,
        [TokenType.BitwiseAndAssignment] = 1,
        [TokenType.BitwiseOrAssignment] = 1,
        [TokenType.BitwiseXorAssignment] = 1,
        [TokenType.LeftShiftAssignment] = 1,
        [TokenType.RightShiftAssignment] = 1
    }.ToFrozenDictionary();
    
    public AstNodeTag Tag => AstNodeTag.Program;
    
    public static ProgramNode Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        List<FunctionDeclarationNode> functions = new(10);        
        while (!tokens.IsEmpty)
        {
            if (ParseFunction(ref tokens, fileContent) is not { } func)
                throw new FormatException($"Expected function but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
            functions.Add(func);           
        }
            
        return new ProgramNode(functions);
    }

    private static List<string> ParseFunctionParams(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (CheckKeywordAndConsume(tokens, Keyword.Void, out tokens))
            return [];

        List<string> parameters = new(5);
        while (!CheckType(tokens, TokenType.CloseParenthesis))
        {
            CheckTypeAndConsume(tokens, TokenType.Comma, out tokens);
            
            if (GetTokenAndConsume<KeywordToken>(ref tokens) is null)
                throw new FormatException($"Expected parameter type but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
            if (GetTokenAndConsume<IdentifierToken>(ref tokens) is not { } id)
                throw new FormatException($"Expected parameter name but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
            parameters.Add(GetString(id, fileContent));
        }
        return parameters;
    }

    private static List<IBlockItem>? ParseFunctionBody(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckTypeAndConsume(tokens, TokenType.OpenBrace, out tokens))
            return null;

        List<IBlockItem> body = [];
        while (!CheckType(tokens, TokenType.CloseBrace))
        {
            if (ParseBlockItem(ref tokens, fileContent) is not { } item)
                break;
            
            body.Add(item);
        }
            
        AssertTypeAndConsume(tokens, TokenType.CloseBrace, fileContent.Span, out tokens);
        return body;
    }

    private static FunctionDeclarationNode? ParseFunction(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var shifted = tokens;
        if (GetTokenAndConsume<KeywordToken>(ref shifted) is not { } keyword)
            return null;

        if (GetTokenAndConsume<IdentifierToken>(ref shifted) is not { } id)
            throw new FormatException($"expected identifier before '{ReadTokenValue(shifted, fileContent.Span)}'");

        if (!CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
            return null;

        var parameters = ParseFunctionParams(ref shifted, fileContent);
        AssertTypeAndConsume(shifted, TokenType.CloseParenthesis, fileContent.Span, out shifted);
        
        var body = ParseFunctionBody(ref shifted, fileContent) is { } nodes 
            ? new BlockNode(nodes) 
            : null;
        
        if (body is null)
            CheckTypeAndConsume(shifted, TokenType.Semicolon, out shifted);
        
        tokens = shifted;        
        var returnType = fileContent.Slice(keyword.Index, keyword.Length);
        
        return new FunctionDeclarationNode
        (
            GetString(id, fileContent), 
            returnType.ToString(), 
            parameters,
            body
        );
    }

    private static IBlockItem? ParseBlockItem(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (tokens.IsEmpty)
            return null;        

        if (ParseStatement(ref tokens, fileContent) is { } statement)
            return statement;       
        
        if (ParseDeclaration(ref tokens, fileContent) is { } declaration)
            return declaration;
        
        if (ParseFunction(ref tokens, fileContent) is { } func)
            return func;

        return null;
    }

    private static List<string>? ParseLabels(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        List<string>? labels = null;
        while (IsLabel(tokens))
        {
            labels ??= [];
            labels.Add(ConsumeLabel(ref tokens, fileContent));
        }
        
        return labels;
        
        static string ConsumeLabel(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
        {
            var identifier = AssertTokenAndConsume<IdentifierToken>(ref tokens, TokenType.Identifier);
            AssertTypeAndConsume(tokens, TokenType.Colon, fileContent.Span, out tokens);
            return GetString(identifier, fileContent);
        }       
        static bool IsLabel(in Span<IToken> tokens)
            => CheckType(tokens, TokenType.Identifier) &&
               Shift(tokens, out var shifted) &&
               CheckType(shifted, TokenType.Colon);
    }

    private static VariableDeclarationNode? ParseDeclaration(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var shifted = tokens;
        
        // TODO: Handle different type other than int.
        if (GetTokenAndConsume<KeywordToken>(ref shifted) is not { } keyword)
            return null;
        
        if (keyword.Keyword is not Keyword.Int)
            throw new FormatException($"Expected 'int' but found '{keyword.Keyword.ToStringFast()}'");

        if (GetTokenAndConsume<IdentifierToken>(ref shifted) is not { } id)
            return null;

        // If we find a parenthesis, then this is a function declaration.
        if (CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
            return null;
        
        tokens = shifted;

        if (GetTokenAndConsume<AssignmentToken>(ref tokens) is null )
        {
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
            return new VariableDeclarationNode(GetString(id, fileContent));            
        }
        
        var rhs = ParseExpression(ref tokens, fileContent);
        AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        return new VariableDeclarationNode(GetString(id, fileContent), rhs);
    }

    private static IStatementNode? ParseStatement(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var labels = ParseLabels(ref tokens, fileContent);
        if (ParseReturn(ref tokens, fileContent) is { } @return)
            return WrapStatement(@return, labels);
        
        if (ParseGoto(ref tokens, fileContent) is { } @goto)
            return WrapStatement(@goto, labels);
        
        if (ParseExpression(ref tokens, fileContent) is { } expression)
        {
            var expr = new ExpressionNode(expression);
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
            return WrapStatement(expr, labels);
        }

        if (ParseIf(ref tokens, fileContent) is { } @if)
            return WrapStatement(@if, labels);
        
        if (ParseSwitch(ref tokens, fileContent) is { } @switch)
            return WrapStatement(@switch, labels);
        
        if (ParseDefault(ref tokens, fileContent) is { } @default)
            return WrapStatement(@default, labels);
        
        if (ParseCase(ref tokens, fileContent) is { } @case)
            return WrapStatement(@case, labels);
        
        if (ParseCompound(ref tokens, fileContent) is { } compound)
            return WrapStatement(compound, labels);
        
        if (ParseWhile(ref tokens, fileContent) is { } @while)
            return WrapStatement(@while, labels);

        if (ParseDoWhile(ref tokens, fileContent) is { } doWhile)
        {
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
            return WrapStatement(doWhile, labels);            
        }
        
        if (ParseFor(ref tokens, fileContent) is { } @for) 
            return WrapStatement(@for, labels);

        if (ParseBreak(ref tokens) is { } @break)
        {
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
            return WrapStatement(@break, labels);
        }

        if (ParseContinue(ref tokens) is { } @continue)
        {
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
            return WrapStatement(@continue, labels);
        }
        
        if (CheckTypeAndConsume(tokens, TokenType.Semicolon, out tokens))
            return WrapStatement(NullNode.Statement, labels);
        
        // Note, in C23 this is supported.
        if (labels is not null)
            throw new FormatException("A label can only be part of a statement");

        return null;

        static IStatementNode WrapStatement(IStatementNode statement, List<string>? labels)
        {
            if (labels is null or { Count: 0 }) 
                return statement;
            
            var last = labels.Last();
            labels.RemoveAt(labels.Count - 1);            
            labels.Reverse();
            
            return labels.Aggregate(
                new LabelNode(last, statement), (acc, label) => new LabelNode(label, acc));
        }
    }
    
    private static DefaultNode? ParseDefault(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.Default, out tokens))
            return null;
        
        AssertTypeAndConsume(tokens, TokenType.Colon, fileContent.Span, out tokens);
        
        return ParseStatement(ref tokens, fileContent) is not { } statement 
            ? throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'") 
            : new DefaultNode(statement);
    }

    private static CaseNode? ParseCase(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.Case, out tokens))
            return null;

        if (ParseExpression(ref tokens, fileContent) is not { } expression)
            throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        
        AssertTypeAndConsume(tokens, TokenType.Colon, fileContent.Span, out tokens);
        
        return ParseStatement(ref tokens, fileContent) is not { } statement 
            ? throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'") 
            : new CaseNode(expression, statement);     
    }

    private static SwitchNode? ParseSwitch(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.Switch, out tokens))
            return null;
        
        var condition = ParseRequiredParenthesizedExpression(ref tokens, fileContent);
        
        if (ParseStatement(ref tokens, fileContent) is not { } body)
            throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        
        return new SwitchNode(condition, body);
    }

    private static ForNode? ParseFor(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.For, out tokens))
            return null;
        
        AssertTypeAndConsume(tokens, TokenType.OpenParenthesis, fileContent.Span, out tokens);

        IForLoopInitializer? init = ParseDeclaration(ref tokens, fileContent);

        if (init is null)
        {
            init = ParseExpression(ref tokens, fileContent);
            AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        }
     
        var condition = ParseExpression(ref tokens, fileContent);
        
        AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        
        var incrementOrDecrement = ParseExpression(ref tokens, fileContent);
        
        AssertTypeAndConsume(tokens, TokenType.CloseParenthesis, fileContent.Span, out tokens);

        return ParseStatement(ref tokens, fileContent) is not { } body
            ? throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'")
            : new ForNode(init, condition, incrementOrDecrement, body);
    }

    private static DoWhileNode? ParseDoWhile(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.Do, out tokens))
            return null;

        if (ParseStatement(ref tokens, fileContent) is not { } statement)
            throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'"); 
       
        AssertKeywordAndConsume(tokens, Keyword.While, fileContent.Span, out tokens);
        
        return ParseParenthesizedExpression(ref tokens, fileContent) is not { } condition 
            ? throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'") 
            : new DoWhileNode(statement, condition);
    }

    private static WhileNode? ParseWhile(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.While, out tokens))
            return null;           
        
        var condition = ParseRequiredParenthesizedExpression(ref tokens, fileContent);             

        return ParseStatement(ref tokens, fileContent) is not { } statement 
            ? throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'") 
            : new WhileNode(condition, statement);
    }

    private static BreakNode? ParseBreak(ref Span<IToken> tokens) =>
        CheckKeywordAndConsume(tokens, Keyword.Break, out tokens)
            ? new BreakNode()
            : null;
    
    private static ContinueNode? ParseContinue(ref Span<IToken> tokens) =>
        CheckKeywordAndConsume(tokens, Keyword.Continue, out tokens)
            ? new ContinueNode()
            : null;
    

    private static CompoundNode? ParseCompound(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckTypeAndConsume(tokens, TokenType.OpenBrace, out tokens))
            return null;
        
        List<IBlockItem> body = [];
        while (!CheckType(tokens, TokenType.CloseBrace))
        {
            if (ParseBlockItem(ref tokens, fileContent) is not { } item)
                break;
            
            body.Add(item);
        }                   
        AssertTypeAndConsume(tokens, TokenType.CloseBrace, fileContent.Span, out tokens);
        return new CompoundNode(new BlockNode(body));
    }

    private static GotoNode? ParseGoto(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.Goto, out tokens)) 
            return null;
        
        var identifier = AssertTokenAndConsume<IdentifierToken>(ref tokens, TokenType.Identifier);
        AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        return new GotoNode(GetString(identifier, fileContent));
    }

    private static IfNode? ParseIf(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.If, out tokens)) 
            return null;
        
        AssertTypeAndConsume(tokens, TokenType.OpenParenthesis, fileContent.Span, out tokens);
            
        if (ParseExpression(ref tokens, fileContent) is not { } condition)
            throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
        AssertTypeAndConsume(tokens, TokenType.CloseParenthesis, fileContent.Span, out tokens);

        if (ParseStatement(ref tokens, fileContent) is not { } statement)
            throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
        return new IfNode(condition, statement, ParseElse(ref tokens, fileContent));
    }
    
    private static IStatementNode? ParseElse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.Else, out tokens)) 
            return null;
        
        if (ParseIf(ref tokens, fileContent) is {} @if)
            return @if;
            
        if (ParseStatement(ref tokens, fileContent) is not { } statement)
            throw new FormatException($"Expected statement but found '{ReadTokenValue(tokens, fileContent.Span)}'");
            
        return statement;
    }

    private static ReturnNode? ParseReturn(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!CheckKeywordAndConsume(tokens, Keyword.Return, out tokens))
            return null;
        
        if (ParseExpression(ref tokens, fileContent) is not { } expression)
            throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        
        AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        return new ReturnNode(expression);
    }

    private static IExpressionNode CreateAssignmentNode(TokenType type, IExpressionNode lhs, IExpressionNode rhs)
        => type switch
        {
            TokenType.AdditionAssignment => new AdditionAssignmentNode(lhs, rhs),
            TokenType.SubtractionAssignment => new SubtractionAssignmentNode(lhs, rhs),
            TokenType.MultiplicationAssignment => new MultiplicationAssignmentNode(lhs, rhs),
            TokenType.DivisionAssignment => new DivisionAssignmentNode(lhs, rhs),
            TokenType.RemainderAssignment => new RemainderAssignmentNode(lhs, rhs),
            TokenType.BitwiseAndAssignment => new BitwiseAndAssignmentNode(lhs, rhs),
            TokenType.BitwiseOrAssignment => new BitwiseOrAssignmentNode(lhs, rhs),
            TokenType.BitwiseXorAssignment => new BitwiseXorAssignmentNode(lhs, rhs),
            TokenType.LeftShiftAssignment => new LeftShiftAssignmentNode(lhs, rhs),
            TokenType.RightShiftAssignment => new RightShiftAssignmentNode(lhs, rhs),
            TokenType.Assignment => new AssignmentNode(lhs, rhs),
            _ => throw new FormatException($"Expected assignment but found '{type.ToStringFast()}'")
        };

    private static IExpressionNode? ParseExpression(
        ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent, int precedence = 0)
    {
        var lhs = ParseFactor(ref tokens, fileContent);
        while (PeekOperator(ref tokens, out var op) && Precedence[op] >= precedence)
        {
            if (lhs is null)
                throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

            if (IsAssignment(op))
            {
                AssertTypeAndConsume(tokens, op, fileContent.Span, out tokens);
                
                if (ParseExpression(ref tokens, fileContent, Precedence[op]) is not { } rhs)
                    throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

                lhs = CreateAssignmentNode(op, lhs, rhs);
                continue;
            }
            if (IsConditional(op))
            {
                var middle = ParseConditionMiddle(ref tokens, fileContent);
                
                if (ParseExpression(ref tokens, fileContent, Precedence[op]) is not { } rhs)
                    throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
                
                lhs = new ConditionalNode(lhs, middle, rhs);
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

    private static IExpressionNode ParseConditionMiddle(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        AssertTypeAndConsume(tokens, TokenType.QuestionMark, fileContent.Span, out tokens);
        
        if (ParseExpression(ref tokens, fileContent) is not { } condition)
            throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        
        AssertTypeAndConsume(tokens, TokenType.Colon, fileContent.Span, out tokens);
        
        return condition;
    }
    
    private static bool IsConditional(TokenType op)
        => op is TokenType.QuestionMark;

    private static bool IsAssignment(TokenType op)
        => op is TokenType.AdditionAssignment
            or TokenType.SubtractionAssignment
            or TokenType.DivisionAssignment
            or TokenType.MultiplicationAssignment
            or TokenType.RemainderAssignment
            or TokenType.BitwiseAndAssignment
            or TokenType.BitwiseOrAssignment
            or TokenType.BitwiseXorAssignment
            or TokenType.LeftShiftAssignment
            or TokenType.RightShiftAssignment
            or TokenType.Assignment;

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
        if (CheckType(tokens, TokenType.AdditionAssignment))
            type = TokenType.AdditionAssignment;
        if (CheckType(tokens, TokenType.SubtractionAssignment))
            type = TokenType.SubtractionAssignment;
        if (CheckType(tokens, TokenType.DivisionAssignment))
            type = TokenType.DivisionAssignment;
        if (CheckType(tokens, TokenType.MultiplicationAssignment))
            type = TokenType.MultiplicationAssignment;
        if (CheckType(tokens, TokenType.RemainderAssignment))
            type = TokenType.RemainderAssignment;
        if (CheckType(tokens, TokenType.BitwiseAndAssignment))
            type = TokenType.BitwiseAndAssignment;
        if (CheckType(tokens, TokenType.BitwiseOrAssignment))
            type = TokenType.BitwiseOrAssignment;
        if (CheckType(tokens, TokenType.BitwiseXorAssignment))
            type = TokenType.BitwiseXorAssignment;
        if (CheckType(tokens, TokenType.LeftShiftAssignment))
            type = TokenType.LeftShiftAssignment;
        if (CheckType(tokens, TokenType.RightShiftAssignment))
            type = TokenType.RightShiftAssignment;
        if (CheckType(tokens, TokenType.QuestionMark))
            type = TokenType.QuestionMark;
        
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
        
        if (ParseFunctionCall(ref tokens, fileContent) is { } call)
            return call;
        
        if (ParseVariable(ref tokens, fileContent) is { } variable)
            return variable;

        return null;
    }

    private static FunctionCallNode? ParseFunctionCall(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var shifted = tokens;
        if (GetTokenAndConsume<IdentifierToken>(ref shifted) is not { } identifier)
            return null;
        
        if (!CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
            return null;
        
        var args = new List<IExpressionNode>(3);
        while (!CheckType(shifted, TokenType.CloseParenthesis))
        {
            CheckTypeAndConsume(shifted, TokenType.Comma, out shifted);
            
            if (ParseExpression(ref shifted, fileContent) is not { } arg)
                throw new FormatException($"Expected expression but found '{ReadTokenValue(shifted, fileContent.Span)}'");
            
            args.Add(arg);
        }
        
        AssertTypeAndConsume(shifted, TokenType.CloseParenthesis, fileContent.Span, out shifted);         
        tokens = shifted;
        
        return new FunctionCallNode(GetString(identifier, fileContent), args);        
    }

    private static VariableNode? ParseVariable(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var shifted = tokens;
        if (GetTokenAndConsume<IdentifierToken>(ref shifted) is not { } identifier)
            return null;
        
        if (CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
            return null;
        
        tokens = shifted;
        
        return new VariableNode(GetString(identifier, fileContent));
    }
    

    private static IExpressionNode? ParseParenthesizedExpression(ref Span<IToken> tokens,
        ReadOnlyMemory<char> fileContent)
    {        
        if (!CheckType(tokens, TokenType.OpenParenthesis)) 
            return null;

        if (!Shift(tokens, out var shifted))
            return null;

        if (ParseExpression(ref shifted, fileContent) is not { } expression)
            return null;
        
        AssertTypeAndConsume(shifted, TokenType.CloseParenthesis, fileContent.Span, out shifted);
        
        tokens = shifted;
        return expression;
    }
    
    private static IExpressionNode ParseRequiredParenthesizedExpression(ref Span<IToken> tokens,
        ReadOnlyMemory<char> fileContent)
    {        
        AssertTypeAndConsume(tokens, TokenType.OpenParenthesis, fileContent.Span, out var shifted);        

        if (ParseExpression(ref shifted, fileContent) is not { } expression)
            throw new  FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        
        AssertTypeAndConsume(shifted, TokenType.CloseParenthesis, fileContent.Span, out shifted);
        
        tokens = shifted;
        return expression;
    }
    
    private static UnaryNode? ParseUnary(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {           
        IExpressionNode? expr = null;
        var prefix = ParseUnaryOperator(ref tokens, postfix: false);
        
        if (prefix is not null)
        {
            if ((expr = ParseFactor(ref tokens, fileContent)) is null)
                throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");

            if (expr is not VariableNode)            
                return new UnaryNode(prefix, expr);            
        }

        if (expr is null && !IsPostfixUnary(tokens))
            return null;

        expr ??= ParseVariable(ref tokens, fileContent);
        expr ??= ParseParenthesizedExpression(ref tokens, fileContent);
        expr ??= ParseConstant<int>(ref tokens, fileContent);
        
        if (expr is null)
            throw new FormatException($"Expected expression but found '{ReadTokenValue(tokens, fileContent.Span)}'");
        
        while (ParseUnaryOperator(ref tokens, postfix: true) is { } postfix)
            expr = new UnaryNode(postfix, expr);
        
        return prefix is not null
            ? new UnaryNode(prefix, expr)
            : (UnaryNode)expr;    
    }
    
    private static bool IsPostfixUnary(in Span<IToken> tokens)
    {
        var shifted = tokens;
        var result = false;
        
        if (CheckType(shifted, TokenType.OpenParenthesis))
        {
            while (!CheckType(shifted, TokenType.CloseParenthesis) && Shift(shifted, out shifted)) { }

            if (!CheckType(shifted, TokenType.CloseParenthesis))
                return false;
            
            shifted = shifted[1..];
            result = true;
        }
        else if (CheckType(shifted, TokenType.NumericConstant) || CheckType(shifted, TokenType.Identifier))
        {
            shifted = shifted[1..];
            result = true;
        }        
        return result && shifted[0].Type is TokenType.Increment or TokenType.Decrement;
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
    
    private static IUnaryOperatorNode? ParseUnaryOperator(ref Span<IToken> tokens, bool postfix)
    {
        if (CheckTypeAndConsume(tokens, TokenType.Minus, out tokens))
            return NegateNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Complement, out tokens))
            return ComplementNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Not, out tokens))
            return NotNode.Operator;
        if (CheckTypeAndConsume(tokens, TokenType.Increment, out tokens))
            return GetIncrementOperator(postfix);
        if (CheckTypeAndConsume(tokens, TokenType.Decrement, out tokens))
            return GetDecrementOperator(postfix);
        
        return null;
        
        static IUnaryOperatorNode GetIncrementOperator(bool postfix) =>
            postfix ? PostfixIncrementNode.Operator : PrefixIncrementNode.Operator;
        
        static IUnaryOperatorNode GetDecrementOperator(bool postfix) =>
            postfix ? PostfixDecrementNode.Operator : PrefixDecrementNode.Operator;
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
    private static bool CheckKeyword(in Span<IToken> tokens, in Keyword keyword) => 
        !tokens.IsEmpty && 
        tokens[0] is KeywordToken token && 
        token.Keyword == keyword;

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
        Span<IToken> tokens, Keyword keyword, ReadOnlySpan<char> fileContent, out Span<IToken> shifted)
    {
        if (CheckKeywordAndConsume(tokens, keyword, out shifted))
            return;
        
        throw tokens.IsEmpty 
            ? new FormatException($"Missing '{keyword.ToStringFast(true)}'")
            : new FormatException($"Expected '{keyword.ToStringFast(true)}' but found '{ReadTokenValue(tokens, fileContent)}'");                    
    }

    private static bool CheckTypeAndConsume(Span<IToken> tokens, TokenType tokenType, out Span<IToken> shifted)
    {
        shifted = tokens;
        return CheckType(tokens, tokenType) && Shift(tokens, out shifted);
    }
    
    [Pure]
    private static bool CheckKeywordAndConsume(Span<IToken> tokens, Keyword keyword, out Span<IToken> shifted)
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