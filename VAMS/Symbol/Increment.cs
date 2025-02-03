using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol;

public class Increment : Symbol
{
    public Increment() : base(null) {}

    public Increment(string[] args) : base(args) {}

    public override string GetCommand() => "INCREMENT";
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Execute(FunctionRuntime runtime)
    {
        runtime.Stack.Push((int)runtime.Stack.Pop() + 1);
    }
}