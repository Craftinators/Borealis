using System;
using Borealis.Binding;

namespace Borealis {
    internal sealed class Evaluator {
        private readonly BoundExpression _root;
        
        public Evaluator(BoundExpression root) {
            _root = root;
        }

        public object Evaluate() {
            return EvaluateExpression(_root);
        }

        private static object EvaluateExpression(BoundExpression node) {
            switch (node) {
                case BoundLiteralExpression number:
                    return number.Value;
                case BoundUnaryExpression unaryExpression: {
                    object expression = EvaluateExpression(unaryExpression.Expression);
                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (unaryExpression.BoundOperator.OperatorType) {
                        case BoundUnaryOperatorType.Identity: return +(int) expression;
                        case BoundUnaryOperatorType.Negation: return -(int) expression;
                        case BoundUnaryOperatorType.LogicalNegation: return !(bool) expression;
                        default: throw new Exception($"Unexpected unary operator {unaryExpression.BoundOperator}");
                    }
                }
                case BoundBinaryExpression binaryExpression: {
                    object leftValue = EvaluateExpression(binaryExpression.LeftExpression);
                    object rightValue = EvaluateExpression(binaryExpression.RightExpression);

                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (binaryExpression.BoundOperator.OperatorType) {
                        case BoundBinaryOperatorType.Addition: return (int) leftValue + (int) rightValue;
                        case BoundBinaryOperatorType.Subtraction: return (int) leftValue - (int) rightValue;
                        case BoundBinaryOperatorType.Multiplication: return (int) leftValue * (int) rightValue;
                        case BoundBinaryOperatorType.Division: return (int) leftValue / (int) rightValue;
                        case BoundBinaryOperatorType.LogicalAnd: return (bool) leftValue && (bool) rightValue;
                        case BoundBinaryOperatorType.LogicalOr: return (bool) leftValue || (bool) rightValue;
                        case BoundBinaryOperatorType.Equals: return Equals(leftValue, rightValue);
                        case BoundBinaryOperatorType.NotEquals: return !Equals(leftValue, rightValue);
                        default: throw new Exception($"Unexpected binary operator {binaryExpression.BoundOperator}");
                    }
                }
            }
            
            throw new Exception($"Unexpected node {node.Type}");
        }
    }
}