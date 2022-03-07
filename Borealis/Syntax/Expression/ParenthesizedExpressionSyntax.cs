using System.Collections.Generic;

namespace Borealis.Syntax {
    internal sealed class ParenthesizedExpressionSyntax : ExpressionSyntax {
        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken) {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }
        
        public override SyntaxType Type => SyntaxType.ParenthesizedExpression;
        public ExpressionSyntax Expression { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public SyntaxToken CloseParenthesisToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
        }
    }
}