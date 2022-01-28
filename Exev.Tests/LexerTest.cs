using Xunit;

namespace Exev.Tests;

public class UnitTest1
{
    [Fact]
    public void ShouldReturnNumberToken()
    {
        var lexer = new Lexer("12");
        var token = lexer.NextToken();

        Assert.Equal(SyntaxKind.Number, token.Kind);
        Assert.Equal(0, token.Position);
        Assert.Equal("12", token.Text);
        Assert.Equal(12, token.Value);
    }
}
