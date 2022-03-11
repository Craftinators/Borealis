using System;
using System.Linq;
using Borealis.Syntax;

namespace Borealis.Binding {
    internal sealed class BoundUnaryOperator {
        private static readonly BoundUnaryOperator[] Operators = {
            new BoundUnaryOperator(SyntaxType.BangToken, BoundUnaryOperatorType.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxType.PlusToken, BoundUnaryOperatorType.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxType.TildeToken, BoundUnaryOperatorType.BitwiseComplement, typeof(int)),
            new BoundUnaryOperator(SyntaxType.MinusToken, BoundUnaryOperatorType.Negation, typeof(int))
        };

        public BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType operatorType, Type expressionType) :
            this(syntaxType, operatorType, expressionType, expressionType) {
        }

        public BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType operatorType, Type expressionType,
            Type resultType) {
            SyntaxType = syntaxType;
            OperatorType = operatorType;
            ExpressionType = expressionType;
            ResultType = resultType;
        }

        public SyntaxType SyntaxType { get; }
        public BoundUnaryOperatorType OperatorType { get; }
        public Type ExpressionType { get; }
        public Type ResultType { get; }

        public static BoundUnaryOperator Bind(SyntaxType syntaxType, Type expressionType) {
            return Operators.FirstOrDefault(boundOperator =>
                boundOperator.SyntaxType == syntaxType && boundOperator.ExpressionType == expressionType);
        }
    }
}