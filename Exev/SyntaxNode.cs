namespace Exev;

public class SyntaxNode
{
    public SyntaxNode(SyntaxToken token, int precedence, SyntaxNodeInfo metaInfo)
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
