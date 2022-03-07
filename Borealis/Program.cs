using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Compilation compilation = new Compilation(syntaxTree);
                EvaluationResult result = compilation.Evaluate();

                IReadOnlyList<Diagnostic> diagnostics = result.Diagnostics;
                
                // PrintSyntaxNodeTree(syntaxTree.Root);

                if (!diagnostics.Any()) {
                    Console.WriteLine(result.Value);
                } else {
                    foreach (Diagnostic diagnostic in diagnostics) {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        string prefix = line?.Substring(0, diagnostic.Span.Start);
                        string error = line?.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        string suffix = line?.Substring(diagnostic.Span.End);

                        Console.Write("     ");
                        Console.Write(prefix);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();
                        Console.Write(suffix);
                        Console.WriteLine();
                    }
                    
                    Console.WriteLine();
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void PrintSyntaxNodeTree(SyntaxNode node, string indent = "", bool isLast = true) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            
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
            
            Console.ResetColor();
        }
    }
}