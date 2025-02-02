namespace ConsoleApp1.Symbol
{
    public class Divide : Symbol
    {
        public Divide() : base(null) {}

        public Divide(string[] args) : base(args) {}

        public override string GetCommand() => "DIV";

        public override void Execute(FunctionRuntime runtime)
        {
            var a = runtime.Stack.Pop();
            var b = runtime.Stack.Pop();
            if ((int)b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            runtime.Stack.Push((double)a / (double)b);
        }
    }
}