namespace Exev;

public class SyntaxTree
{
    public SyntaxTree(SyntaxNode root)
    {
        Root = root;
    }

    public SyntaxNode Root { get; }
}
