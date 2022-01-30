namespace Exev.Syntax;

public enum TokenKind
{
    NumberToken,
    SpaceToken,
    EofToken,
    PlusToken,
    MinusToken,
    AsteriskToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    LiteralToken,
    ExponentToken,
    BadToken,
    FunctionNameToken
}

public enum NodeKind
{
    UnaryPlus,
    UnaryMinus,
    Multiplication,
    Division,
    Addition,
    Subtration
}
