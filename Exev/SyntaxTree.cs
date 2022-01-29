namespace Exev;

public class SyntaxTree
{
    public SyntaxNode Root { get; private set; }

    private SyntaxNode CurrentNode { get; set; }

    public SyntaxTree(SyntaxNode root) => Root = CurrentNode = root;

    public SyntaxTree Insert(SyntaxNode node)
    {
        CurrentNode = ClimbUp(node);
        if (node.Token.Kind == SyntaxKind.CloseParenthesisToken)
        {
            CurrentNode = RemoveOpenBracket();
            return this;
        }
        node.Left = CurrentNode.Right;
        node.Parent = CurrentNode;
        if (CurrentNode.Right != null) CurrentNode.Right.Parent = node;
        CurrentNode.Right = node;
        CurrentNode = node;
        return this;
    }

    private SyntaxNode RemoveOpenBracket()
    {
        var node = CurrentNode.Parent;
        if (node == null) return CurrentNode;
        node.Right = CurrentNode.Right;
        if (CurrentNode.Right != null)
        {
            CurrentNode.Right.Parent = node;
        }
        return node;
    }

    private SyntaxNode ClimbUp(SyntaxNode node)
    {
        if (node.MetaInfo == SyntaxNodeInfo.SkipClimbUp) return CurrentNode;
        var currentNode = CurrentNode!;
        if (node.MetaInfo is SyntaxNodeInfo.LeftAssoc)
        {
            while (currentNode != Root && node.Precedence <= currentNode!.Precedence)
                currentNode = currentNode.Parent;
        }
        else
        {
            while (currentNode != Root && node.Precedence < currentNode!.Precedence)
                currentNode = currentNode.Parent;
        }
        return currentNode;
    }
}
