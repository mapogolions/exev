using Exev.Syntax;

namespace Exev;

public class Evaluator : IEvaluator
{
    public double Evaluate(SyntaxTree tree)
    {
        return EvaluateExpression(tree.Root.Right);
        throw new NotImplementedException();
    }

    public double EvaluateExpression(SyntaxNode? node)
    {
        if (node == null) return 0;
        var a = EvaluateExpression(node.Left);
        var b = EvaluateExpression(node.Right);
        if (node.Kind == SyntaxKind.NumberExpression) return Convert.ToDouble(node.Token.Value!);
        if (node.Kind == SyntaxKind.UnaryOperator)
        {
            return node.Token.Text switch
            {
                "+" => b,
                "-" => -b,
                _ => throw new InvalidOperationException()
            };
        }
        if (node.Kind == SyntaxKind.BinaryOperator)
        {
            return node.Token.Text switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b,
                _ => throw new InvalidOperationException()
            };
        }
        throw new InvalidOperationException();
    }
}
