using System;

namespace Borealis.Binding {
    internal sealed class BoundBinaryExpression : BoundExpression {
        public BoundBinaryExpression(BoundExpression leftExpression, BoundBinaryOperator boundOperator, BoundExpression rightExpression) {
            LeftExpression = leftExpression;
            BoundOperator = boundOperator;
            RightExpression = rightExpression;
        }

        public override BoundNodeType NodeType => BoundNodeType.BinaryExpression;
        public override Type Type => BoundOperator.ResultType;
        public BoundExpression LeftExpression { get; }
        public BoundBinaryOperator BoundOperator { get; }
        public BoundExpression RightExpression { get; }
    }
}