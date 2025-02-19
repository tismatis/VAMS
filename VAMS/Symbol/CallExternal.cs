using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApp1.Symbol
{
    public class CallExternal : Symbol
    {
        public string Path;
        public string Name;
        private bool ShouldPush;
        private MethodInfo _method;

        public CallExternal() : base(null) {}

        public CallExternal(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Path = args[0];
            Name = args[1];
            if (args.Length > 1)
                ShouldPush = bool.Parse(args[2]);
            string argsType = ""; // "Type1;Type2;Type3"
            if(args.Length > 2)
                argsType = args[3];
            
            var type = Type.GetType(Path);
            if (type == null)
                throw new InvalidOperationException($"Type '{Path}' not found.");

            List<Type> types = new List<Type>();
            foreach (var typeArg in argsType.Split(';'))
            {
                var typeArgType = Type.GetType(typeArg);
                if (typeArgType == null)
                    throw new InvalidOperationException($"Type '{typeArg}' not found.");
                types.Add(typeArgType);
            }
            
            if(String.IsNullOrEmpty(argsType))
                _method = type.GetMethod(Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            else
                _method = type.GetMethod(Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, types.ToArray(), null);
            if (_method == null)
                throw new InvalidOperationException($"Method '{Path}.{Name}' not found.");
            
            List<object> arguments = new List<object>();
            int count = _method.GetParameters().Length;
            
            Execute = runtime =>
            {
                VAMSInterface.OnConsoleOutput("Executing external function " + Path + "." + Name);
                
                for (int i = count; i > 0; i--)
                    arguments.Add(runtime.Stack.Peek((byte)((byte)runtime.Stack.Count-i)));
                
                var obj = _method.Invoke(null, arguments.ToArray());
                if (ShouldPush)
                    runtime.Stack.Push(obj);
            };
        }

        public override string GetCommand() => "CALL_EXTERNAL";
    }
}