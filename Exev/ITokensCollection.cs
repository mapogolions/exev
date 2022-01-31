using Exev.Syntax;
using System.Collections.Generic;

namespace Exev;

public interface ITokensCollection : IEnumerable<SyntaxToken>
{
    SyntaxToken Current { get; }
    SyntaxToken Previous { get; }
    SyntaxToken Next { get; }
    SyntaxToken Peek(int offset);
    bool MoveNext();
}
