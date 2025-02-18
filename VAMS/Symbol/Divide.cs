using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Divide : Symbol
    {
        public Divide() : base(null) {}

        public Divide(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            if (args.Length != 1)
                throw new InvalidOperationException("DIV REQUIRES EXACTLY 1 ARGUMENT");
            
            Type type;
            try
            {
                type = Type.GetType(args[0]);
            }
            catch
            {
                throw new InvalidOperationException("DIV REQUIRES A TYPE");
            }
            
            if(type == typeof(double))
                Execute = runtime => runtime.Stack.Push((double)runtime.Stack.Pop() / (double)runtime.Stack.Pop());
            else if(type == typeof(float))
                Execute = runtime => runtime.Stack.Push((float)runtime.Stack.Pop() / (float)runtime.Stack.Pop());
            else
                throw new InvalidOperationException("DIV NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "DIV";
    }
}