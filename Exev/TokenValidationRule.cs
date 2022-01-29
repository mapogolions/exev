namespace Exev;

public class TokenValidationRule : ITokenValidationRule
{
    public TokenValidationRule(SyntaxKind kind, Func<SyntaxToken, SyntaxToken, bool> validation,
        string failureMessage)
    {
        Kind = kind;
        Validation = validation;
        FailureMessage = failureMessage;
    }

    public SyntaxKind Kind { get; }

    public Func<SyntaxToken, SyntaxToken, bool> Validation { get; }

    public string FailureMessage { get; }


    public void Validate(SyntaxToken previousToken, SyntaxToken currentToken)
    {
        if(Kind == currentToken.Kind)
        {
            if (!Validation.Invoke(previousToken, currentToken))
            {
                throw new TokenValidationException(FailureMessage);
            }
        }
    }
}
