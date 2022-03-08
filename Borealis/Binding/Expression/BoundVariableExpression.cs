using System;

namespace Borealis.Binding {
    internal sealed class BoundVariableExpression : BoundExpression {
        public BoundVariableExpression(string name, Type type) {
            Name = name;
            Type = type;
        }

        public override BoundNodeType NodeType => BoundNodeType.VariableExpression;
        public override Type Type { get; }
        public string Name { get; }
    }
}