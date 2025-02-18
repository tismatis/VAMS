using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Multiply : Symbol
    {
        public Multiply() : base(null) {}

        public Multiply(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            if (args.Length != 1)
                throw new InvalidOperationException("MUL REQUIRES EXACTLY 1 ARGUMENT");
            
            Type type;
            try
            {
                type = Type.GetType(args[0]);
            }
            catch
            {
                throw new InvalidOperationException("MUL REQUIRES A TYPE");
            }
            
            if(type == typeof(int))
                Execute = runtime => { runtime.Stack.Push((int)runtime.Stack.Pop() * 1); };
            else if(type == typeof(float))
                Execute = runtime => { runtime.Stack.Push((float)runtime.Stack.Pop() * 1); };
            else
                throw new InvalidOperationException("MUL NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "MUL";
    }
}
