namespace ConsoleApp1.Symbol
{
    public class Power : Symbol
    {
        public Power() : base(null) {}

        public Power(string[] args) : base(args) {}

        public override string GetCommand() => "POW";

        public override void Execute(FunctionRuntime runtime, ref int address)
        {
            var a = runtime.Stack.Pop();
            var b = runtime.Stack.Pop();
            if ((int)a == 0)
            {
                throw new DivideByZeroException("Cannot power zero");
            }
            if ((int)b == 0)
            {
                throw new DivideByZeroException("Cannot power by zero");
            }
            runtime.Stack.Push(Math.Pow((int)a, (int)b));
        }
    }
}
