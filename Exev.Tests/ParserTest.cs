using System.IO;
using Exev.Syntax;
using Exev.Validation;
using Xunit;

namespace Exev.Tests;

public class ParserTests
{
    [Fact]
    public void CloseParenthesisShouldNotBeAfterOpen()
    {
        var parser = new Parser(new Lexer("()"));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void OpenParenthesisShouldNotBeTheLastToken()
    {
        var parser = new Parser(new Lexer("12 + ("));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void CloseParenthesisShouldNotBeTheFirstToken()
    {
        var parser = new Parser(new Lexer(") 12"));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void ShouldWrapNumberTwice()
    {
        var parser = new Parser(new Lexer("((1))"));
        var tree = parser.Parse();
        var actual = tree.Traverse(Traversal.PreOrder);
        Assert.Equal("( 1", actual);
    }


    [Fact]
    public void ShouldThrowExceptionIfNumberFollowsCloseParenthesis()
    {
        var parser = new Parser(new Lexer("(1) 23"));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void ShouldThrowExceptionIfNumberFollowsFactorial()
    {
        var parser = new Parser(new Lexer("12! 23"));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void ShouldThrowExceptionIfOpenParenthesisFollowsNumber()
    {
        var parser = new Parser(new Lexer("12 ("));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void ShouldThrowExceptionIfOpenParenthesisFollowsClose()
    {
        var parser = new Parser(new Lexer("(1) ("));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void ShouldThrowExceptionWhenBadTokenFound()
    {
        var parser = new Parser(new Lexer("  \n\t~"));
        Assert.Throws<TokenValidationException>(parser.Parse);
    }

    [Fact]
    public void ShouldSkipWhitespacesAndReturnSyntaxTree()
    {
        var parser = new Parser(new Lexer("  \n\t"));
        var tree = parser.Parse();

        Assert.Equal(SyntaxKind.OpenParenthesisToken, tree?.Root?.Token?.Kind);
        Assert.Null(tree?.Root.Left);
        Assert.Null(tree?.Root.Right);
    }

    [Fact]
    public void ShouldReturnSyntaxTreeWithOpenParenthesisAsRoot()
    {
        var parser = new Parser(new Lexer(""));
        var tree = parser.Parse();

        Assert.Equal(SyntaxKind.OpenParenthesisToken, tree?.Root?.Token?.Kind);
        Assert.Null(tree?.Root.Left);
        Assert.Null(tree?.Root.Right);
    }
}
