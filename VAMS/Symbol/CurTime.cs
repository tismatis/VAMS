using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class CurTime : Symbol
    {
        public CurTime() : base(null) {}

        public CurTime(string[] args) : base(args) {}

        public override string GetCommand() => "CURTIME";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime)
        {
            runtime.Stack.Push(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
    }
}