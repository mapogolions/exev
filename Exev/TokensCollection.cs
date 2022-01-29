using System.Collections;
using Exev.Syntax;

namespace Exev;

public class TokensCollection : ITokensCollection
{
    private int _position;
    private readonly IReadOnlyList<SyntaxToken> _tokens;

    public TokensCollection(IReadOnlyList<SyntaxToken> tokens)
    {
        _tokens = tokens;
        _position = 0;
    }

    public SyntaxToken Current => Peek(0);
    public SyntaxToken Previous => Peek(-1);
    public SyntaxToken Next => Peek(1);

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

    public IEnumerator<SyntaxToken> GetEnumerator() => _tokens.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
