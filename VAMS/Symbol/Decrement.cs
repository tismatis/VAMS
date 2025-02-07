using System;

namespace ConsoleApp1.Symbol
{
    public class Decrement : Symbol
    {
        public Decrement() : base(null) {}

        public Decrement(string[] args) : base(args) {}

        public override string GetCommand() => "DECREMENT";
        
        public override void Execute(FunctionRuntime runtime)
        {
            if (runtime.Stack.Pop() is int a)
            {
                runtime.Stack.Push(a - 1);
            }
            else
            {
                throw new NotImplementedException("DECREMENT NOT IMPLEMENTED FOR THIS");
            }
        }
    }
}