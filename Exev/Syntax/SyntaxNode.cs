namespace Exev.Syntax;

public class SyntaxNode
{
    public SyntaxNode(SyntaxToken token, int precedence, SyntaxKind kind,
        Assoc assoc = Assoc.Left)
    {
        Token = token;
        Precedence = precedence;
        Kind = kind;
        Assoc = assoc;
    }

    public SyntaxToken Token { get; set; }
    public int Precedence { get; set; }
    public SyntaxKind Kind { get; }
    public Assoc Assoc { get; set; }

    public SyntaxNode? Parent { get; set; }
    public SyntaxNode? Left { get; set; }
    public SyntaxNode? Right { get; set; }
}
