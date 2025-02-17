using System.Collections.Generic;

namespace VAMSCSharpCompiler
{
    public class GenericSymbol
    {
        public string Name;
        public string[] Arguments;
        
        public GenericSymbol(string name, List<string> arguments)
        {
            Name = name;
            Arguments = arguments.ToArray();
        }
        
        public override string ToString()
        {
            string args = "";
            foreach(var arg in Arguments)
                args += (arg.Contains(" ") ? (arg.StartsWith("\"") ? arg : $"\"{arg}\"") : arg) + " ";
            return $"{Name} {args}";
        }
    }
}