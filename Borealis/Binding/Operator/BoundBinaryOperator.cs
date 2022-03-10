using System;
using System.Linq;
using Borealis.Syntax;

namespace Borealis.Binding {
    internal sealed class BoundBinaryOperator {
        private static readonly BoundBinaryOperator[] Operators = {
            new BoundBinaryOperator(SyntaxType.PlusToken, BoundBinaryOperatorType.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxType.MinusToken, BoundBinaryOperatorType.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxType.StarStarToken, BoundBinaryOperatorType.Exponent, typeof(int)),
            new BoundBinaryOperator(SyntaxType.StarToken, BoundBinaryOperatorType.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxType.SlashToken, BoundBinaryOperatorType.Division, typeof(int)),
            new BoundBinaryOperator(SyntaxType.PercentToken, BoundBinaryOperatorType.Remainder, typeof(int)),

            new BoundBinaryOperator(SyntaxType.RightAngleBracketEqualsToken,
                BoundBinaryOperatorType.GreaterThanOrEqualTo, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxType.RightAngleBracketToken, BoundBinaryOperatorType.GreaterThan, typeof(int),
                typeof(bool)),
            new BoundBinaryOperator(SyntaxType.LeftAngleBracketEqualsToken, BoundBinaryOperatorType.LessThanOrEqualTo,
                typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxType.LeftAngleBracketToken, BoundBinaryOperatorType.LessThan, typeof(int),
                typeof(bool)),

            new BoundBinaryOperator(SyntaxType.EqualsEqualsToken, BoundBinaryOperatorType.Equals, typeof(int),
                typeof(bool)),
            new BoundBinaryOperator(SyntaxType.EqualsEqualsToken, BoundBinaryOperatorType.Equals, typeof(bool)),

            new BoundBinaryOperator(SyntaxType.BangEqualsToken, BoundBinaryOperatorType.NotEquals, typeof(int),
                typeof(bool)),
            new BoundBinaryOperator(SyntaxType.BangEqualsToken, BoundBinaryOperatorType.NotEquals, typeof(bool)),

            new BoundBinaryOperator(SyntaxType.AmpersandAmpersandToken, BoundBinaryOperatorType.LogicalAnd,
                typeof(bool)),
            new BoundBinaryOperator(SyntaxType.CaretCaretToken, BoundBinaryOperatorType.LogicalXor, typeof(bool)),
            new BoundBinaryOperator(SyntaxType.PipePipeToken, BoundBinaryOperatorType.LogicalOr, typeof(bool)),

            new BoundBinaryOperator(SyntaxType.LeftAngleBracketLeftAngleBracketToken, BoundBinaryOperatorType.Leftshift,
                typeof(int)),
            new BoundBinaryOperator(SyntaxType.RightAngleBracketRightAngleBracketToken,
                BoundBinaryOperatorType.Rightshift, typeof(int))
        };

        public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType operatorType, Type type) : this(
            syntaxType, operatorType, type, type, type) {
        }

        public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType operatorType, Type expressionType,
            Type resultType) : this(syntaxType, operatorType, expressionType, expressionType, resultType) {
        }

        public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType operatorType, Type leftExpressionType,
            Type rightExpressionType, Type resultType) {
            SyntaxType = syntaxType;
            OperatorType = operatorType;
            LeftExpressionType = leftExpressionType;
            RightExpressionType = rightExpressionType;
            ResultType = resultType;
        }

        public SyntaxType SyntaxType { get; }
        public BoundBinaryOperatorType OperatorType { get; }
        public Type LeftExpressionType { get; }
        public Type RightExpressionType { get; }
        public Type ResultType { get; }

        public static BoundBinaryOperator Bind(SyntaxType syntaxType, Type leftExpressionType,
            Type rightExpressionType) {
            return Operators.FirstOrDefault(boundOperator =>
                boundOperator.SyntaxType == syntaxType && boundOperator.LeftExpressionType == leftExpressionType &&
                boundOperator.RightExpressionType == rightExpressionType);
        }
    }
}