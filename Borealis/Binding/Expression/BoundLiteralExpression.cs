using System;

namespace Borealis.Binding {
    internal sealed class BoundLiteralExpression : BoundExpression {
        public BoundLiteralExpression(object value) {
            Value = value;
        }

        public override BoundNodeType NodeType => BoundNodeType.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
    }
}