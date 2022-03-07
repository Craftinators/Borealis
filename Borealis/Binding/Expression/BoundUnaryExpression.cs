using System;

namespace Borealis.Binding {
    internal sealed class BoundUnaryExpression : BoundExpression {
        public BoundUnaryExpression(BoundUnaryOperator boundOperator, BoundExpression expression) {
            BoundOperator = boundOperator;
            Expression = expression;
        }

        public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;
        public override Type Type => BoundOperator.ResultType;
        public BoundUnaryOperator BoundOperator { get; }
        public BoundExpression Expression { get; }
    }
}