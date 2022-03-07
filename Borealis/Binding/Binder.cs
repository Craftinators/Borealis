using System;
using Borealis.Syntax;

namespace Borealis.Binding {
    internal sealed class Binder {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        
        public DiagnosticBag Diagnostics => _diagnostics;

        public BoundExpression BindExpression(SyntaxNode expression) {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (expression.Type) {
                case SyntaxType.LiteralExpression: return BindLiteralExpression(expression as LiteralExpressionSyntax);
                case SyntaxType.UnaryExpression: return BindUnaryExpression(expression as UnaryExpressionSyntax);
                case SyntaxType.BinaryExpression: return BindBinaryExpression(expression as BinaryExpressionSyntax);
                case SyntaxType.ParenthesizedExpression: return BindExpression((expression as ParenthesizedExpressionSyntax)?.Expression);
                default: throw new Exception($"Unexpected syntax {expression.Type}");
            }
        }

        private static BoundExpression BindLiteralExpression(LiteralExpressionSyntax expression) {
            object value = expression.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax expression) {
            BoundExpression boundExpression = BindExpression(expression.Expression);
            BoundUnaryOperator boundOperator = BoundUnaryOperator.Bind(expression.OperatorToken.Type, boundExpression.Type);

            if (boundOperator != null) 
                return new BoundUnaryExpression(boundOperator, boundExpression);
            
            _diagnostics.ReportUndefinedUnaryOperator(expression.OperatorToken.Span, expression.OperatorToken.Text, boundExpression.Type);
            return boundExpression;
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax expression) {
            BoundExpression boundLeftExpression = BindExpression(expression.LeftExpression);
            BoundExpression boundRightExpression = BindExpression(expression.RightExpression);
            BoundBinaryOperator boundOperator = BoundBinaryOperator.Bind(expression.OperatorToken.Type, boundLeftExpression.Type, boundRightExpression.Type);
            
            if (boundOperator != null) 
                return new BoundBinaryExpression(boundLeftExpression, boundOperator, boundRightExpression);
            
            _diagnostics.ReportUndefinedBinaryOperator(expression.OperatorToken.Span, expression.OperatorToken.Text, boundLeftExpression.Type, boundRightExpression.Type);
            return boundLeftExpression;
        }
    }
}