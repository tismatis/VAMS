using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Add : Symbol
    {
        public Add() : base(null) {}

        public Add(string[] args) : base(args) {}

        public override string GetCommand() => "ADD";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime)
        {
            if (runtime.Stack.Pop() is int a && runtime.Stack.Pop() is int b)
                runtime.Stack.Push(a + b);
            else
                throw new NotImplementedException("ADD NOT IMPLEMENTED FOR THIS");
        }
    }
}