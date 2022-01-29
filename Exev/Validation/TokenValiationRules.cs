using System.Collections;
using Exev.Syntax;

namespace Exev.Validation;

public class TokenValiationRules : IEnumerable<ITokenValidationRule>
{
    public IEnumerator<ITokenValidationRule> GetEnumerator()
    {
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            (previousToken, _) => previousToken.Kind != SyntaxKind.CloseParenthesisToken,
            "Open bracket cannot be after close bracket"
        );
        yield return new TokenValidationRule(
            SyntaxKind.OpenParenthesisToken,
            (previousToken, ct) => previousToken.Kind != SyntaxKind.NumberToken,
            "Open bracket cannot be after number"
        );
        // yield return new TokenValidationRule(
        //     SyntaxKind.OpenParenthesisToken,
        //     (previousToken, _) => ,
        //     "Close parenthesis should not be the last token"
        // );
        yield return new TokenValidationRule(
            SyntaxKind.CloseParenthesisToken,
            (previousToken, currentToken) => !object.ReferenceEquals(previousToken, currentToken),
            "Close bracket can't be the first token"
        );
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
