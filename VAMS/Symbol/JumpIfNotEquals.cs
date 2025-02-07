using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class JumpIfNotEquals : Symbol
    {
        private int _address;

        public JumpIfNotEquals() : base(null) {}

        public JumpIfNotEquals(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            _address = int.Parse(args[0]);
        }

        public override string GetCommand() => "JUMP_IF_NOT_EQUALS";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime)
        {
            if ((int)runtime.Stack.Peek() != (int)runtime.Stack.Peek(1))
                runtime.Address = _address;
        }
    }
}