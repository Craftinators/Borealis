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
        AmpersandToken,
        CaretCaretToken,
        CaretToken,
        PipePipeToken,
        PipeToken,
        RightAngleBracketEqualsToken,
        RightAngleBracketToken,
        LeftAngleBracketEqualsToken,
        LeftAngleBracketToken,
        EqualsEqualsToken,
        EqualsToken,
        RightAngleBracketRightAngleBracketToken,
        LeftAngleBracketLeftAngleBracketToken,

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