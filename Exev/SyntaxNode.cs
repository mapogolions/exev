namespace Exev;

public class SyntaxNode
{
    public SyntaxNode? Left { get; set; }
    public SyntaxToken? Token { get; set; }
    public SyntaxNode? Right { get; set; }
    public int Precedence { get; set; }
    public SyntaxNodeInfo NodeInfo { get; set; }
}
