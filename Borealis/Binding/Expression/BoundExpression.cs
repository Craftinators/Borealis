using System;

namespace Borealis.Binding {
    internal abstract class BoundExpression : BoundNode {
        public abstract Type Type { get; }
    }
}