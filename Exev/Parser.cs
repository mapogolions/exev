namespace Exev;

public class Parser : IParser
{
    private int _position;
    private readonly Lazy<IList<SyntaxToken>> _tokens;
    private readonly ILexer _lexer;


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
                new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                10,
                SyntaxNodeInfo.SkipClimbUp
            )
        );
        while (true)
        {
            SyntaxNode? node = null;
            if (Current.Kind == SyntaxKind.EofToken) break;
            if (TryMatch(SyntaxKind.SpaceToken, out var token)) continue;
            if (TryMatch(SyntaxKind.OpenParenthesisToken, out token))
                node = new SyntaxNode(token!, 1, SyntaxNodeInfo.SkipClimbUp);
            else if (TryMatch(SyntaxKind.CloseParenthesisToken, out token))
                node = new SyntaxNode(token!, 1, SyntaxNodeInfo.ClimbUp);
            else if (TryMatch(SyntaxKind.AsteriskToken, out token))
                node = new SyntaxNode(token!, 4, SyntaxNodeInfo.SkipClimbUp);
            else if (TryMatch(SyntaxKind.SlashToken, out token))
                node = new SyntaxNode(token!, 4, SyntaxNodeInfo.SkipClimbUp);
            else if (TryMatch(SyntaxKind.NumberToken, out token))
                node = new SyntaxNode(token!, 10, SyntaxNodeInfo.None);
            else throw new InvalidOperationException();
        }
        return tree;
    }

    private bool TryMatch(SyntaxKind kind, out SyntaxToken? token)
    {
        if (Current.Kind == kind)
        {
            token = GetCurrentAndMoveToNext();
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
        tokens.Add(token);
        while (token.Kind != SyntaxKind.EofToken)
        {
            token = _lexer.NextToken();
            tokens.Add(token);
        }
        return tokens;
    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;
        if (index >= _tokens.Value.Count)
        {
            return _tokens.Value[_tokens.Value.Count - 1];
        }
        return _tokens.Value[index];
    }
}
