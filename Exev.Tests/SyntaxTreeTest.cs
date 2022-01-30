using Exev.Syntax;
using Xunit;

namespace Exev.Tests;

public class SyntaxTreeTest
{
    [Fact]
    public void ShouldNotRemoveRoot()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.CloseParenthesisToken, -1, ")", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                assoc: Assoc.Right
            ));
        Assert.Equal("(", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldRemoveOpenParenthesis()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                assoc: Assoc.None
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.CloseParenthesisToken, -1, ")", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                assoc: Assoc.Right
            ));
        Assert.Equal("(", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpRightAssocSyntaxNodeIfPrecedenceIsEqualToCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 1,
                kind: SyntaxKind.UnaryOperator,
                assoc: Assoc.Right
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.PlusToken, -1, "-", null),
                precedence: 1,
                kind: SyntaxKind.UnaryOperator,
                assoc: Assoc.Right
            ));
        Assert.Equal("( + -", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpRightAssocSyntaxNodeIfPrecedenceIsGreaterThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.PlusToken, -1, "-", null),
                precedence: 11,
                kind: SyntaxKind.UnaryOperator,
                assoc: Assoc.Right
            ));
        Assert.Equal("( 1 -", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpRightAssocSyntaxNodeIfPrecedenceIsLessThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.MinusToken, -1, "-", null),
                precedence: 2,
                kind: SyntaxKind.UnaryOperator,
                assoc: Assoc.Right
            ));
        Assert.Equal("( - 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpIfSpecifiedExplicitly()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                kind: SyntaxKind.NumberExpression,
                precedence: 10
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "(", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                Assoc.None
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
                kind: SyntaxKind.BinaryOperator
            ))
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression
            ));
        Assert.Equal("( + 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpLefAssocSyntaxNodeIfPrecedenceIsEqualToCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "2", 2),
                precedence: 10,
                kind: SyntaxKind.NumberExpression
            ));
        Assert.Equal("( 2 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpLefAssocSyntaxNodeIfPrecedenceIsLessThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 2,
                kind: SyntaxKind.BinaryOperator
            ));
        Assert.Equal("( + 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldInsertNumberAsRightChildOfRoot()
    {
        var tree = new SyntaxTree(Root)
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "12", 12),
                precedence: 10,
                kind: SyntaxKind.NumberExpression
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
        kind: SyntaxKind.PrecedenceOperator,
        assoc: Assoc.None
    );
}
