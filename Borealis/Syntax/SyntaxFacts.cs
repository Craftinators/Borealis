namespace Borealis.Syntax {
    internal static class SyntaxFacts {
        public static int GetUnaryOperatorPrecedence(this SyntaxType type) {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (type) {
                case SyntaxType.PlusToken:
                case SyntaxType.MinusToken:
                case SyntaxType.BangToken:
                    return 8;
                default: return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxType type) {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (type) {
                case SyntaxType.StarStarToken:
                    return 7;
                case SyntaxType.StarToken:
                case SyntaxType.SlashToken:
                case SyntaxType.PercentToken:
                    return 6;
                case SyntaxType.PlusToken:
                case SyntaxType.MinusToken:
                    return 5;
                case SyntaxType.EqualsEqualsToken:
                case SyntaxType.BangEqualsToken:
                    return 4;
                case SyntaxType.AmpersandAmpersandToken:
                    return 3;
                case SyntaxType.CaretCaretToken:
                    return 2;
                case SyntaxType.PipePipeToken:
                    return 1;
                default: return 0;
            }
        }

        public static SyntaxType GetKeywordType(string text) {
            switch (text) {
                case "true": return SyntaxType.TrueKeyword;
                case "false": return SyntaxType.FalseKeyword;
                default: return SyntaxType.IdentifierToken;
            }
        }
    }
}