using Exev.Syntax;

namespace Exev;

public class Evaluator : IEvaluator
{
    public double Evaluate(SyntaxTree tree)
    {
        return EvaluateExpression(tree.Root);

        static double EvaluateExpression(SyntaxNode? node)
        {
            if (node == null) return 0;
            var a = EvaluateExpression(node.Left);
            var b = EvaluateExpression(node.Right);
            if (node.Kind == SyntaxKind.NumberExpression) return Convert.ToDouble(node.Token.Value!);
            if (node.Kind == SyntaxKind.UnaryOperator)
            {
                return node.Token.Kind switch
                {
                    SyntaxKind.PlusToken => b,
                    SyntaxKind.MinusToken => -b,
                    _ => throw new InvalidOperationException($"Unsupported Operation: {node.Token.Text}")
                };
            }
            if (node.Kind == SyntaxKind.BinaryOperator)
            {
                return node.Token.Kind switch
                {
                    SyntaxKind.PlusToken => a + b,
                    SyntaxKind.MinusToken => a - b,
                    Syntax.SyntaxKind.AsteriskToken => a * b,
                    SyntaxKind.SlashToken => a / b,
                    SyntaxKind.ExponentToken => Math.Pow(a, b),
                    _ => throw new InvalidOperationException($"Unsupported Operation: {node.Token.Text}")
                };
            }
            throw new InvalidOperationException($"Unsupported Operation: {node.Token.Text}");
        }
    }
}
