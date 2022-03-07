using System.Collections.Generic;

namespace Borealis.Syntax {
    internal sealed class Lexer {
        private readonly string _text;
        private readonly List<string> _diagnostics = new List<string>();
        private int _position;
        
        public Lexer(string text) {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;
        private char Current => Peek(0);
        private char Lookahead => Peek(1);
        
        public SyntaxToken Lex() {
            if (_position >= _text.Length) return new SyntaxToken(SyntaxType.EndOfLineToken, _position, "\0", null);

            if (char.IsDigit(Current)) {
                int start = _position;

                while (char.IsDigit(Current)) Next();

                int length = _position - start;
                string subtext = _text.Substring(start, length);

                if (!int.TryParse(subtext, out int value)) 
                    _diagnostics.Add($"The number {_text} isn't a valid Int32");

                return new SyntaxToken(SyntaxType.NumberToken, start, subtext, value);
            }

            if (char.IsWhiteSpace(Current)) {
                int start = _position;

                while (char.IsWhiteSpace(Current)) Next();

                int length = _position - start;
                string subtext = _text.Substring(start, length);

                return new SyntaxToken(SyntaxType.WhitespaceToken, start, subtext, null);
            }

            if (char.IsLetter(Current)) {
                int start = _position;

                while (char.IsLetter(Current)) Next();

                int length = _position - start;
                string subtext = _text.Substring(start, length);
                SyntaxType type = SyntaxFacts.GetKeywordType(subtext);
                
                return new SyntaxToken(type, start, subtext, null);
            }
            
            switch (Current) {
                case '+': return new SyntaxToken(SyntaxType.PlusToken, _position++, "+", null);
                case '-': return new SyntaxToken(SyntaxType.MinusToken, _position++, "-", null);
                case '*': return new SyntaxToken(SyntaxType.StarToken, _position++, "*", null);
                case '/': return new SyntaxToken(SyntaxType.SlashToken, _position++, "/", null);
                case '(': return new SyntaxToken(SyntaxType.OpenParenthesisToken, _position++, "(", null);
                case ')': return new SyntaxToken(SyntaxType.CloseParenthesisToken, _position++, ")", null);
                case '&':
                    if (Lookahead == '&')
                        return new SyntaxToken(SyntaxType.AmpersandAmpersandToken, _position += 2, "&&", null);
                    
                    break;
                case '|':
                    if (Lookahead == '|')
                        return new SyntaxToken(SyntaxType.PipePipeToken, _position += 2, "==", null);
                    
                    break;
                case '=':
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxType.EqualsEqualsToken, _position += 2, "==", null);
                    
                    break;
                case '!':
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxType.BangEqualsToken, _position += 2, "!=", null);
                    else return new SyntaxToken(SyntaxType.BangToken, _position++, "!", null);
            }
            
            _diagnostics.Add($"ERROR: bad character input: '{Current}'");
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