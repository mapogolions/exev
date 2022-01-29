namespace Exev;

public class SyntaxNode
{
    public SyntaxNode(SyntaxToken token, int precedence,
        SyntaxNodeInfo metaInfo = SyntaxNodeInfo.None)
    {
        Token = token;
        Precedence = precedence;
        MetaInfo = metaInfo;
    }

    public SyntaxToken Token { get; set; }
    public int Precedence { get; set; }
    public SyntaxNodeInfo MetaInfo { get; set; }

    public SyntaxNode? Parent { get; set; }
    public SyntaxNode? Left { get; set; }
    public SyntaxNode? Right { get; set; }
}
