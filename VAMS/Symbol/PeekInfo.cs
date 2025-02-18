using System;

namespace ConsoleApp1.Symbol
{
    public class PeekInfo : Symbol
    {
        public PeekInfo() : base(null) {}

        public PeekInfo(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime =>
            {
                var val = runtime.Stack.Peek();
                VAMSInterface.OnConsoleOutput($"Peek value '{val}' of type {val.GetType()}");
            };
        }

        public override string GetCommand() => "PEEK_INFO";
    }

}