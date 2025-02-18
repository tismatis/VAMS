namespace ConsoleApp1.Symbol
{
    public class JumpIfFalse : Symbol
    {
        public JumpIfFalse() : base(null) {}

        public JumpIfFalse(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            var address = int.Parse(args[0]);

            Execute = runtime => {if(!(bool)runtime.Stack.Pop()) runtime.Address = address;};
        }

        public override string GetCommand() => "JUMP_IF_FALSE";
    }
}