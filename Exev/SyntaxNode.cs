namespace Exev;

public class SyntaxNode
{
    public SyntaxNode(SyntaxNode left, SyntaxToken token,
        SyntaxNode right, int precedence, SyntaxNodeInfo nodeInfo)
    {
        Left = left;
        Token = token;
        Right = right;
        Precedence = precedence;
        NodeInfo = nodeInfo;
    }

    public SyntaxNode Left { get; }
    public SyntaxToken Token { get; }
    public SyntaxNode Right { get; }
    public int Precedence { get; }
    public SyntaxNodeInfo NodeInfo { get; }
}
