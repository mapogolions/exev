using Exev.Syntax;

namespace Exev.Validation;

public class TokenValidationRule : ITokenValidationRule
{
    public TokenValidationRule(SyntaxKind? kind, Func<ITokensCollection, bool> violation,
        Func<ITokensCollection, string> failure)
    {
        Kind = kind;
        Violation = violation;
        Failure = failure;
    }

    public TokenValidationRule(SyntaxKind? kind, Func<ITokensCollection, bool> violation)
        : this (kind, violation,
        tokens => $"Unexpected {tokens.Current.Kind} follows {tokens.Previous.Kind}: {tokens.Previous.Text}{tokens.Current.Text}") { }

    public SyntaxKind? Kind { get; }

    public Func<ITokensCollection, bool> Violation { get; }

    public Func<ITokensCollection, string> Failure { get; }


    public void Validate(SyntaxKind? kind, ITokensCollection tokens)
    {
        if(Kind == kind)
        {
            if (Violation.Invoke(tokens))
            {
                throw new TokenValidationException(Failure(tokens));
            }
        }
    }
}
