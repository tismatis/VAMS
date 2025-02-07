namespace ConsoleApp1.Symbol
{
    public class Sub : Symbol
    {
        public Sub() : base(null) {}

        public Sub(string[] args) : base(args) {}

        public override string GetCommand() => "SUB";
        
        public override void Execute(FunctionRuntime runtime)
        {
            var a = runtime.Stack.Pop();
            var b = runtime.Stack.Pop();
            runtime.Stack.Push((long)a - (long)b);
        }
    }
}