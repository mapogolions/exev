using Exev.Syntax;

namespace Exev.Validation;

public interface ITokenValidationRule
{
    void Validate(SyntaxToken previousToken, SyntaxToken currentToken);
}
