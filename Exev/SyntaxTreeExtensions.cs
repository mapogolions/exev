namespace Exev;

public static class SyntaxTreeExtensions
{
    public static string Traverse(this SyntaxTree tree, Traversal traversal)
    {
        return Traverse(tree, traversal, node => node.Token.Text);
    }

    public static string Traverse(this SyntaxTree tree, Traversal traversal, Func<SyntaxNode, string> selector, string sep = " ")
    {
        var nodes = traversal switch
        {
            Traversal.PreOrder => PreOrder(tree),
            Traversal.PostOrder => PostOrder(tree),
            _ => InOrder(tree)
        };
        return string.Join(sep, nodes.Select(selector));
    }

    public static IEnumerable<SyntaxNode> PreOrder(this SyntaxTree tree)
    {
        static IEnumerable<SyntaxNode> Iter(SyntaxNode? node, IList<SyntaxNode> acc)
        {
            if (node is null) return acc;
            acc.Add(node);
            Iter(node.Left, acc);
            Iter(node.Right, acc);
            return acc;
        }
        return Iter(tree.Root, new List<SyntaxNode>());
    }

    public static IEnumerable<SyntaxNode> InOrder(this SyntaxTree tree)
    {
        static IEnumerable<SyntaxNode> Iter(SyntaxNode? node, IList<SyntaxNode> acc)
        {
            if (node is null) return acc;
            Iter(node.Left, acc);
            acc.Add(node);
            Iter(node.Right, acc);
            return acc;
        }
        return Iter(tree.Root, new List<SyntaxNode>());
    }

    public static IEnumerable<SyntaxNode> PostOrder(this SyntaxTree tree)
    {
        static IEnumerable<SyntaxNode> Iter(SyntaxNode? node, IList<SyntaxNode> acc)
        {
            if (node is null) return acc;
            Iter(node.Left, acc);
            Iter(node.Right, acc);
            acc.Add(node);
            return acc;
        }
        return Iter(tree.Root, new List<SyntaxNode>());
    }

    // private static bool MustBeIncluded(SyntaxNode node) =>
    //     !Equals(node.Token, OperatorToken.OpenBracket) && !Equals(node.Token, OperatorToken.CloseBracket);
}
