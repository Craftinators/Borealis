namespace Borealis.Syntax {
    internal sealed class Lexer {
        private readonly string _text;
        private int _position;

        public Lexer(string text) {
            _text = text;
        }

        public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

        private char Current => Peek(0);
        private char Lookahead => Peek(1);

        public SyntaxToken Lex() {
            if (_position >= _text.Length) return new SyntaxToken(SyntaxType.EndOfLineToken, _position, "\0", null);

            int start = _position;

            if (char.IsDigit(Current)) {
                while (char.IsDigit(Current)) Next();

                int length = _position - start;
                string subtext = _text.Substring(start, length);

                if (!int.TryParse(subtext, out int value))
                    Diagnostics.ReportInvalidNumber(new TextSpan(start, length), _text, typeof(int));

                return new SyntaxToken(SyntaxType.NumberToken, start, subtext, value);
            }

            if (char.IsWhiteSpace(Current)) {
                while (char.IsWhiteSpace(Current)) Next();

                int length = _position - start;
                string subtext = _text.Substring(start, length);

                return new SyntaxToken(SyntaxType.WhitespaceToken, start, subtext, null);
            }

            if (char.IsLetter(Current)) {
                while (char.IsLetter(Current)) Next();

                int length = _position - start;
                string subtext = _text.Substring(start, length);
                SyntaxType type = SyntaxFacts.GetKeywordType(subtext);

                return new SyntaxToken(type, start, subtext, null);
            }

            switch (Current) {
                case '+': return new SyntaxToken(SyntaxType.PlusToken, _position++, "+", null);
                case '-': return new SyntaxToken(SyntaxType.MinusToken, _position++, "-", null);
                case '*':
                    if (Lookahead == '*') {
                        _position += 2;
                        return new SyntaxToken(SyntaxType.StarStarToken, start, "**", null);
                    } else {
                        return new SyntaxToken(SyntaxType.StarToken, _position++, "*", null);
                    }
                case '/': return new SyntaxToken(SyntaxType.SlashToken, _position++, "/", null);
                case '%': return new SyntaxToken(SyntaxType.PercentToken, _position++, "%", null);
                case '(': return new SyntaxToken(SyntaxType.OpenParenthesisToken, _position++, "(", null);
                case ')': return new SyntaxToken(SyntaxType.CloseParenthesisToken, _position++, ")", null);
                case '&':
                    if (Lookahead == '&') {
                        _position += 2;
                        return new SyntaxToken(SyntaxType.AmpersandAmpersandToken, start, "&&", null);
                    }

                    break;
                case '^':
                    if (Lookahead == '^') {
                        _position += 2;
                        return new SyntaxToken(SyntaxType.CaretCaretToken, start, "^^", null);
                    }

                    break;
                case '|':
                    if (Lookahead == '|') {
                        _position += 2;
                        return new SyntaxToken(SyntaxType.PipePipeToken, start, "==", null);
                    }

                    break;
                case '=':
                    if (Lookahead == '=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxType.EqualsEqualsToken, start, "==", null);
                    } else {
                        return new SyntaxToken(SyntaxType.EqualsToken, _position++, "=", null);
                    }
                case '!':
                    if (Lookahead == '=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxType.BangEqualsToken, start, "!=", null);
                    } else {
                        return new SyntaxToken(SyntaxType.BangToken, _position++, "!", null);
                    }
                case '>':
                    switch (Lookahead) {
                        case '=':
                            _position += 2;
                            return new SyntaxToken(SyntaxType.RightAngleBracketEqualsToken, start, ">=", null);
                        case '>':
                            _position += 2;
                            return new SyntaxToken(SyntaxType.RightAngleBracketRightAngleBracketToken, start, ">>",
                                null);
                        default:
                            return new SyntaxToken(SyntaxType.RightAngleBracketToken, _position++, ">", null);
                    }
                case '<':
                    switch (Lookahead) {
                        case '=':
                            _position += 2;
                            return new SyntaxToken(SyntaxType.LeftAngleBracketEqualsToken, start, "<=", null);
                        case '<':
                            _position += 2;
                            return new SyntaxToken(SyntaxType.LeftAngleBracketLeftAngleBracketToken, start, "<<", null);
                        default:
                            return new SyntaxToken(SyntaxType.LeftAngleBracketToken, _position++, "<", null);
                    }
            }

            Diagnostics.ReportBadCharacter(_position, Current);
            return new SyntaxToken(SyntaxType.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }

        private char Peek(int offset) {
            int index = _position + offset;
            return index >= _text.Length ? '\0' : _text[index];
        }

        private void Next() {
            _position++;
        }
    }
}