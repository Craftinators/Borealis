using System.Collections.Generic;

namespace Borealis.Syntax {
    internal sealed class Parser {
        private readonly SyntaxToken[] _tokens;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private int _position;
        
        public Parser(string text) {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            Lexer lexer = new Lexer(text);
            SyntaxToken token;
              
            do {
                token = lexer.Lex();
                if (token.Type != SyntaxType.WhitespaceToken && token.Type != SyntaxType.BadToken) tokens.Add(token);
            } while (token.Type != SyntaxType.EndOfLineToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        private SyntaxToken Current => Peek(0);
        
        public SyntaxTree Parse() {
            ExpressionSyntax expression = ParseExpression();
            SyntaxToken endOfFileToken = MatchToken(SyntaxType.EndOfLineToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0) {
            ExpressionSyntax leftExpression;
            
            int unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence) {
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax expression = ParseExpression(unaryOperatorPrecedence);
                leftExpression = new UnaryExpressionSyntax(operatorToken, expression);
            } else {
                leftExpression = ParsePrimaryExpression();
            }
            
            while (true) {
                int precedence = Current.Type.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence) break;

                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax rightExpression = ParseExpression(precedence);

                leftExpression = new BinaryExpressionSyntax(leftExpression, operatorToken, rightExpression);
            }

            return leftExpression;
        }
        
        private ExpressionSyntax ParsePrimaryExpression() {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (Current.Type) {
                case SyntaxType.OpenParenthesisToken:
                    SyntaxToken leftToken = NextToken();
                    ExpressionSyntax expression = ParseExpression();
                    SyntaxToken rightToken = MatchToken(SyntaxType.CloseParenthesisToken);
                    return new ParenthesizedExpressionSyntax(leftToken, expression, rightToken);
                case SyntaxType.TrueKeyword:
                case SyntaxType.FalseKeyword:
                    SyntaxToken keywordToken = NextToken();
                    bool value = keywordToken.Type == SyntaxType.TrueKeyword;
                    return new LiteralExpressionSyntax(keywordToken, value);
                default:
                    SyntaxToken numberToken = MatchToken(SyntaxType.NumberToken);
                    return new LiteralExpressionSyntax(numberToken);
            }
        }
        
        private SyntaxToken NextToken() {
            SyntaxToken current = Current;
            _position++;
            return current;
        }
        
        private SyntaxToken MatchToken(SyntaxType type) {
            if (Current.Type == type)
                return NextToken();
            
            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Type, type);
            return new SyntaxToken(type, Current.Position, null, null);
        }
        
        private SyntaxToken Peek(int offset) {
            int index = _position + offset;
            return index >= _tokens.Length ? _tokens[_tokens.Length - 1] : _tokens[index];
        }
    }
}