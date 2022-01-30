using Xunit;
using  Exev.Tests.DataSources;

namespace Exev;

public class EvaluatorTests
{
    [Theory]
    [ClassData(typeof(EvaluatorDataSource))]
    public void ShouldEvaluateValidExpressions(string source, double expected)
    {
        var tree = new Parser(new Lexer(source)).Parse();
        var evaluator = new Evaluator();
        Assert.Equal(expected, evaluator.Evaluate(tree));
    }
}
