namespace Exev;

public class SyntaxNode
{
    public SyntaxNode(SyntaxToken token)
    {
        Token = token;
    }

    public SyntaxToken Token { get; }
}
