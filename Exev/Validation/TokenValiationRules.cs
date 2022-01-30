using System.Collections;
using Exev.Syntax;

namespace Exev.Validation;

public class TokenValiationRules : IEnumerable<ITokenValidationRule>
{
    public IEnumerator<ITokenValidationRule> GetEnumerator()
    {
        // check single element token collection
        yield return new TokenValidationRule(
            null,
            tokens => tokens.Count() == 2 && tokens.Current.Kind != SyntaxKind.NumberToken
        );
        // check start
        yield return new TokenValidationRule(
            null,
            tokens => tokens.Count() > 2 &&
                (tokens.Current.Kind != SyntaxKind.OpenParenthesisToken
                    && tokens.Current.Kind != SyntaxKind.NumberToken
                    && tokens.Current.Kind != SyntaxKind.PlusToken
                    && tokens.Current.Kind != SyntaxKind.MinusToken)
        );
        // check end
        yield return new TokenValidationRule(
            SyntaxKind.EofToken,
            tokens => tokens.Count() > 2 && (tokens.Previous.Kind != SyntaxKind.NumberToken
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
