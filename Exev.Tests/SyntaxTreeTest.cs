using Exev.Syntax;
using Xunit;

namespace Exev.Tests;

public class SyntaxTreeTest
{
    [Fact]
    public void ShouldRemoveOpenParenthesis()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                climbUpStrategy: ClimbUpStrategy.Skip
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.CloseParenthesisToken, -1, ")", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                climbUpStrategy: ClimbUpStrategy.Lt
            ));
        Assert.Equal("", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpRightAssocSyntaxNodeIfPrecedenceIsEqualToCurrentNodePrecedence()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 1,
                kind: SyntaxKind.UnaryOperator,
                climbUpStrategy: ClimbUpStrategy.Lt
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.PlusToken, -1, "-", null),
                precedence: 1,
                kind: SyntaxKind.UnaryOperator,
                climbUpStrategy: ClimbUpStrategy.Lt
            ));
        Assert.Equal("+ -", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpRightAssocSyntaxNodeIfPrecedenceIsGreaterThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression,
                climbUpStrategy: ClimbUpStrategy.Lte
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.PlusToken, -1, "-", null),
                precedence: 11,
                kind: SyntaxKind.UnaryOperator,
                climbUpStrategy: ClimbUpStrategy.Lt
            ));
        Assert.Equal("1 -", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpRightAssocSyntaxNodeIfPrecedenceIsLessThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression,
                climbUpStrategy: ClimbUpStrategy.Lte
            ))
            .Insert(new SyntaxNode (
                token: new SyntaxToken(SyntaxKind.MinusToken, -1, "-", null),
                precedence: 2,
                kind: SyntaxKind.UnaryOperator,
                climbUpStrategy: ClimbUpStrategy.Lt
            ));
        Assert.Equal("- 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpIfSpecifiedExplicitly()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                kind: SyntaxKind.NumberExpression,
                precedence: 10,
                climbUpStrategy: ClimbUpStrategy.Lte
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "(", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                ClimbUpStrategy.Skip
            ));
        Assert.Equal("1 (", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldSkipClimbUpLeftAssocSyntaxNodeIfPrecedenceIsGreaterThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 1,
                kind: SyntaxKind.BinaryOperator,
                climbUpStrategy: ClimbUpStrategy.Lte
            ))
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression,
                climbUpStrategy: ClimbUpStrategy.Lte
            ));
        Assert.Equal("+ 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpLefAssocSyntaxNodeIfPrecedenceIsEqualToCurrentNodePrecedence()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression,
                climbUpStrategy: ClimbUpStrategy.Lte
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "2", 2),
                precedence: 10,
                kind: SyntaxKind.NumberExpression,
                climbUpStrategy: ClimbUpStrategy.Lte
            ));
        Assert.Equal("2 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldClimbUpLefAssocSyntaxNodeIfPrecedenceIsLessThanCurrentNodePrecedence()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "1", 1),
                precedence: 10,
                kind: SyntaxKind.NumberExpression,
                climbUpStrategy: ClimbUpStrategy.Lte
            ))
            .Insert(new SyntaxNode (
                new SyntaxToken(SyntaxKind.PlusToken, -1, "+", null),
                precedence: 2,
                kind: SyntaxKind.BinaryOperator,
                climbUpStrategy: ClimbUpStrategy.Lte
            ));
        Assert.Equal("+ 1", tree.Traverse(Traversal.PreOrder));
    }

    [Fact]
    public void ShouldInsertNumberAsRightChildOfRoot()
    {
        var tree = new SyntaxTree()
            .Insert(new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.NumberToken, -1, "12", 12),
                precedence: 10,
                kind: SyntaxKind.NumberExpression,
                climbUpStrategy: ClimbUpStrategy.Lte
            ));
        Assert.Equal("12", tree.Traverse(Traversal.InOrder));
    }

    [Fact]
    public void ShouldReturnOpenParenthesis()
    {
        var tree = new SyntaxTree();
        Assert.Equal("", tree.Traverse(Traversal.InOrder));
    }
}
