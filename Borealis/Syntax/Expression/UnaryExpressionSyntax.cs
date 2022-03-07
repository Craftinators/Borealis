using System.Collections.Generic;

namespace Borealis.Syntax {
    internal sealed class UnaryExpressionSyntax : ExpressionSyntax {
        public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax expression) {
            OperatorToken = operatorToken;
            Expression = expression;
        }
        
        public override SyntaxType Type => SyntaxType.UnaryExpression;
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return OperatorToken;
            yield return Expression;
        }
    }
}