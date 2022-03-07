using System.Collections.Generic;

namespace Borealis.Syntax {
    internal abstract class SyntaxNode {
        public abstract SyntaxType Type { get; }
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}