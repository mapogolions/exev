namespace Exev.Syntax;

public class SyntaxTree
{
    private readonly SyntaxNode _root;
    private SyntaxNode _currentNode { get; set; }

    public SyntaxTree()
    {
        _root = _currentNode = new SyntaxNode(
                token: new SyntaxToken(SyntaxKind.OpenParenthesisToken, -1, "(", null),
                precedence: 1,
                kind: SyntaxKind.PrecedenceOperator,
                climbUpStrategy: ClimbUpStrategy.Skip
            );
    }

    public SyntaxNode? Root => _root.Right;

    public SyntaxTree Insert(SyntaxNode node)
    {
        _currentNode = ClimbUp(node);
        if (node.Token.Kind == SyntaxKind.CloseParenthesisToken)
        {
            _currentNode = RemoveOpenparenthesis();
            return this;
        }
        node.Left = _currentNode.Right;
        node.Parent = _currentNode;
        if (_currentNode.Right != null) _currentNode.Right.Parent = node;
        _currentNode.Right = node;
        _currentNode = node;
        return this;
    }

    private SyntaxNode RemoveOpenparenthesis()
    {
        var node = _currentNode.Parent;
        if (node == null) return _currentNode;
        node.Right = _currentNode.Right;
        if (_currentNode.Right != null)
        {
            _currentNode.Right.Parent = node;
        }
        return node;
    }

    private SyntaxNode ClimbUp(SyntaxNode node)
    {
        if (node.ClimbUpStrategy == ClimbUpStrategy.Skip) return _currentNode;
        var currentNode = _currentNode!;
        if (node.ClimbUpStrategy is ClimbUpStrategy.Lte)
        {
            while (currentNode != _root && node.Precedence <= currentNode!.Precedence)
                currentNode = currentNode.Parent;
        }
        else
        {
            while (currentNode != _root && node.Precedence < currentNode!.Precedence)
                currentNode = currentNode.Parent;
        }
        return currentNode;
    }
}
