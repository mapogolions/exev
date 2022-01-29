using Xunit;

namespace Exev.Tests;

public class ParserTests
{
    [Fact]
    public void ShouldSkipWhitespacesAndReturnSyntaxTreeWithFakeRoot()
    {
        var parser = new Parser(new Lexer("  \n\t"));
        var tree = parser.Parse();

        Assert.Equal(SyntaxKind.OpenParenthesisToken, tree?.Root?.Token?.Kind);
        Assert.Null(tree?.Root.Left);
        Assert.Null(tree?.Root.Right);
    }

    [Fact]
    public void ShouldReturnSyntaxTreeWithFakeRoot()
    {
        var parser = new Parser(new Lexer(""));
        var tree = parser.Parse();

        Assert.Equal(SyntaxKind.OpenParenthesisToken, tree?.Root?.Token?.Kind);
        Assert.Null(tree?.Root.Left);
        Assert.Null(tree?.Root.Right);
    }
}