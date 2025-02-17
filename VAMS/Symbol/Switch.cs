using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Switch: Symbol
    {
        public Switch() : base(null) {}

        public Switch(string[] args) : base(args) {}

        public override string GetCommand() => "SWITCH";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime)
        {
            /*var a = runtime.Stack.Pop();
            var b = runtime.Stack.Pop();
            runtime.Stack.Push(a);
            runtime.Stack.Push(b);*/
            runtime.Stack.Switch(runtime.Stack.Count - 2, runtime.Stack.Count - 1);
        }
    }
}