using System;
using System.Collections.Generic;
using Borealis.Binding;

namespace Borealis {
    internal sealed class Evaluator {
        private readonly BoundExpression _root;
        private readonly Dictionary<string, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<string, object> variables) {
            _root = root;
            _variables = variables;
        }

        public object Evaluate() {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node) {
            switch (node) {
                case BoundLiteralExpression number:
                    return number.Value;
                case BoundVariableExpression variableExpression:
                    return _variables[variableExpression.Name];
                case BoundAssignmentExpression assignmentExpression:
                    object value = EvaluateExpression(assignmentExpression.Expression);
                    _variables[assignmentExpression.Name] = value;
                    return value;
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