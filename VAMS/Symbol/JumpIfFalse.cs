namespace ConsoleApp1.Symbol
{
    public class JumpIfFalse : Symbol
    {
        private int _address;

        public JumpIfFalse() : base(null) {}

        public JumpIfFalse(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            _address = int.Parse(args[0]);
        }

        public override string GetCommand() => "JUMP_IF_FALSE";
        
        public override void Execute(FunctionRuntime runtime)
        {
            if (!(bool)runtime.Stack.Pop())
                runtime.Address = _address;
        }
    }
}