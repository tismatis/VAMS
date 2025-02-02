namespace ConsoleApp1.Symbol;

public class Print : Symbol
{
    public Print() : base(null) {}

    public Print(string[] args) : base(args) {}

    public override string GetCommand() => "PRINT";
        
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        Console.WriteLine(runtime.Stack.Peek());
    }
}