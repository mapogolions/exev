using Exev.Syntax;
using Exev.Validation;

namespace Exev;

public class Parser : IParser
{
    private readonly ICursor _ptr;
    private readonly ILexer _lexer;
    private readonly TokenValiationRules _tokenValidationRules = new TokenValiationRules();

    public Parser(ILexer lexer)
    {
        _lexer = lexer;
        _ptr = new Cursor(GetSyntaxTokens());
    }

    public SyntaxTree Parse()
    {
        var tokens = _ptr.Tokens;
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
            if (_ptr.Current.Kind == SyntaxKind.EofToken) break;
            if (TryMatch(SyntaxKind.OpenParenthesisToken, out var token))
                currentNode = new SyntaxNode(token!, 1, SyntaxNodeInfo.SkipClimbUp);
            else if (TryMatch(SyntaxKind.CloseParenthesisToken, out token))
                currentNode = new SyntaxNode(token!, 1, SyntaxNodeInfo.RightAssoc);
            else if (TryMatch(SyntaxKind.NumberToken, out token))
                currentNode = new SyntaxNode(token!, 10);
            else if (TryMatch(SyntaxKind.PlusToken, out token))
                currentNode = new SyntaxNode(token!, 2);
            else
                throw new TokenValidationException($"Unexpected token {_ptr.Current.Text} was found");
            tree.Insert(currentNode);
        }
        return tree;
    }

    private bool TryMatch(SyntaxKind kind, out SyntaxToken? token)
    {
        if (_ptr.Current.Kind == kind)
        {
            var previousToken = _ptr.Peek(-1);
            token = _ptr.NextToken();
            _tokenValidationRules.Validate(previousToken, token!);
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
