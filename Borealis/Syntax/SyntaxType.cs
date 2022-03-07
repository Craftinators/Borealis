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
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BangToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        
        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        
        // Keywords
        TrueKeyword,
        FalseKeyword
    }
}