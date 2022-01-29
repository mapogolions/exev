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
        throw new NotImplementedException();
    }

    private SyntaxToken Match(SyntaxKind kind)
    {
        if (Current.Kind == kind)
        {
            return GetCurrentAndMoveToNext();
        }
        throw new NotImplementedException();
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
        while (token.Kind != SyntaxKind.EofToken)
        {
            tokens.Add(token);
            token = _lexer.NextToken();
        }
        return tokens;
    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken Peek(int offset )
    {
        var index = _position + offset;
        if (index >= _tokens.Value.Count)
        {
            return _tokens.Value[_tokens.Value.Count - 1];
        }
        return _tokens.Value[index];
    }
}
