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
        
            _address = int.Parse(args[0]);
        }

        public override string GetCommand() => "JUMP_IF_TRUE";
        
        public override void Execute(FunctionRuntime runtime)
        {
            if ((bool)runtime.Stack.Pop())
                runtime.Address = _address;
        }
    }
}