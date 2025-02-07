using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Insert : Symbol
    {
        public object value;
    
        public Insert() : base(null) {}

        public Insert(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            var type = Type.GetType(args[0]);
            if (type == null)
                throw new InvalidOperationException($"Type '{args[0]}' not found.");

            value = Convert.ChangeType(args[1], type);
        }

        public override string GetCommand() => "INSERT";
        
        public override void Execute(FunctionRuntime runtime)
        {
            runtime.Stack.Push(value);
        }
    }
}