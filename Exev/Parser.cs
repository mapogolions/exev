namespace Exev;

public class Parser : IParser
{
    private int _position;
    private readonly Lazy<IList<SyntaxToken>> _tokens;
    private readonly ILexer _lexer;
    private readonly TokenValiationRules _tokenValidationRules = new TokenValiationRules();

    public Parser(ILexer lexer)
    {
        _lexer = lexer;
        _tokens = new(GetSyntaxTokens);
    }

    public SyntaxTree Parse()
    {
        var tokens = _tokens.Value;
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
            if (Current.Kind == SyntaxKind.EofToken) break;
            if (TryMatch(SyntaxKind.OpenParenthesisToken, out var token))
                currentNode = new SyntaxNode(token!, 1, SyntaxNodeInfo.SkipClimbUp);
            else if (TryMatch(SyntaxKind.CloseParenthesisToken, out token))
                currentNode = new SyntaxNode(token!, 1, SyntaxNodeInfo.RightAssoc);
            else if (TryMatch(SyntaxKind.NumberToken, out token))
                currentNode = new SyntaxNode(token!, 10);
            else
                throw new TokenValidationException($"Unexpected token {Current.Text} was found");
            tree.Insert(currentNode);
        }
        return tree;
    }

    private bool TryMatch(SyntaxKind kind, out SyntaxToken? token)
    {
        if (Current.Kind == kind)
        {
            var previousToken = Peek(-1);
            token = GetCurrentAndMoveToNext();
            _tokenValidationRules.Validate(previousToken, token!);
            return true;
        }
        token = null;
        return false;
    }

    private SyntaxToken GetCurrentAndMoveToNext()
    {
        var current = Current;
        _position++;
        return current;
    }

    private IList<SyntaxToken> GetSyntaxTokens()
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

    private SyntaxToken Current => Peek(0);

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;
        if (index < 0) return _tokens.Value[0];
        if (index >= _tokens.Value.Count) return _tokens.Value[_tokens.Value.Count - 1];
        return _tokens.Value[index];
    }
}
