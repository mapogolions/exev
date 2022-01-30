using System.Collections;
using Exev.Syntax;

namespace Exev.Validation;

public class TokenValiationRules : IEnumerable<ITokenValidationRule>
{
    public IEnumerator<ITokenValidationRule> GetEnumerator()
    {
        yield return new TokenValidationRule(
            null,
            tokens => tokens.Count() == 2 && tokens.Current.Kind != SyntaxKind.NumberToken,
            tokens => $"Invalid expression: {tokens.Current.Text}"
        );
        yield return new TokenValidationRule(
            null,
            tokens => !IsBalanced(tokens, (SyntaxKind.OpenParenthesisToken, SyntaxKind.CloseParenthesisToken)),
            tokens => $"Unbalanced expression. " +
                $"( - {tokens.Where(x => x.Kind == SyntaxKind.OpenParenthesisToken).Count()} " +
                $") - {tokens.Where(x => x.Kind == SyntaxKind.CloseParenthesisToken).Count()}"
        );
        yield return new TokenValidationRule(
            null,
            tokens => tokens.Count() > 2 &&
                (tokens.Current.Kind != SyntaxKind.OpenParenthesisToken
                    && tokens.Current.Kind != SyntaxKind.NumberToken
                    && tokens.Current.Kind != SyntaxKind.PlusToken
                    && tokens.Current.Kind != SyntaxKind.MinusToken),
            tokens => $"Invalid usage: {tokens.Current.Text}"
        );
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

    private static bool IsBalanced(IEnumerable<SyntaxToken> tokens, (SyntaxKind, SyntaxKind) kinds)
    {
        var i = 0;
        foreach (var token in tokens)
        {
            if (kinds.Item1 == token.Kind)
            {
                i++;
                continue;
            }
            if (kinds.Item2 == token.Kind && (--i) < 0)
            {
                return false;
            }
        }
        return i == 0;
    }
}
