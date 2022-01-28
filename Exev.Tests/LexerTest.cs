using Xunit;

namespace Exev.Tests;

public class LexerTests
{
    [Fact]
    public void ShouldReturnOpenParenthesisToken()
    {
        var token = new Lexer("(").NextToken();

        Assert.Equal(SyntaxKind.OpenParenthesisToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("(", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnEndOfFileTokenIfSourceIsEmpty()
    {
        var token = new Lexer("").NextToken();

        Assert.Equal(SyntaxKind.EofToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("\0", token.Text);
        Assert.Null(token.Value);
    }

    [Theory]
    [InlineData(" ", " ")]
    [InlineData("\t", "\t")]
    [InlineData(" \t", " \t")]
    [InlineData("\n", "\n")]
    [InlineData("  12", "  ")]
    public void ShouldReturnSpaceToken(string source, string expected)
    {
        var lexer = new Lexer(source);
        var token = lexer.NextToken();

        Assert.Equal(SyntaxKind.SpaceToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal(expected, token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnNumberToken()
    {
        var lexer = new Lexer("12");
        var token = lexer.NextToken();

        Assert.Equal(SyntaxKind.NumberToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("12", token.Text);
        Assert.Equal(12, token.Value);
    }
}
