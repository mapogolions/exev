namespace Exev.Syntax;

public class SyntaxNode
{
    public SyntaxNode(SyntaxToken token, int precedence, SyntaxKind kind,
        ClimbUpStrategy climbUpStrategy = ClimbUpStrategy.Lte)
    {
        Token = token;
        Precedence = precedence;
        Kind = kind;
        ClimbUpStrategy = climbUpStrategy;
    }

    public SyntaxToken Token { get; set; }
    public int Precedence { get; set; }
    public SyntaxKind Kind { get; }
    public ClimbUpStrategy ClimbUpStrategy { get; set; }

    public SyntaxNode? Parent { get; set; }
    public SyntaxNode? Left { get; set; }
    public SyntaxNode? Right { get; set; }
}
