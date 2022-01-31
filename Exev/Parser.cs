using Exev.Syntax;
using Exev.Validation;

namespace Exev;

public class Parser : IParser
{
    private readonly ITokensCollection _tokens;
    private readonly ILexer _lexer;
    private readonly TokenValiationRules _validationRules;

    public Parser(ILexer lexer)
    {
        _lexer = lexer;
        _tokens = new TokensCollection(GetSyntaxTokens().ToList());
        _validationRules = new TokenValiationRules();
    }

    public SyntaxTree Parse()
    {
        var tree = new SyntaxTree(
            new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                assoc: Assoc.None
            )
        );
        if (!_tokens.Any()) return tree;
        _validationRules.Validate(null, _tokens);
        SyntaxNode? currentNode;
        while (true)
        {
            _validationRules.Validate(_tokens.Current.Kind, _tokens);
            if (TryMatch(SyntaxKind.OpenParenthesisToken, out var token))
                currentNode = new SyntaxNode(token!, 1, SyntaxKind.PrecedenceOperator, Assoc.None);
            else if (TryMatch(SyntaxKind.CloseParenthesisToken, out token))
                currentNode = new SyntaxNode(token!, 1, SyntaxKind.PrecedenceOperator, Assoc.Right);
            else if (TryMatch(SyntaxKind.NumberToken, out token))
                currentNode = new SyntaxNode(token!, 10, SyntaxKind.NumberExpression);
            else if (TryMatch(SyntaxKind.AsteriskToken, out token))
                currentNode = new SyntaxNode(token!, 4, SyntaxKind.BinaryOperator);
            else if (TryMatch(SyntaxKind.SlashToken, out token))
                currentNode = new SyntaxNode(token!, 4, SyntaxKind.BinaryOperator);
            else if (TryMatch(SyntaxKind.FactorialToken, out token))
                currentNode = new SyntaxNode(token!, 6, SyntaxKind.UnaryOperator);
            else if (TryMatch(SyntaxKind.ExponentToken, out token))
                currentNode = new SyntaxNode(token!, 5, SyntaxKind.BinaryOperator, Assoc.Right);
            else if (TryMatch(SyntaxKind.LiteralToken, out token))
                currentNode = new SyntaxNode(token!, 10, SyntaxKind.CallOperator);
            else if (TryMatch(SyntaxKind.PlusToken, out token) || TryMatch(SyntaxKind.MinusToken, out token))
            {
                var kind = _tokens.Previous.Kind;
                if (kind == SyntaxKind.NumberToken || kind == SyntaxKind.CloseParenthesisToken)
                    currentNode = new SyntaxNode(token!, 2, SyntaxKind.BinaryOperator);
                else
                    currentNode = new SyntaxNode(token!, 3, SyntaxKind.UnaryOperator, Assoc.None);
    		}
            else
                throw new TokenValidationException($"Unexpected token {_tokens.Current.Text} was found");
            tree.Insert(currentNode);
            if (!_tokens.MoveNext()) break;
        }
        return tree;
    }

    private bool TryMatch(SyntaxKind kind, out SyntaxToken? token)
    {
        if (_tokens.Current.Kind == kind)
        {
            token = _tokens.Current;
            return true;
        }
        token = null;
        return false;
    }

    private IEnumerable<SyntaxToken> GetSyntaxTokens()
    {
        SyntaxToken? token;
        while (true)
        {
            token = _lexer.NextToken();
            if (token.Kind == SyntaxKind.EofToken) yield break;
            if (token.Kind == SyntaxKind.SpaceToken) continue;
            yield return token;
        }
    }
}
