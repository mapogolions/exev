using Exev.Syntax;

namespace Exev.Validation;

public interface ITokenValidationRule
{
    void Validate(SyntaxKind? kind, ITokensCollection tokens);
}
