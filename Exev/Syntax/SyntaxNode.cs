namespace Exev.Syntax;

public class SyntaxNode
{
    public SyntaxNode(SyntaxToken token, int precedence, SyntaxKind kind,
        ClimbUpCondition climbUpStrategy = ClimbUpCondition.Lte)
    {
        Token = token;
        Precedence = precedence;
        Kind = kind;
        ClimbUpStrategy = climbUpStrategy;
    }

    public SyntaxToken Token { get; set; }
    public int Precedence { get; set; }
    public SyntaxKind Kind { get; }
    public ClimbUpCondition ClimbUpStrategy { get; set; }

    public SyntaxNode? Parent { get; set; }
    public SyntaxNode? Left { get; set; }
    public SyntaxNode? Right { get; set; }
}
