using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Not : Symbol
    {
        public Not() : base(null) {}

        public Not(string[] args) : base(args) {}

        public override string GetCommand() => "NOT";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime)
        {
            runtime.Stack.Push(!(bool)runtime.Stack.Pop());
        }
    }

}