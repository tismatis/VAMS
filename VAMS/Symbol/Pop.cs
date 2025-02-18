namespace ConsoleApp1.Symbol
{
    public class Pop : Symbol
    {
        public Pop() : base(null) {}

        public Pop(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime => runtime.Stack.Pop();
        }

        public override string GetCommand() => "POP";
    }
}