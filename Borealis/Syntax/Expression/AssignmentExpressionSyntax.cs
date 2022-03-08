using System.Collections.Generic;

namespace Borealis.Syntax {
    public sealed class AssignmentExpressionSyntax : ExpressionSyntax {
        public AssignmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken equalsToken, ExpressionSyntax expression) {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public override SyntaxType Type => SyntaxType.AssignmentExpression;
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Expression;
        } 
    }
}