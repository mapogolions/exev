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
                token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                precedence: 1,
                metaInfo: SyntaxNodeInfo.SkipClimbUp
            )
        );
        SyntaxNode? currentNode = tree.Root;
        SyntaxToken? previousToken = null;
        while (true)
        {
            if (Current.Kind == SyntaxKind.BadToken) throw new InvalidDataException();
            if (Current.Kind == SyntaxKind.EofToken) break;
            if (TryMatch(SyntaxKind.SpaceToken, out _)) continue;
            if (TryMatch(SyntaxKind.OpenParenthesisToken, out var token))
            {
                previousToken = Peek(-1);
                if (previousToken.Kind == SyntaxKind.CloseParenthesisToken)
                    throw new InvalidDataException("Open bracket cannot be after close bracket");
                if (previousToken.Kind == SyntaxKind.NumberToken)
                    throw new InvalidDataException("Open bracket cannot be after number");
                currentNode = new SyntaxNode(token!, 1, SyntaxNodeInfo.SkipClimbUp);
            }
            else throw new InvalidDataException();
            tree.Insert(currentNode);
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
        if (index < 0) return _tokens.Value[0];
        if (index >= _tokens.Value.Count) return _tokens.Value[_tokens.Value.Count - 1];
        return _tokens.Value[index];
    }
}
