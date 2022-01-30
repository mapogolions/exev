using System.Collections;
using Exev.Syntax;

namespace Exev.Validation;

public class TokenValiationRules : IEnumerable<ITokenValidationRule>
{
    public IEnumerator<ITokenValidationRule> GetEnumerator()
    {
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Previous.Kind != SyntaxKind.CloseParenthesisToken,
            "Open parenthesis cannot be after close parenthesis"
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Previous.Kind != SyntaxKind.NumberToken,
            "Open parenthesis cannot be after number"
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            tokens => tokens.Next.Kind != SyntaxKind.EofToken,
            "Open parenthesis should not be the last token"
        );
        yield return new TokenValidationRule(
            SyntaxKind.CloseParenthesisToken,
            tokens => !object.ReferenceEquals(tokens.Previous, tokens.Current),
            "Close parenthesis can't be the first token"
        );
        yield return new TokenValidationRule(
            SyntaxKind.CloseParenthesisToken,
            tokens => tokens.Previous.Kind != SyntaxKind.OpenParenthesisToken,
            "Close parethesis afer open"
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
