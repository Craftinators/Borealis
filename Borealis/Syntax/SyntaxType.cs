namespace Borealis.Syntax {
    public enum SyntaxType {
        // Generic Tokens
        BadToken,
        EndOfLineToken,
        WhitespaceToken,
        IdentifierToken,

        // Tokens
        NumberToken,
        PlusToken,
        MinusToken,
        StarStarToken,
        StarToken,
        SlashToken,
        PercentToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BangEqualsToken,
        BangToken,
        AmpersandAmpersandToken,
        CaretCaretToken,
        PipePipeToken,
        GreaterThanEqualsToken,
        GreaterThanToken,
        LessThanEqualsToken,
        LessThanToken,
        EqualsEqualsToken,
        EqualsToken,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        NameExpression,
        AssignmentExpression,

        // Keywords
        TrueKeyword,
        FalseKeyword
    }
}