using System.Collections.Generic;

namespace VAMSCSharpCompiler
{
    public class ClassDescriptor
    {
        public List<FunctionDescriptor> Functions;
        public string Name;

        public ClassDescriptor(string name)
        {
            Name = name;
            Functions = new List<FunctionDescriptor>();
        }
    }
}