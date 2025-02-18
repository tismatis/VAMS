using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Switch: Symbol
    {
        public Switch() : base(null) {}

        public Switch(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime => runtime.Stack.Switch(runtime.Stack.Count - 2, runtime.Stack.Count - 1);
        }

        public override string GetCommand() => "SWITCH";
    }
}