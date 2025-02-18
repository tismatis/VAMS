using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Not : Symbol
    {
        public Not() : base(null) {}

        public Not(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime => runtime.Stack.Push(!(bool)runtime.Stack.Pop());
        }

        public override string GetCommand() => "NOT";
    }

}