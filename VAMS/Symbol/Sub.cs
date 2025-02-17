using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Sub : Symbol
    {
        private readonly Action<FunctionRuntime> _execute;
        
        public Sub() : base(null) {}

        public Sub(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            if (args.Length != 1)
                throw new InvalidOperationException("SUB REQUIRES EXACTLY 1 ARGUMENT");
            
            Type type;
            try
            {
                type = Type.GetType(args[0]);
            }
            catch
            {
                throw new InvalidOperationException("SUB REQUIRES A TYPE");
            }
            
            if(type == typeof(short))
                _execute = runtime => { runtime.Stack.Push((short)runtime.Stack.Pop() - (short)runtime.Stack.Pop()); };
            else if(type == typeof(int))
                _execute = runtime => { runtime.Stack.Push((int)runtime.Stack.Pop() - (int)runtime.Stack.Pop()); };
            else if(type == typeof(long))
                _execute = runtime => { runtime.Stack.Push((long)runtime.Stack.Pop() - (long)runtime.Stack.Pop()); };
            else if(type == typeof(float))
                _execute = runtime => { runtime.Stack.Push((float)runtime.Stack.Pop() - (float)runtime.Stack.Pop()); };
            else
                throw new InvalidOperationException("SUB NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "SUB";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime) => _execute.Invoke(runtime);
    }
}