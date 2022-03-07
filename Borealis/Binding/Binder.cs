using System;
using System.Collections.Generic;
using Borealis.Syntax;

namespace Borealis.Binding {
    internal sealed class Binder {
        private readonly List<string> _diagnostics = new List<string>();
        
        public IEnumerable<string> Diagnostics => _diagnostics;

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
            
            _diagnostics.Add($"Unary operator '{expression.OperatorToken.Text}' is not defined for type {boundExpression.Type}");
            return boundExpression;
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax expression) {
            BoundExpression boundLeftExpression = BindExpression(expression.LeftExpression);
            BoundExpression boundRightExpression = BindExpression(expression.RightExpression);
            BoundBinaryOperator boundOperator = BoundBinaryOperator.Bind(expression.OperatorToken.Type, boundLeftExpression.Type, boundRightExpression.Type);
            
            if (boundOperator != null) 
                return new BoundBinaryExpression(boundLeftExpression, boundOperator, boundRightExpression);
            
            _diagnostics.Add($"Binary operator '{expression.OperatorToken.Text}' is not defined for types {boundLeftExpression.Type} and {boundRightExpression.Type}");
            return boundLeftExpression;
        }
    }
}