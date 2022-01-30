using Exev.Syntax;
using Xunit;

namespace Exev.Tests;

public class LexerTests
{
    [Fact]
    public void ShouldBeAbleParseNextIfInvalidFloatingPointNumber()
    {
        var lexer = new Lexer("12. 12");
        var badToken = lexer.NextToken();
        var spaceToken = lexer.NextToken();
        var numberToken = lexer.NextToken();

        Assert.Equal(SyntaxKind.BadToken, badToken.Kind);
        Assert.Equal("12.", badToken.Text);
        Assert.Equal(SyntaxKind.SpaceToken, spaceToken.Kind);
        Assert.Equal(" ", spaceToken.Text);
        Assert.Equal(SyntaxKind.NumberToken, numberToken.Kind);
        Assert.Equal("12", numberToken.Text);
    }

    [Fact]
    public void ShouldReturnBadTokenIfInvalidFormatOfNumber()
    {
        var token = new Lexer("12.").NextToken();

        Assert.Equal(SyntaxKind.BadToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("12.", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnFloatingPointNumber()
    {
        var token = new Lexer("12.2").NextToken();

        Assert.Equal(SyntaxKind.NumberToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("12.2", token.Text);
        Assert.Equal(12.2, token.Value);
    }

    [Theory]
    [InlineData("f", "f")]
    [InlineData("foo", "foo")]
    [InlineData("foo12", "foo12")]
    [InlineData("foo_12", "foo")]
    public void ShouldReturnFunctionNameToken(string source, string expected)
    {
        var token = new Lexer(source).NextToken();

        Assert.Equal(SyntaxKind.LiteralToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal(expected, token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnFactorialToken()
    {
        var token = new Lexer("!").NextToken();

        Assert.Equal(SyntaxKind.LiteralToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("!", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnExponentToken()
    {
        var token = new Lexer("^").NextToken();

        Assert.Equal(SyntaxKind.ExponentToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("^", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnBadToken()
    {
        var token = new Lexer("?").NextToken();

        Assert.Equal(SyntaxKind.BadToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("?", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnSlashToken()
    {
        var token = new Lexer("/").NextToken();

        Assert.Equal(SyntaxKind.SlashToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("/", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnMinusToken()
    {
        var token = new Lexer("-").NextToken();

        Assert.Equal(SyntaxKind.MinusToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("-", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnPlusToken()
    {
        var token = new Lexer("+").NextToken();

        Assert.Equal(SyntaxKind.PlusToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("+", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnStarToken()
    {
        var token = new Lexer("*").NextToken();

        Assert.Equal(SyntaxKind.AsteriskToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("*", token.Text);
        Assert.Null(token.Value);
    }

    [Fact]
    public void ShouldReturnCloseParenthesisToken()
    {
        var token = new Lexer(")").NextToken();

        Assert.Equal(SyntaxKind.CloseParenthesisToken, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal(")", token.Text);
        Assert.Null(token.Value);
    }

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
