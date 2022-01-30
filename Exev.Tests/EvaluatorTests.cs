using Xunit;

namespace Exev;

public class EvaluatorTests
{
    [Theory]
    [InlineData("1 + 1", 2.0)]
    [InlineData("2 - 1", 1.0)]
    public void ShouldEvaluateValidExpressions(string source, double expected)
    {
        var tree = new Parser(new Lexer(source)).Parse();
        var evaluator = new Evaluator();
        Assert.Equal(expected, evaluator.Evaluate(tree));
    }
}
