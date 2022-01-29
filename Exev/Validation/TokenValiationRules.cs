using System.Collections;
using Exev.Syntax;

namespace Exev.Validation;

public class TokenValiationRules : IEnumerable<ITokenValidationRule>
{
    public IEnumerator<ITokenValidationRule> GetEnumerator()
    {
        yield return new TokenValidationRule(SyntaxKind.OpenParenthesisToken,
            (pt, ct) => pt.Kind != SyntaxKind.CloseParenthesisToken, "Open bracket cannot be after close bracket");
        yield return new TokenValidationRule(SyntaxKind.OpenParenthesisToken,
            (pt, ct) => pt.Kind != SyntaxKind.NumberToken, "Open bracket cannot be after number");
        // yield return new TokenValidationRule(SyntaxKind.EofToken,
        //     (pt, ct) => (pt.Kind == SyntaxKind.NumberToken || pt.Kind == SyntaxKind.CloseParenthesisToken), "Open bracket cannot be after number");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Validate(SyntaxToken previousToken, SyntaxToken currentToken)
    {
        var failures = new List<string>();
        foreach (var rule in this)
        {
            rule.Validate(previousToken, currentToken);
        }
    }
}
