using System;
using System.Linq;
using Borealis.Binding;
using Borealis.Syntax;

namespace Borealis {
    public sealed class Compilation {
        public Compilation(SyntaxTree syntaxTree) {
            SyntaxTree = syntaxTree;
        }
        
        public SyntaxTree SyntaxTree { get; }

        public EvaluationResult Evaluate() {
            Binder binder = new Binder();
            BoundExpression boundExpression = binder.BindExpression(SyntaxTree.Root);

            Diagnostic[] diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if (diagnostics.Any()) return new EvaluationResult(diagnostics, null);

            Evaluator evaluator = new Evaluator(boundExpression);
            object value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(), value);
        }
    }
}