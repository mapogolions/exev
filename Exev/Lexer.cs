using Exev.Syntax;

namespace Exev;

public class Lexer : ILexer
{
    private readonly string _source;
    private int _position;

    public Lexer(string source)
    {
        _source = source;
    }

    public SyntaxToken NextToken()
    {
        if (Current == '\0')
        {
            return new SyntaxToken(SyntaxKind.EofToken, _position, "\0", null);
        }
        if (char.IsDigit(Current))
        {
            var start = _position;
            while (char.IsDigit(Current)) _position++;
            var text = _source.Substring(start, _position - start);
            var value = int.Parse(text);
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }
        if (char.IsWhiteSpace(Current))
        {
            var start = _position;
            while (char.IsWhiteSpace(Current)) _position++;
            var text = _source.Substring(start, _position - start);
            return new SyntaxToken(SyntaxKind.SpaceToken, start, text, null);
        }
        if (Current == '(')
            return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
        if (Current == ')')
            return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
        if (Current == '*')
            return new SyntaxToken(SyntaxKind.AsteriskToken, _position++, "*", null);
        if (Current == '/')
            return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
        if (Current == '+')
            return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
        if (Current == '-')
            return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
        if (Current == '.')
            return new SyntaxToken(SyntaxKind.DotToken, _position++, ".", null);
        return new SyntaxToken(SyntaxKind.BadToken, _position++,
            _source.Substring(_position - 1, 1), null);
    }

    private char Current
    {
        get
        {
            if (_position >= _source.Length) return '\0';
            return _source[_position];
        }
    }
}
