using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VAMSCSharpCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = @"
using System;

namespace VAMSCSharpCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var nbr = 5;
            Console.WriteLine(""Hello World!"");
            var k = ""uwuu"";
            Console.WriteLine(nbr);
            Test();
        }

        static void Test()
        {
            Console.WriteLine(""Test"");
        }
    }
}";
            // Parse the content
            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            // Walk the syntax tree
            var walker = new MySyntaxWalker();
            walker.Visit(root);

            // Process the queue after the first analysis
            walker.ProcessQueue();

            // Output the class descriptors
            foreach (var classDescriptor in walker.ClassDescriptors)
            {
                Console.WriteLine($"Class: {classDescriptor.Name}");
                foreach (var functionDescriptor in classDescriptor.Functions)
                {
                    Console.WriteLine($"  Function: {functionDescriptor.Name}");
                    foreach (var symbol in functionDescriptor.Symbols)
                    {
                        Console.WriteLine($"    Symbol: {symbol.ToString()}");
                    }
                }
            }
        }
    }

    class MySyntaxWalker : CSharpSyntaxWalker
    {
        public List<ClassDescriptor> ClassDescriptors { get; } = new List<ClassDescriptor>();
        private Queue<(string methodName, string fullName, FunctionDescriptor functionDescriptor, List<ArgumentSyntax> arguments)> methodCallQueue = new Queue<(string, string, FunctionDescriptor, List<ArgumentSyntax>)>();
        private Dictionary<string, string> declaredVariables = new Dictionary<string, string>();

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var className = node.Identifier.Text;
            var classDescriptor = new ClassDescriptor(className);
            ClassDescriptors.Add(classDescriptor);

            base.VisitClassDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var methodName = node.Identifier.Text;
            var functionDescriptor = new FunctionDescriptor(methodName);

            // Add the function descriptor to the last class descriptor
            if (ClassDescriptors.Count > 0)
            {
                ClassDescriptors[ClassDescriptors.Count - 1].Functions.Add(functionDescriptor);
            }

            base.VisitMethodDeclaration(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var methodName = node.Expression.ToString();

            // Get the class and namespace
            var classNode = node.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            var namespaceNode = node.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            if (classNode != null && namespaceNode != null)
            {
                var className = classNode.Identifier.Text;
                var namespaceName = namespaceNode.Name.ToString();
                var fullMethodName = $"{namespaceName}.{className}.{methodName}";

                // Get the arguments
                var arguments = node.ArgumentList.Arguments.Select(arg => arg).ToList();

                // Add the method call to the queue
                if (ClassDescriptors.Count > 0 && ClassDescriptors[ClassDescriptors.Count - 1].Functions.Count > 0)
                {
                    var lastClass = ClassDescriptors[ClassDescriptors.Count - 1];
                    var lastFunction = lastClass.Functions[lastClass.Functions.Count - 1];
                    methodCallQueue.Enqueue((methodName, fullMethodName, lastFunction, arguments));
                }
            }

            base.VisitInvocationExpression(node);
        }

        public void ProcessQueue()
        {
            while (methodCallQueue.Count > 0)
            {
                var (methodName, fullName, functionDescriptor, arguments) = methodCallQueue.Dequeue();
                var lastClass = ClassDescriptors[ClassDescriptors.Count - 1];

                // Check if the method is local
                bool isLocal = lastClass.Functions.Any(f => f.Name == methodName);
                var args = new List<string> { isLocal ? fullName : methodName };

                if(arguments.Count > 0)
                    args.Add("true");

                string argsType = "";
                
                foreach (var argument in arguments)
                {
                    if (!declaredVariables.ContainsKey(argument.ToString()))
                    {
                        var type = argument.Expression.Kind() == SyntaxKind.StringLiteralExpression
                            ? "System.String"
                            : "unknown";
                        functionDescriptor.Symbols.Add(new GenericSymbol("INSERT", new List<string> { type, argument.ToString() }));
                        argsType += type + ";";
                    }
                    else
                    {
                        var type = declaredVariables[argument.ToString()];
                        argsType += type + ";";
                        if (functionDescriptor.Stack.Peek() != argument.ToString())
                        {
                            var pos = Array.IndexOf(functionDescriptor.Stack.ToArray(), argument.ToString());
                            functionDescriptor.Symbols.Add(new GenericSymbol("COME_DOWN", new List<string> { pos.ToString() }));
                            functionDescriptor.Stack.Switch(pos, functionDescriptor.Stack.Count - 1);
                        }
                    }
                }
                argsType = argsType.TrimEnd(';');
                args.Add(argsType);

                functionDescriptor.Symbols.Add(new GenericSymbol(isLocal ? "CALL_INTERNAL" : "CALL_EXTERNAL", args));
            }
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            var variableDeclaration = node.Declaration;
            var variableType = variableDeclaration.Type.ToString();

            foreach (var variable in variableDeclaration.Variables)
            {
                var variableName = variable.Identifier.Text;
                var variableValue = variable.Initializer?.Value.ToString() ?? "null";
                var variableTypeSyntax = variable.Initializer?.Value.Kind();

                // Add the variable to the declared variables dictionary
                declaredVariables[variableName] = TypeConverter.FromVariableDeclarationSyntax(variable);

                // Create the symbol and add it to the last function descriptor
                if (ClassDescriptors.Count > 0 && ClassDescriptors[ClassDescriptors.Count - 1].Functions.Count > 0)
                {
                    var lastClass = ClassDescriptors[ClassDescriptors.Count - 1];
                    var lastFunction = lastClass.Functions[lastClass.Functions.Count - 1];
                    var args = new List<string> { declaredVariables[variableName], variableValue };
                    lastFunction.Symbols.Add(new GenericSymbol("INSERT", args));
                    lastFunction.Stack.Push(variableName);
                }
            }

            base.VisitLocalDeclarationStatement(node);
        }

        public override void Visit(SyntaxNode node)
        {
            Console.WriteLine($"Visited node: {node.Kind()}");
            base.Visit(node);
        }
    }

    static class TypeConverter
    {
        public static string FromVariableDeclarationSyntax(VariableDeclaratorSyntax variable)
        {
            var kind = variable.Initializer?.Value.Kind();
            if (kind == SyntaxKind.NumericLiteralExpression)
            {
                return "System.Int32";
            }
            else if (kind == SyntaxKind.StringLiteralExpression)
            {
                return "System.String";
            }
            else
            {
                return "unknown";
            }
        }
    }
}