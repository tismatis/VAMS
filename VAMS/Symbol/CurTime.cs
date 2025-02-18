using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class CurTime : Symbol
    {
        public CurTime() : base(null) {}

        public CurTime(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime => runtime.Stack.Push(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public override string GetCommand() => "CURTIME";
    }
}