using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Equals : Symbol
    {
        public Equals() : base(null) {}

        public Equals(string[] args) : base(args) {}

        public override string GetCommand() => "EQUALS";
        
        public override void Execute(FunctionRuntime runtime)
        {
            if(runtime.Stack.Peek() is int a && runtime.Stack.Peek(1) is int b)
                runtime.Stack.Push(a == b);
            else
                throw new Exception("Cannot compare non-integer values.");
        }
    }
}