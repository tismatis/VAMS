using System;
using System.Reflection;

namespace ConsoleApp1.Symbol
{
    public class CallExternal : Symbol
    {
        public string Path;
        public string Name;

        public CallExternal() : base(null) {}

        public CallExternal(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Path = args[0];
            Name = args[1];
        }

        public override string GetCommand() => "CALL_EXTERNAL";
        
        public override void Execute(FunctionRuntime runtime, ref int address)
        {
            Console.WriteLine("Executing external function " + Path + "." + Name);

            var type = Type.GetType(Path);
            if (type == null)
                throw new InvalidOperationException($"Type '{Path}' not found.");

            var method = type.GetMethod(Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
                throw new InvalidOperationException($"Method '{Path}.{Name}' not found.");

            method.Invoke(null, new object[]{"Hello, World!"});
        }
    }
}