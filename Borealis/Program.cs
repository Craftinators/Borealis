using System;
using System.Collections.Generic;
using System.Linq;
using Borealis.Binding;
using Borealis.Syntax;

namespace Borealis {
    // ReSharper disable once UnusedType.Global
    internal static class Program {
        // ReSharper disable once UnusedMember.Global
        public static void Compile() {
            while (true) {
                Console.Write("> ");
                string line = Console.ReadLine();
                
                SyntaxTree syntaxTree = SyntaxTree.Parse(line);
                Binder binder = new Binder();
                BoundExpression boundExpression = binder.BindExpression(syntaxTree.Root);
                
                IReadOnlyList<string> diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
                
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrintSyntaxNodeTree(syntaxTree.Root);
                Console.ForegroundColor = color;
                
                if (!diagnostics.Any()) {
                    Evaluator evaluator = new Evaluator(boundExpression);
                    object result = evaluator.Evaluate();
                    Console.WriteLine(result);
                } else {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach (string diagnostic in diagnostics) 
                        Console.WriteLine(diagnostic);
                    Console.ForegroundColor = color;
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void PrintSyntaxNodeTree(SyntaxNode node, string indent = "", bool isLast = true) {
            string marker = isLast ? "└──" : "├──";
            
            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Type);

            if (node is SyntaxToken token && token.Value != null) {
                Console.Write(" ");
                Console.Write(token.Value);
            }

            Console.WriteLine();
            indent += isLast ? "   " : "│  ";

            SyntaxNode lastChild = node.GetChildren().LastOrDefault();
            foreach (SyntaxNode child in node.GetChildren()) 
                PrintSyntaxNodeTree(child, indent, child == lastChild);
        }
    }
}