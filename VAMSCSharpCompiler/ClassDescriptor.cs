using System.Collections.Generic;

namespace VAMSCSharpCompiler
{
    public class ClassDescriptor
    {
        public List<FunctionDescriptor> Functions;
        public string Name;
        public string Namespace;

        public ClassDescriptor(string name, string @namespace = "")
        {
            Name = name;
            Namespace = @namespace;
            Functions = new List<FunctionDescriptor>();
        }
        
        public string GetFullName()
        {
            if(Namespace == "")
                return Name;
            return Namespace + "." + Name;
        }
    }
}