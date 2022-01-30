namespace Exev.Syntax;

public enum SyntaxKind
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
    FactorialToken,
    LiteralToken,
    ExponentToken,
    BadToken,


    NumberExpression,
    UnaryOperator,
    BinaryOperator,
    CallOperator,
    PrecedenceOperator
}
