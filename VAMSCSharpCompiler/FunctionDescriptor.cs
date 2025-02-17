using System.Collections.Generic;

namespace VAMSCSharpCompiler
{
    public class FunctionDescriptor
    {
        public string Name;
        public List<GenericSymbol> Symbols;

        public ConsoleApp1.Stack<string> Stack;
        
        public FunctionDescriptor(string name)
        {
            Name = name;
            Symbols = new List<GenericSymbol>();
            Stack = new ConsoleApp1.Stack<string>();
        }
    }
}