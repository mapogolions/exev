using System.Collections;
using Exev.Syntax;

namespace Exev.Validation;

public class TokenValiationRules : IEnumerable<ITokenValidationRule>
{
    public IEnumerator<ITokenValidationRule> GetEnumerator()
    {
        yield return new TokenValidationRule(
            SyntaxKind.NumberToken,
            tokens => tokens.Previous.Kind != SyntaxKind.CloseParenthesisToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.NumberToken,
            tokens => tokens.Previous.Kind != SyntaxKind.FactorialToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Previous.Kind != SyntaxKind.CloseParenthesisToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Previous.Kind != SyntaxKind.NumberToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Next.Kind != SyntaxKind.EofToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.CloseParenthesisToken,
            tokens => !object.ReferenceEquals(tokens.Previous, tokens.Current)
        );
        yield return new TokenValidationRule(
            SyntaxKind.CloseParenthesisToken,
            tokens => tokens.Previous.Kind != SyntaxKind.OpenParenthesisToken
        );
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Validate(ITokensCollection tokens)
    {
        var failures = new List<string>();
        foreach (var rule in this)
        {
            rule.Validate(tokens);
        }
    }
}
