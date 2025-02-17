using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Equals : Symbol
    {
        private readonly Action<FunctionRuntime> _execute;
        
        public Equals() : base(null) {}

        public Equals(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            if (args.Length != 1)
                throw new InvalidOperationException("EQUALS REQUIRES EXACTLY 1 ARGUMENT");
            
            Type type;
            try
            {
                type = Type.GetType(args[0]);
            }
            catch
            {
                throw new InvalidOperationException("EQUALS REQUIRES A TYPE");
            }
            
            if(type == typeof(int))
                _execute = runtime => { runtime.Stack.Push((int)runtime.Stack.Peek() == (int)runtime.Stack.Peek(1)); };
            else if(type == typeof(float))
                _execute = runtime => { runtime.Stack.Push((float)runtime.Stack.Peek() == (float)runtime.Stack.Peek(1)); };
            else
                throw new InvalidOperationException("EQUALS NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "EQUALS";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime) => _execute.Invoke(runtime);
    }
}