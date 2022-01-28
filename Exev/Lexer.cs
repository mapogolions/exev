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
            return new SyntaxToken(SyntaxKind.Number, start, text);

        }
        throw new NotImplementedException();
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
