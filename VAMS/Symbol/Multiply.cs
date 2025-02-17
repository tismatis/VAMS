using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Multiply : Symbol
    {
        private readonly Action<FunctionRuntime> _execute;
        
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
                _execute = runtime => { runtime.Stack.Push((int)runtime.Stack.Pop() * 1); };
            else if(type == typeof(float))
                _execute = runtime => { runtime.Stack.Push((float)runtime.Stack.Pop() * 1); };
            else
                throw new InvalidOperationException("MUL NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "MUL";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime) => _execute.Invoke(runtime);
    }
}
