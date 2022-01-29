using Exev.Syntax;

namespace Exev;

public interface ILexer
{
    SyntaxToken NextToken();
}
