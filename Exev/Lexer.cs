namespace Exev;

public class Lexer
{
    private readonly string _source;
    private int _position;

    public Lexer(string source)
    {
        _source = source;
    }

    public SyntaxToken NextToken()
    {
        if (char.IsDigit(Current))
        {
            var start = _position;
            while (char.IsDigit(Current)) Next();
            var text = _source.Substring(start, _position - start);
            var value = int.Parse(text);
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);

        }
        if (char.IsWhiteSpace(Current))
        {
            var start = _position;
            while (char.IsWhiteSpace(Current)) Next();
            var text = _source.Substring(start, _position - start);
            return new SyntaxToken(SyntaxKind.SpaceToken, start, text, null);
        }
        return new SyntaxToken(SyntaxKind.EofToken, _position, "\0", null);
    }

    private char Current
    {
        get
        {
            if (_position >= _source.Length) return '\0';
            return _source[_position];
        }
    }

    private void Next() => _position++;
}
