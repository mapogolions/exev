using Exev.Syntax;

namespace Exev;

public interface IEvaluator
{
    double Evaluate(SyntaxTree tree);
}
