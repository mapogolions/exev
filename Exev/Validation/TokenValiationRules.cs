using System.Collections;
using Exev.Syntax;

namespace Exev.Validation;

public class TokenValiationRules : IEnumerable<ITokenValidationRule>
{
    public IEnumerator<ITokenValidationRule> GetEnumerator()
    {
        yield return new TokenValidationRule(
            null,
            tokens => tokens.Count() == 1 && tokens.Current.Kind != SyntaxKind.NumberToken,
            tokens => $"Invalid expression: {tokens.Current.Text}"
        );
        yield return new TokenValidationRule(
            kind: null,
            violation: tokens => !tokens.IsBalanced(
                token => token.Kind,
                (a, b) => a == b,
                (SyntaxKind.OpenParenthesisToken, SyntaxKind.CloseParenthesisToken)
            ),
            failure: tokens => $"Unbalanced expression. " +
                $"( - {tokens.Where(x => x.Kind == SyntaxKind.OpenParenthesisToken).Count()} " +
                $") - {tokens.Where(x => x.Kind == SyntaxKind.CloseParenthesisToken).Count()}"
        );
        yield return new TokenValidationRule(
            null,
            tokens => tokens.Count() > 1 &&
                (tokens.Current.Kind != SyntaxKind.OpenParenthesisToken
                    && tokens.Current.Kind != SyntaxKind.NumberToken
                    && tokens.Current.Kind != SyntaxKind.PlusToken
                    && tokens.Current.Kind != SyntaxKind.MinusToken),
            tokens => $"Invalid usage: {tokens.Current.Text}"
        );
        yield return new TokenValidationRule(
            SyntaxKind.EofToken,
            tokens => tokens.Count() > 1 && (tokens.Previous.Kind != SyntaxKind.NumberToken
                && tokens.Previous.Kind != SyntaxKind.CloseParenthesisToken
                && tokens.Previous.Kind != SyntaxKind.FactorialToken)
        );
        yield return new TokenValidationRule(
            SyntaxKind.NumberToken,
            tokens => tokens.Previous.Kind == SyntaxKind.CloseParenthesisToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.NumberToken,
            tokens => tokens.Previous.Kind == SyntaxKind.FactorialToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Previous.Kind == SyntaxKind.CloseParenthesisToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Previous.Kind == SyntaxKind.NumberToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Next.Kind == SyntaxKind.EofToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.CloseParenthesisToken,
            tokens => tokens.Previous.Kind == SyntaxKind.OpenParenthesisToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.AsteriskToken,
            tokens => tokens.Previous.Kind != SyntaxKind.NumberToken
                && tokens.Previous.Kind != SyntaxKind.CloseParenthesisToken
                && tokens.Previous.Kind != SyntaxKind.FactorialToken
        );
        yield return new TokenValidationRule(
            SyntaxKind.SlashToken,
            tokens => tokens.Previous.Kind != SyntaxKind.NumberToken
                && tokens.Previous.Kind != SyntaxKind.CloseParenthesisToken
                && tokens.Previous.Kind != SyntaxKind.FactorialToken
        );

        yield return new TokenValidationRule(
            SyntaxKind.ExponentToken,
            tokens => tokens.Previous.Kind != SyntaxKind.NumberToken
                && tokens.Previous.Kind != SyntaxKind.CloseParenthesisToken
                && tokens.Previous.Kind != SyntaxKind.FactorialToken
        );
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Validate(SyntaxKind? kind, ITokensCollection tokens)
    {
        var failures = new List<string>();
        foreach (var rule in this)
        {
            rule.Validate(kind, tokens);
        }
    }
}
