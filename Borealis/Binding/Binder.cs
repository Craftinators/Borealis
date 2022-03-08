using System;
using System.Collections.Generic;
using Borealis.Syntax;

namespace Borealis.Binding {
    internal sealed class Binder {
        private readonly Dictionary<string, object> _variables;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public Binder(Dictionary<string, object> variables) {
            _variables = variables;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        public BoundExpression BindExpression(SyntaxNode expression) {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (expression.Type) {
                case SyntaxType.ParenthesizedExpression: return BindParenthesizedExpression(expression as ParenthesizedExpressionSyntax);
                case SyntaxType.LiteralExpression: return BindLiteralExpression(expression as LiteralExpressionSyntax);
                case SyntaxType.UnaryExpression: return BindUnaryExpression(expression as UnaryExpressionSyntax);
                case SyntaxType.BinaryExpression: return BindBinaryExpression(expression as BinaryExpressionSyntax);
                case SyntaxType.NameExpression: return BindNameExpression(expression as NameExpressionSyntax);
                case SyntaxType.AssignmentExpression: return BindAssignmentExpression(expression as AssignmentExpressionSyntax);
                default: throw new Exception($"Unexpected syntax {expression.Type}");
            }
        }
        
        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax expression) {
            return BindExpression(expression.Expression);
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

        private BoundExpression BindNameExpression(NameExpressionSyntax expression) {
            string name = expression.IdentifierToken.Text;
            if (!_variables.TryGetValue(name, out object value)) {
                _diagnostics.ReportUndefinedName(expression.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }

            Type type = value.GetType();
            return new BoundVariableExpression(name, type);
        }
        
        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax expression) {
            string name = expression.IdentifierToken.Text;
            BoundExpression boundExpression = BindExpression(expression.Expression);

            object defaultValue = boundExpression.Type == typeof(int) ? 0 :
                boundExpression.Type == typeof(bool) ? (object) false : null;

            _variables[name] = defaultValue ?? throw new Exception($"Unsupported variable type: {boundExpression.Type}");
            return new BoundAssignmentExpression(name, boundExpression);
        }
    }
}