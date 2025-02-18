namespace ConsoleApp1.Symbol
{
    public class ComeDown: Symbol
    {
        public int Index;

        public ComeDown() : base(null) {}

        public ComeDown(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Index = byte.Parse(args[0]);
            
            Execute = runtime => runtime.Stack.Switch(Index, runtime.Stack.Count - 1);
        }

        public override string GetCommand() => "COME_DOWN";
    }
}