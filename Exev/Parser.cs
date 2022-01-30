using Exev.Syntax;
using Exev.Validation;

namespace Exev;

public class Parser : IParser
{
    private readonly ITokensCollection _tokens;
    private readonly ILexer _lexer;
    private readonly TokenValiationRules _tokenValidationRules;

    public Parser(ILexer lexer)
    {
        _lexer = lexer;
        _tokens = new TokensCollection(GetSyntaxTokens());
        _tokenValidationRules = new TokenValiationRules();
    }

    public SyntaxTree Parse()
    {
        var tree = new SyntaxTree(
            new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                precedence: 1,
                metaInfo: SyntaxNodeInfo.SkipClimbUp
            )
        );
        SyntaxNode? currentNode = null;
        while (true)
        {
            _tokenValidationRules.Validate(_tokens);
            if (_tokens.Current.Kind == SyntaxKind.EofToken) break;
            if (TryMatch(SyntaxKind.OpenParenthesisToken, out var token))
                currentNode = new SyntaxNode(token!, 1, SyntaxNodeInfo.SkipClimbUp);
            else if (TryMatch(SyntaxKind.CloseParenthesisToken, out token))
                currentNode = new SyntaxNode(token!, 1, SyntaxNodeInfo.RightAssoc);
            else if (TryMatch(SyntaxKind.NumberToken, out token))
                currentNode = new SyntaxNode(token!, 10);
            else if (TryMatch(SyntaxKind.PlusToken, out token))
                currentNode = new SyntaxNode(token!, 2);
            else
                throw new TokenValidationException($"Unexpected token {_tokens.Current.Text} was found");
            tree.Insert(currentNode);
            _tokens.MoveNext();
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

    private IReadOnlyList<SyntaxToken> GetSyntaxTokens()
    {
        var tokens = new List<SyntaxToken>();
        var token = _lexer.NextToken();
        if (token.Kind != SyntaxKind.SpaceToken)
        {
            tokens.Add(token);
        }
        while (token.Kind != SyntaxKind.EofToken)
        {
            token = _lexer.NextToken();
            if (token.Kind != SyntaxKind.SpaceToken)
            {
                tokens.Add(token);
            }
        }
        return tokens;
    }
}
