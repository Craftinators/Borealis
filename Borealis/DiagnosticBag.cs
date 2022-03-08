using System;
using System.Collections;
using System.Collections.Generic;
using Borealis.Syntax;

namespace Borealis {
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic> {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(DiagnosticBag diagnostics) {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        public void ReportInvalidNumber(TextSpan span, string text, Type type) {
            string message = $"The number {text} isn't a valid {type}.";
            Report(span, message);
        }

        public void ReportBadCharacter(int position, char character) {
            TextSpan span = new TextSpan(position, 1);
            string message = $"Bad character input: '{character}'.";
            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxType actualType, SyntaxType expectedType) {
            string message = $"Unexpected token <{actualType}>, expected <{expectedType}>.";
            Report(span, message);
        }
        
        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type expressionType) {
            string message = $"Unary operator '{operatorText}' is not defined for type {expressionType}.";
            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftExpressionType, Type rightExpressionType) {
            string message = $"Binary operator '{operatorText}' is not defined for types {leftExpressionType} and {rightExpressionType}.";
            Report(span, message);
        }
        
        public void ReportUndefinedName(TextSpan span, string name) {
            string message = $"Variable '{name}' doesn't exist.";
            Report(span, message);
        }
        
        private void Report(TextSpan span, string message) {
            Diagnostic diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }
    }
}