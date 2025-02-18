namespace ConsoleApp1.Symbol
{
    public class Jump : Symbol
    {
        public Jump() : base(null) {}

        public Jump(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            var address = int.Parse(args[0]);
            
            Execute = runtime => runtime.Address = address;
        }

        public override string GetCommand() => "JUMP";
    }
}