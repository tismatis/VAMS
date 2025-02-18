namespace ConsoleApp1.Symbol
{
    public class ToString : Symbol
    {
        public ToString() : base(null) {}

        public ToString(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            Execute = runtime => runtime.Stack.Push(runtime.Stack.Pop().ToString());
        }

        public override string GetCommand() => "TOSTRING";
    }
}