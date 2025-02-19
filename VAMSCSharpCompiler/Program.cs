using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ConsoleApp1.Symbol;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GenericSymbol = VAMSCSharpCompiler.GenericSymbol;

namespace VAMSCSharpCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = File.ReadAllText("ToCompile.cs");
            
            // Parse the content
            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            // Walk the syntax tree
            var firstPassWalker = new FirstPassSyntaxWalker();
            firstPassWalker.Visit(root);
            var walker = new MySyntaxWalker(firstPassWalker);
            walker.Visit(root);

            // Process the queue after the first analysis
            // walker.ProcessQueue();

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
            
            // Output inside an file
            if(Directory.Exists("Compiled"))
                Directory.Delete("Compiled", true);
            Directory.CreateDirectory("Compiled");
            foreach (var classDescriptor in walker.ClassDescriptors)
            {
                string classContent = "define class Program\n";
                foreach (var function in classDescriptor.Functions)
                {
                    classContent += $"\tdefine method {function.Name}\n";
                    foreach (var symbol in function.Symbols)
                    {
                        classContent += $"\t\t{symbol}\n";
                    }
                    classContent += "\tdefine method_end\n";
                }
                classContent += "define class_end";
                File.WriteAllText($"Compiled/{classDescriptor.Name}.lasil", classContent);
            }
        }
    }

    class FirstPassSyntaxWalker : CSharpSyntaxWalker
    {
        public List<string> Usings { get; } = new List<string>();
        public Dictionary<string, string> Alias { get; } = new Dictionary<string, string>();
        public List<ClassDescriptor> ClassDescriptors { get; } = new List<ClassDescriptor>();
        private string _namespace = "";
        
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            if(node.Alias != null)
                Alias.Add(node.Alias.ToString(), node.Name.ToString());
            else
                Usings.Add(node.Name.ToString());
            base.VisitUsingDirective(node);
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            _namespace = node.Name.ToString();
            base.VisitNamespaceDeclaration(node);
            _namespace = "";
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var className = node.Identifier.Text;
            var classDescriptor = new ClassDescriptor(className, _namespace);
            ClassDescriptors.Add(classDescriptor);

            base.VisitClassDeclaration(node);
        }
        
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var methodName = node.Identifier.Text;
            var functionDescriptor = new FunctionDescriptor(methodName);

            if (ClassDescriptors.Count > 0)
            {
                ClassDescriptors[ClassDescriptors.Count - 1].Functions.Add(functionDescriptor);
            }

            base.VisitMethodDeclaration(node);
        }
    }
    
    class MySyntaxWalker : CSharpSyntaxWalker
    {
        public List<ClassDescriptor> ClassDescriptors { get; } = new List<ClassDescriptor>();
        private Dictionary<string, string> _declaredVariables = new Dictionary<string, string>();
        private string _namespace = "";

        private FirstPassSyntaxWalker _firstPassSyntaxWalker;
        public MySyntaxWalker(FirstPassSyntaxWalker firstPassSyntaxWalker)
        {
            _firstPassSyntaxWalker = firstPassSyntaxWalker;
        }
        
        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            _namespace = node.Name.ToString();
            base.VisitNamespaceDeclaration(node);
            _namespace = "";
        }
        
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var className = node.Identifier.Text;
            var classDescriptor = new ClassDescriptor(className, _namespace);
            ClassDescriptors.Add(classDescriptor);

            base.VisitClassDeclaration(node);
        }
    
        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            _declaredVariables.Clear();
            
            var methodName = node.Identifier.Text;
            var functionDescriptor = new FunctionDescriptor(methodName);

            if (ClassDescriptors.Count > 0)
            {
                ClassDescriptors[ClassDescriptors.Count - 1].Functions.Add(functionDescriptor);
            }

            base.VisitMethodDeclaration(node);
            
            ShowStack();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            string variableType = "unknown";
            var typeName = node.Type.ToString();
            string variableName = node.Variables[0].Identifier.Text;
            
            if(typeName == "var") // node.Variables[0] Should exist at this point, var cannot be compiled without a value.
                variableType = TypeConverter.FromSyntaxKind(node.Variables[0].Initializer.Value.Kind(), node.Variables[0].Initializer.Value.ToString());
            if(variableType == "unknown")
                variableType = TypeConverter.FromShortTypeName(typeName);
            if (variableType == "unknown")
                variableType = TypeConverter.TryFindTypeUsingReflection(typeName, _firstPassSyntaxWalker);
            if(variableType.StartsWith("unknown"))
                throw new InvalidOperationException($"Unknown type: '{typeName}'");
            
            _declaredVariables.Add(node.Variables[0].Identifier.Text, variableType);
            AddSymbol(new GenericSymbol("INSERT", new List<string>{ variableType, node.Variables[0].Initializer.Value.ToString() }), variableName);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            base.VisitInvocationExpression(node);
            
            var argumentTypes = new List<string>();
            foreach (var argument in node.ArgumentList.Arguments)
            {
                var typeInfo = TypeConverter.FromSyntaxKind(argument.Expression.Kind(), argument.Expression.ToString());
                if (typeInfo == "unknown-IdentifierName")
                {
                    if (_declaredVariables.ContainsKey(argument.Expression.ToString()))
                    {
                        typeInfo = _declaredVariables[argument.Expression.ToString()];
                    }
                }
                argumentTypes.Add(typeInfo);
            }

            foreach (var argument in node.ArgumentList.Arguments)
            {
                var typeInfo = TypeConverter.FromSyntaxKind(argument.Expression.Kind(), argument.Expression.ToString());
                if (typeInfo == "unknown-IdentifierName")
                {
                    ApplyOrderNeeded(argument.Expression.ToString());
                    break;
                }
                ApplyOrderNeeded($"LITERAL#{argument.Expression.ToString()}");
            }
            
            var result = TypeConverter.TryFindMethodUsingReflection(node.Expression.ToString(), _firstPassSyntaxWalker);

            var onlyClass = TypeConverter.RemoveMethodFromPath(result.Item1);
            
            Type type = Type.GetType(onlyClass.Item1);
            if(type == null)
                throw new Exception($"Class '{onlyClass.Item1}' not found");

            foreach (var t in type.GetMethods().OrderBy(m => m.GetParameters().Count(p => p.ParameterType == typeof(object))))
            {
                if (t.Name != onlyClass.Item2) continue;
                var args = t.GetParameters();
                    
                if(args.Length != argumentTypes.Count)
                    continue;
                    
                bool found = true;
                for (var i = 0; i < args.Length; i++)
                {
                    if (args[i].ParameterType.FullName != argumentTypes[i])
                    {
                        if (TypeConverter.IsImplicitlyConvertible(args[i].ParameterType, argumentTypes[i]))
                            continue;
                        
                        found = false;
                        break;
                    }
                }

                if (!found) continue;
                
                string argsList = "";
                if (args.Length > 0)
                {
                    foreach (var arg in args)
                        argsList += arg.ParameterType.FullName + ";";
                    argsList = argsList.Remove(argsList.Length - 1);
                }
                AddSymbol(new GenericSymbol($"CALL_EXTERNAL", new List<string>{onlyClass.Item1, onlyClass.Item2, (t.ReturnType != typeof(void)).ToString(), argsList}));
                return;
            }

            foreach (var @class in _firstPassSyntaxWalker.ClassDescriptors)
            {
                if(@class.GetFullName() == onlyClass.Item1)
                {
                    foreach (var function in @class.Functions)
                    {
                        if(function.Name == onlyClass.Item2)
                        {
                            // TODO: Check arguments
                            AddSymbol(new GenericSymbol($"CALL_INTERNAL", new List<string>{result.Item1, "False", ""}));
                            return;
                        }
                    }
                }
            }
            
            throw new Exception($"Method not found: {result.Item1}");
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            //base.VisitLiteralExpression(node);
            // TODO: Move this inside another expression to avoid the "INSERT" symbol
            if(node.Parent.Kind() == SyntaxKind.SimpleAssignmentExpression)
                return;

            var type = TypeConverter.FromSyntaxKind(node.Kind(), node.ToString());
            AddSymbol(new GenericSymbol("INSERT", new List<string>{ type, node.ToString() }), $"LITERAL#{node.ToString()}");
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            //base.VisitPostfixUnaryExpression(node);

            switch (node.OperatorToken.Kind())
            {
                case SyntaxKind.PlusPlusToken:
                    AddSymbol(new GenericSymbol("INCREMENT", new List<string>{ _declaredVariables[node.Operand.ToString()] }));
                    break;
                case SyntaxKind.MinusMinusToken:
                    AddSymbol(new GenericSymbol("DECREMENT", new List<string>{ _declaredVariables[node.Operand.ToString()] }));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown operator: {node.OperatorToken}");
            }
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            var address = GetAddress();
            var list = GetCurrentFunctionDescriptor().Symbols.ToList();
            list.RemoveRange(0, address);

            base.VisitForStatement(node);

            string identifierText = "";
            if (node.Declaration != null)
                identifierText = node.Declaration.Variables[0].Identifier.Text;
            else
                identifierText = ((AssignmentExpressionSyntax)node.Initializers[0]).Left.ToString();
                
            list.AddRange(GetCurrentFunctionDescriptor().Symbols.ToList().GetRange(address, GetCurrentFunctionDescriptor().Symbols.Count - address));
            
            // Detect the end of the for "header"
            // TODO: Detect the end of the for "header" in a better way using the type of the incremented value
            var i = list.FirstOrDefault(x => x.Name == "INCREMENT" || x.Name == "DECREMENT");
            var index = list.IndexOf(i);
            
            if(i == null)
                throw new Exception("Increment/Decrement not found, they are the only supported solutions for now.");
            
            AddSymbol(new GenericSymbol("JUMP_IF_NOT_EQUALS",
                new List<string> { (index+address).ToString(), _declaredVariables[identifierText] }));
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            string wantedModified = node.Left.ToString();
            ApplyOrderNeeded(wantedModified);
            base.VisitAssignmentExpression(node);
            AddSymbol(new GenericSymbol("POP", new List<string>()));
            AddSymbol(new GenericSymbol("INSERT", new List<string>{ _declaredVariables[wantedModified], node.Right.ToString() }), wantedModified);
        }

        public override void Visit(SyntaxNode node)
        {
            Console.WriteLine($"Visited node: {node.Kind()} aka {node.GetType().Name} -> {node.ToString().Replace("\r\n", "")}");
            base.Visit(node);
        }

        /// <summary>
        /// Help the compiler to apply a Stack order like when trying to do a CALL_INTERNAL/CALL_EXTERNAL, we like when it's the good arguments that is pushed.
        /// <param name="identity">It's the default value of the variable wanted.</param>
        /// </summary>
        public void ApplyOrderNeeded(string identity)
        {
            var func = ClassDescriptors[ClassDescriptors.Count - 1]
                .Functions[ClassDescriptors[ClassDescriptors.Count - 1].Functions.Count - 1];
            
            string[] stack = func.Stack.ToArray();
            int index = Array.IndexOf(stack, identity);
            
            if(index == -1)
                throw new Exception("Identity not found in the stack");
            
            if(index == stack.Length - 1)
                return;
            if(index == stack.Length - 2)
            {
                func.Symbols.Add(new GenericSymbol("SWITCH", new List<string>()));
                return;
            }
            
            func.Symbols.Add(new GenericSymbol("COME_DOWN", new List<string>{ index.ToString() }));
        }

        public void ClearStackUntil(string[] allowedIdentity)
        {
            var func = GetCurrentFunctionDescriptor();
            
            string[] stack = func.Stack.ToArray();
            int index = 0;
            foreach(var @object in stack)
            {
                index++;
                if (allowedIdentity.Contains(@object))
                    continue;
                
            }
        }
        
        public void AddSymbol(GenericSymbol symbol, string identity = "")
        {
            if (ClassDescriptors.Count > 0 && ClassDescriptors[ClassDescriptors.Count - 1].Functions.Count > 0)
            {
                var func = GetCurrentFunctionDescriptor();
                func.Symbols.Add(symbol);
                
                // Simulation of the Stack
                switch (symbol.Name)
                {
                    case "INSERT":
                        func.Stack.Push(identity);
                        break;
                    case "POP":
                        func.Stack.Pop();
                        break;
                    case "SWITCH":
                        func.Stack.Switch(func.Stack.Count - 1, func.Stack.Count - 2);
                        break;
                    case "COME_DOWN":
                        func.Stack.Switch(Convert.ToInt32(symbol.Arguments[0]), func.Stack.Count - 1);
                        break;
                }
            }
        }

        public void ShowStack()
        {
            var func = GetCurrentFunctionDescriptor();
            Console.WriteLine("Stack:");
            var array = func.Stack.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                var @object = array[i];
                Console.WriteLine($"  {i}: {@object}");
            }
        }

        public FunctionDescriptor GetCurrentFunctionDescriptor() => ClassDescriptors[ClassDescriptors.Count - 1].Functions[ClassDescriptors[ClassDescriptors.Count - 1].Functions.Count - 1];
        public int GetAddress() => GetCurrentFunctionDescriptor().Symbols.Count - 1;
        public int GetAddress() => GetCurrentFunctionDescriptor().Symbols.Count;
    }

    static class TypeConverter
    {
        public static string FromSyntaxKind(SyntaxKind kind, string value) // TODO: Separate this for detect automatically using values
        {
            switch (kind)
            {
                case SyntaxKind.UnaryMinusExpression:
                    if (value.Contains("."))
                    {
                        var @decimal = Convert.ToDecimal(value);
                        if(@decimal < Convert.ToDecimal(Single.MaxValue) && @decimal > Convert.ToDecimal(Single.MinValue))
                            return "System.Single";
                        if(@decimal < Convert.ToDecimal(Double.MaxValue) && @decimal > Convert.ToDecimal(Double.MinValue))
                            return "System.Double";
                        return "System.Decimal";
                    }
                    
                    var @long = Convert.ToInt64(value);
                    if(@long < Convert.ToInt64(Byte.MaxValue) && @long > Convert.ToInt64(Byte.MinValue))
                        return "System.Byte";
                    if(@long < Convert.ToInt64(Int16.MaxValue) && @long > Convert.ToInt64(Int16.MinValue))
                        return "System.Int16";
                    if(@long < Convert.ToInt64(Int32.MaxValue) && @long > Convert.ToInt64(Int32.MinValue))
                        return "System.Int32";
                    return "System.Int64";
                case SyntaxKind.NumericLiteralExpression:
                    if (value.Contains("."))
                    {
                        double @double;
                        try
                        {
                            @double = Convert.ToDouble(value.Replace(".", ","));
                        }
                        catch
                        {
                            return "System.Decimal";
                        }
                        if(@double < Convert.ToDouble(Single.MaxValue) && @double > Convert.ToDouble(Single.MinValue))
                            return "System.Single";
                        if(@double < Convert.ToDouble(Double.MaxValue) && @double > Convert.ToDouble(Double.MinValue))
                            return "System.Double";
                        return "System.Decimal";
                    }
                        
                    long @ulong;
                    try
                    {
                        @ulong = Convert.ToInt64(value);
                    }
                    catch
                    {
                        return "System.UInt64";
                    }
                    if(@ulong < Convert.ToInt64(SByte.MaxValue) && @ulong > Convert.ToInt64(SByte.MinValue))
                        return "System.SByte";
                    if(@ulong < Convert.ToInt64(Byte.MaxValue) && @ulong > Convert.ToInt64(Byte.MinValue))
                        return "System.Byte";
                    if(@ulong < Convert.ToInt64(Int16.MaxValue) && @ulong > Convert.ToInt64(Int16.MinValue))
                        return "System.Int16";
                    if(@ulong < Convert.ToInt64(UInt16.MaxValue) && @ulong > Convert.ToInt64(UInt16.MinValue))
                        return "System.UInt16";
                    if(@ulong < Convert.ToInt64(Int32.MaxValue) && @ulong > Convert.ToInt64(Int32.MinValue))
                        return "System.Int32";
                    if(@ulong < Convert.ToInt64(UInt32.MaxValue) && @ulong > Convert.ToInt64(UInt32.MinValue))
                        return "System.UInt32";
                    if(@ulong < Convert.ToInt64(Int64.MaxValue) && @ulong > Convert.ToInt64(Int64.MinValue))
                        return "System.Int64";
                    return "System.UInt64";
                case SyntaxKind.StringLiteralExpression:
                    return "System.String";
                case SyntaxKind.CharacterLiteralExpression:
                    return "System.Char";
                default:
                    return "unknown-" + kind;
            }
        }
        
        public static string FromShortTypeName(string typeName)
        {
            switch (typeName)
            {
                case "object": // El patron de .NET
                    return "System.Object";
                case "short":
                    return "System.Int16";
                case "int":
                    return "System.Int32";
                case "long":
                    return "System.Int64";
                case "string":
                    return "System.String";
                default:
                    return "unknown";
            }
        }

        public static string TryFindTypeUsingReflection(string typeName, FirstPassSyntaxWalker walker)
        {
            var type = Type.GetType(typeName);
            int i = 0;
            while (type == null && i != walker.Usings.Count)
            {
                type = Type.GetType(walker.Usings[i] + "." + typeName);
                i++;
            }
            return type != null ? type.FullName : "unknown";
        }

        public static (string, bool) TryFindMethodUsingReflection(string methodName, FirstPassSyntaxWalker walker)
        {
            if (!methodName.Contains("."))
            {
                // local method
                foreach (var classDescriptor in from classDescriptor in walker.ClassDescriptors from functionDescriptor in classDescriptor.Functions where functionDescriptor.Name == methodName select classDescriptor)
                {
                    return ($"{classDescriptor.GetFullName()}.{methodName}", true);
                }
                // TODO: Check inheritors
                throw new Exception($"Method '{methodName}' not found");
            } // external method
            
            string[] parts = methodName.Split('.');
            if (parts.Length == 2) // Only Class+Method
            {
                // TODO: Check in Alias
                var val = walker.Alias.FirstOrDefault(p => p.Key == parts[0]);
                if (!val.Equals(default(KeyValuePair<string, string>)))
                    return ($"{val.Value}.{parts[1]}", false);
                var classDescriptor = walker.ClassDescriptors.FirstOrDefault(x => x.Name == parts[0]);
                if(classDescriptor != null)
                    return ($"{classDescriptor.GetFullName()}.{methodName}", true);
                foreach(var @using in walker.Usings)
                {
                    Type type = Type.GetType(@using + "." + parts[0]);
                    if(type != null)
                        return ($"{type.FullName}.{parts[1]}", false);
                }
            }
            else
            {
                // TODO: Check by full path all locals
                var y = parts.ToList();
                string path = "";
                y.RemoveAt(y.Count - 1);
                for (int i = 0; i != y.Count; i++)
                    if(i == y.Count - 1)
                        path += y[i];
                    else
                        path += y[i] + ".";
                
                Type type = Type.GetType(path);
                if(type != null)
                    return ($"{path}.{parts[parts.Length - 1]}", false);
            }
            
            throw new Exception($"Method '{methodName}' not found");
        }
        
        public static (string, string) RemoveMethodFromPath(string methodName)
        {
            string[] parts = methodName.Split('.');
            var y = parts.ToList();
            string path = "";
            y.RemoveAt(y.Count - 1);
            for (int i = 0; i != y.Count; i++)
                if(i == y.Count - 1)
                    path += y[i];
                else
                    path += y[i] + ".";
            return (path, parts[parts.Length - 1]);
        }
        
        public static bool IsImplicitlyConvertible(Type targetType, string sourceType)
        {
            var source = Type.GetType(sourceType);
            if (source == null) return false;

            if (source == typeof(int) && targetType == typeof(long)) return true;
            if (source == typeof(int) && targetType == typeof(float)) return true;
            if (source == typeof(int) && targetType == typeof(double)) return true;
            if (source == typeof(int) && targetType == typeof(decimal)) return true;
            if (source == typeof(long) && targetType == typeof(float)) return true;
            if (source == typeof(long) && targetType == typeof(double)) return true;
            if (source == typeof(long) && targetType == typeof(decimal)) return true;
            if (source == typeof(float) && targetType == typeof(double)) return true;
            if (source == typeof(float) && targetType == typeof(decimal)) return true;
            if(targetType == typeof(object)) return true;
            return false;
        }
    }
}