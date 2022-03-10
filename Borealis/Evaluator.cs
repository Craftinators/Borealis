using System;
using System.Collections.Generic;
using Borealis.Binding;

namespace Borealis {
    internal sealed class Evaluator {
        private readonly BoundExpression _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables) {
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
                    return _variables[variableExpression.Variable];
                case BoundAssignmentExpression assignmentExpression:
                    object value = EvaluateExpression(assignmentExpression.Expression);
                    _variables[assignmentExpression.Variable] = value;
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

                    switch (binaryExpression.BoundOperator.OperatorType) {
                        case BoundBinaryOperatorType.Addition: return (int) leftValue + (int) rightValue;
                        case BoundBinaryOperatorType.Subtraction: return (int) leftValue - (int) rightValue;
                        case BoundBinaryOperatorType.Exponent: return (int) Math.Pow((int) leftValue, (int) rightValue);
                        case BoundBinaryOperatorType.Multiplication: return (int) leftValue * (int) rightValue;
                        case BoundBinaryOperatorType.Division: return (int) leftValue / (int) rightValue;
                        case BoundBinaryOperatorType.Remainder: return (int) leftValue % (int) rightValue;
                        case BoundBinaryOperatorType.LogicalAnd: return (bool) leftValue && (bool) rightValue;
                        case BoundBinaryOperatorType.LogicalXor: return (bool) leftValue ^ (bool) rightValue;
                        case BoundBinaryOperatorType.LogicalOr: return (bool) leftValue || (bool) rightValue;
                        case BoundBinaryOperatorType.GreaterThanOrEqualTo: return (int) leftValue >= (int) rightValue;
                        case BoundBinaryOperatorType.GreaterThan: return (int) leftValue > (int) rightValue;
                        case BoundBinaryOperatorType.LessThanOrEqualTo: return (int) leftValue <= (int) rightValue;
                        case BoundBinaryOperatorType.LessThan: return (int) leftValue < (int) rightValue;
                        case BoundBinaryOperatorType.Equals: return Equals(leftValue, rightValue);
                        case BoundBinaryOperatorType.NotEquals: return !Equals(leftValue, rightValue);
                        case BoundBinaryOperatorType.Leftshift: return (int) leftValue << (int) rightValue;
                        case BoundBinaryOperatorType.Rightshift: return (int) leftValue >> (int) rightValue;
                        default: throw new Exception($"Unexpected binary operator {binaryExpression.BoundOperator}");
                    }
                }
            }

            throw new Exception($"Unexpected node {node.Type}");
        }
    }
}