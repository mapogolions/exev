using Exev.Syntax;

namespace Exev.Validation;

public class TokenValidationRule : ITokenValidationRule
{
    public TokenValidationRule(SyntaxKind kind, Func<ITokensCollection, bool> validation,
        Func<ITokensCollection, string> failure)
    {
        Kind = kind;
        Validation = validation;
        Failure = failure;
    }

    public TokenValidationRule(SyntaxKind kind, Func<ITokensCollection, bool> validation)
        : this (kind, validation,
        tokens => $"Unexpected {tokens.Current.Kind} follows {tokens.Previous.Kind}: {tokens.Previous.Text}{tokens.Current.Text}") { }

    public SyntaxKind Kind { get; }

    public Func<ITokensCollection, bool> Validation { get; }

    public Func<ITokensCollection, string> Failure { get; }


    public void Validate(ITokensCollection tokens)
    {
        if(Kind == tokens.Current.Kind)
        {
            if (!Validation.Invoke(tokens))
            {
                throw new TokenValidationException(Failure(tokens));
            }
        }
    }
}
