using Xunit;

namespace Exev.Tests;

public class SyntaxTreeTest
{
    [Fact]
    public void ShouldSkipClimbUpIfSpecifiedExplicitly()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "(", null),
                precedence: 1,
                SyntaxNodeInfo.SkipClimbUp
            ));
        Assert.Equal("( 1 (", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpLeftAssocSyntaxNodeIfPrecedenceIsGreaterThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 11
            ));
        Assert.Equal("( 1 +", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpLefAssocSyntaxNodeIfPrecedenceIsEqualToCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 10
            ));
        Assert.Equal("( + 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpLefAssocSyntaxNodeIfPrecedenceIsLessThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 2
            ));
        Assert.Equal("( + 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldInsertNumberAsRightChildOfRoot()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "12", 12),
                precedence: 10
            ));
        Assert.Equal("( 12", tree.Traverse(Traversal.InOrder));
    }

    [Fact]
    public void ShouldReturnOpenParenthesis()
    {
        var tree = new SyntaxTree(Root);
        Assert.Equal("(", tree.Traverse(Traversal.InOrder));
    }

    private static SyntaxNode Root => new SyntaxNode(
        token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
        precedence: 1,
        metaInfo: SyntaxNodeInfo.SkipClimbUp
    );
}
