using Xunit;

namespace Exev.Tests;

public class SyntaxTreeTest
{
    [Fact]
    public void ShouldRemoveOpenParenthesis()
    { // + - 1
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                precedence: 1,
                metaInfo: SyntaxNodeInfo.SkipClimbUp
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.CloseParenthesisToken, -1, ")", null),
                precedence: 1,
                metaInfo: SyntaxNodeInfo.RightAssoc
            ));
        Assert.Equal("(", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpRightAssocSyntaxNodeIfPrecedenceIsEqualToCurrentNodePrecedence()
    { // + - 1
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "+", null),
                precedence: 1,
                metaInfo: SyntaxNodeInfo.RightAssoc
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "-", null),
                precedence: 1,
                metaInfo: SyntaxNodeInfo.RightAssoc
            ));
        Assert.Equal("( + -", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpRightAssocSyntaxNodeIfPrecedenceIsGreaterThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "-", null),
                precedence: 11,
                metaInfo: SyntaxNodeInfo.RightAssoc
            ));
        Assert.Equal("( 1 -", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpRightAssocSyntaxNodeIfPrecedenceIsLessThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "-", null),
                precedence: 2,
                metaInfo: SyntaxNodeInfo.RightAssoc
            ));
        Assert.Equal("( - 1", tree.Traverse(Traversal.PreOrder));
    }

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
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 1,
                metaInfo: SyntaxNodeInfo.LeftAssoc
            ))
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10
            ));
        Assert.Equal("( + 1", tree.Traverse(Traversal.PreOrder));
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
                new SyntaxToken(SyntaxKind.PlusToken, -1, "2", 2),
                precedence: 10,
                metaInfo: SyntaxNodeInfo.LeftAssoc
            ));
        Assert.Equal("( 2 1", tree.Traverse(Traversal.PreOrder));
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
