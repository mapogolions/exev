using Exev.Syntax;

namespace Exev;

public class Cursor : ICursor
{
    private int _position;
    private readonly IReadOnlyList<SyntaxToken> _tokens;

    public Cursor(IReadOnlyList<SyntaxToken> tokens)
    {
        _tokens = tokens;
        _position = 0;
    }

    public IReadOnlyList<SyntaxToken> Tokens => _tokens;

    public SyntaxToken Current => Peek(0);

    public SyntaxToken Peek(int offset)
    {
        var index = _position + offset;
        if (index < 0) return _tokens[0];
        if (index >= _tokens.Count) return _tokens[_tokens.Count - 1];
        return _tokens[index];
    }

    public SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }
}
