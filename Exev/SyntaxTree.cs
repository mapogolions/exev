namespace Exev;

public class SyntaxTree
{
    public SyntaxNode Root { get; private set; }

    private SyntaxNode CurrentNode { get; set; }

    public SyntaxTree(SyntaxNode root) => Root = CurrentNode = root;

    public SyntaxTree Insert(SyntaxNode node)
    {
        node.Left = CurrentNode.Right;
        node.Parent = CurrentNode;
        if (CurrentNode.Right != null) CurrentNode.Right.Parent = node;
        CurrentNode.Right = node;
        CurrentNode = node;
        return this;
    }
}
