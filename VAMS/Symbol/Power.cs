using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Power : Symbol
    {
        private readonly Action<FunctionRuntime> _execute;

        public Power() : base(null) {}

        public Power(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            if (args.Length != 1)
                throw new InvalidOperationException("POW REQUIRES EXACTLY 1 ARGUMENT");
            
            Type type;
            try
            {
                type = Type.GetType(args[0]);
            }
            catch
            {
                throw new InvalidOperationException("POW REQUIRES A TYPE");
            }
            
            if(type == typeof(int))
                _execute = runtime => { runtime.Stack.Push(Math.Pow((int)runtime.Stack.Pop(), (int)runtime.Stack.Pop())); };
            else if(type == typeof(float))
                _execute = runtime => { runtime.Stack.Push(Math.Pow((float)runtime.Stack.Pop(), (float)runtime.Stack.Pop())); };
            else
                throw new InvalidOperationException("POW NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "POW";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime) => _execute.Invoke(runtime);
    }
}
