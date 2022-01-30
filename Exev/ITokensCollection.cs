using Exev.Syntax;

namespace Exev;

public interface ITokensCollection : IEnumerable<SyntaxToken>
{
    SyntaxToken Current { get; }
    SyntaxToken Previous { get; }
    SyntaxToken Next { get; }
    SyntaxToken Peek(int offset);
    void MoveNext();
}
