using System.Collections.Generic;

namespace VAMSCSharpCompiler
{
    public class FunctionDescriptor
    {
        public string Name;
        public List<GenericSymbol> Symbols;

        public VAMS.Stack<string> Stack;
        
        public FunctionDescriptor(string name)
        {
            Name = name;
            Symbols = new List<GenericSymbol>();
            Stack = new VAMS.Stack<string>();
        }
    }
}