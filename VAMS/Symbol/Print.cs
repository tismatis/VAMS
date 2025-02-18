namespace ConsoleApp1.Symbol
{
    public class Print : Symbol
    {
        public Print() : base(null) {}

        public Print(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime => VAMSInterface.OnConsoleOutput(runtime.Stack.Peek());
        }

        public override string GetCommand() => "PRINT";
    }
}