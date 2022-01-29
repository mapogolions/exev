using Exev.Syntax;

namespace Exev;

public interface ICursor
{
    IReadOnlyList<SyntaxToken> Tokens { get; }
    SyntaxToken Current { get; }
    SyntaxToken Peek(int offset);
    SyntaxToken NextToken();
}
