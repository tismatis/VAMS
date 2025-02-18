namespace ConsoleApp1.Symbol
{
    public class JumpIndirect : Symbol
    {
        public JumpIndirect() : base(null) {}

        public JumpIndirect(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime => runtime.Address = (int)runtime.Stack.Pop();
        }

        public override string GetCommand() => "JUMP_INDIRECT";
    }
}