using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol;

public class Increment : Symbol
{
    public Increment() : base(null) {}

    public Increment(string[] args) : base(args) {}

    public override string GetCommand() => "INCREMENT";
        
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        if (runtime.Stack.Pop() is int a)
        {
            runtime.Stack.Push(a + 1);
        }
        else
        {
            throw new NotImplementedException("INCREMENT NOT IMPLEMENTED FOR THIS");
        }
    }
}