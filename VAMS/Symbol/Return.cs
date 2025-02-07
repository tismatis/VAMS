namespace ConsoleApp1.Symbol
{
    public class Return : Symbol
    {
        public Return() : base(null)
        {
        }

        public Return(string[] args) : base(args)
        {
        }

        public override string GetCommand() => "RETURN";

        public override void Execute(FunctionRuntime runtime)
        {
            runtime.Address = runtime.Function.MaxStackSize-1;
        }
    }
}