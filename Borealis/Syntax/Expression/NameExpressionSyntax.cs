using System.Collections.Generic;

namespace Borealis.Syntax {
    public sealed class NameExpressionSyntax : ExpressionSyntax {
        public NameExpressionSyntax(SyntaxToken identifierToken) {
            IdentifierToken = identifierToken;
        }

        public override SyntaxType Type => SyntaxType.NameExpression;
        public SyntaxToken IdentifierToken { get; }
        
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return IdentifierToken;
        }
    }
}