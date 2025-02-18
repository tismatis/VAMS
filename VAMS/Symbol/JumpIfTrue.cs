namespace ConsoleApp1.Symbol
{
    public class JumpIfTrue : Symbol
    {
        private int _address;

        public JumpIfTrue() : base(null) {}

        public JumpIfTrue(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            var address = int.Parse(args[0]);
            Execute = runtime => {if((bool)runtime.Stack.Pop()) runtime.Address = address;};
        }

        public override string GetCommand() => "JUMP_IF_TRUE";
    }
}