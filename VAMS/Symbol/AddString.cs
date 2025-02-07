using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class AddString : Symbol
    {
        public AddString() : base(null) {}

        public AddString(string[] args) : base(args) {}

        public override string GetCommand() => "ADD_STRING";
        
        public override void Execute(FunctionRuntime runtime)
        {
            runtime.Stack.Push(runtime.Stack.Pop().ToString() + runtime.Stack.Pop().ToString());
        }
    }
}