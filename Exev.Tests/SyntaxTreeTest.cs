using Xunit;

namespace Exev.Tests;

public class SyntaxTreeTest
{
    [Fact]
    public void ShouldReturnOpenParenthesis()
    {
        var tree = new SyntaxTree(Root);
        Assert.Equal("(", tree.Traverse(Traversal.InOrder));
    }

    private static SyntaxNode Root => new SyntaxNode(
        new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
        1,
        SyntaxNodeInfo.SkipClimbUp
    );
}
