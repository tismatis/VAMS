namespace ConsoleApp1.Symbol
{
    public class Return : Symbol
    {
        public Return() : base(null) { }

        public Return(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime => runtime.Address = runtime.Function.MaxStackSize-1;
        }

        public override string GetCommand() => "RETURN";
    }
}