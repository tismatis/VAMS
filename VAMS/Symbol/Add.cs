using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Add : Symbol
    {
        private readonly Action<FunctionRuntime> _execute;
        
        public Add() : base(null) {}

        public Add(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            if (args.Length != 1)
                throw new InvalidOperationException("ADD REQUIRES EXACTLY 1 ARGUMENT");

            Type type;
            try
            {
                type = Type.GetType(args[0]);
            }
            catch
            {
                throw new InvalidOperationException("ADD REQUIRES A TYPE");
            }
            
            if(type == typeof(int))
                _execute = runtime => { runtime.Stack.Push((int)runtime.Stack.Pop() + (int)runtime.Stack.Pop()); };
            else if(type == typeof(float))
                _execute = runtime => { runtime.Stack.Push((float)runtime.Stack.Pop() + (float)runtime.Stack.Pop()); };
            else if(type == typeof(string))
                _execute = runtime => { runtime.Stack.Push((string)runtime.Stack.Pop() + (string)runtime.Stack.Pop()); };
            else
                throw new InvalidOperationException("ADD NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "ADD";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime) => _execute.Invoke(runtime);
        /*{
            if (runtime.Stack.Pop() is int a && runtime.Stack.Pop() is int b)
                runtime.Stack.Push(a + b);
            else
                throw new NotImplementedException("ADD NOT IMPLEMENTED FOR THIS");
        }*/
    }
}