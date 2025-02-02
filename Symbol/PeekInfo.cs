namespace ConsoleApp1.Symbol;

public class PeekInfo : Symbol
{
    public PeekInfo() : base(null) {}

    public PeekInfo(string[] args) : base(args) {}

    public override string GetCommand() => "PEEK_INFO";

    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        var val = runtime.Stack.Peek();
        Console.WriteLine($"Peek value '{val}' of type {val.GetType()}");
    }
}
