using System.Collections.Generic;

namespace Borealis.Syntax {
    internal sealed class BinaryExpressionSyntax : ExpressionSyntax { 
        public BinaryExpressionSyntax(ExpressionSyntax leftExpression, SyntaxToken operatorToken, ExpressionSyntax rightExpression) {
            LeftExpression = leftExpression;
            OperatorToken = operatorToken;
            RightExpression = rightExpression;
        }
        
        public override SyntaxType Type => SyntaxType.BinaryExpression;
        public ExpressionSyntax LeftExpression { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax RightExpression { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return LeftExpression;
            yield return OperatorToken;
            yield return RightExpression;
        }
    }
}