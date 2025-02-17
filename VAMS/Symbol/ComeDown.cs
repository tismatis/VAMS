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
        }

        public override string GetCommand() => "COME_DOWN";
        
        public override void Execute(FunctionRuntime runtime)
        {
            runtime.Stack.Switch(Index, runtime.Stack.Count - 1);
        }
    }
}