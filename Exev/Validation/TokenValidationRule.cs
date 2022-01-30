using Exev.Syntax;

namespace Exev.Validation;

public class TokenValidationRule : ITokenValidationRule
{
    public TokenValidationRule(SyntaxKind kind, Func<ITokensCollection, bool> validation,
        string failureMessage = "")
    {
        Kind = kind;
        Validation = validation;
        FailureMessage = failureMessage;
    }

    public TokenValidationRule(SyntaxKind kind, Func<ITokensCollection, bool> validation)
        : this(kind, validation, $"Unexpected {kind}") { }

    public SyntaxKind Kind { get; }

    public Func<ITokensCollection, bool> Validation { get; }

    public string FailureMessage { get; }


    public void Validate(ITokensCollection tokens)
    {
        if(Kind == tokens.Current.Kind)
        {
            if (!Validation.Invoke(tokens))
            {
                throw new TokenValidationException(FailureMessage);
            }
        }
    }
}
