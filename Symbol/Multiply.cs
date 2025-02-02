namespace ConsoleApp1.Symbol
{
    public class Multiply : Symbol
    {
        public Multiply() : base(null) {}

        public Multiply(string[] args) : base(args) {}

        public override string GetCommand() => "MUL";

        public override void Execute(FunctionRuntime runtime, ref int address)
        {
            var a = runtime.Stack.Pop();
            var b = runtime.Stack.Pop();
            runtime.Stack.Push((int)a * (int)b);
        }
    }
}
