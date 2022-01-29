namespace Exev;

public interface ITokenValidationRule
{
    void Validate(SyntaxToken previousToken, SyntaxToken currentToken);
}
