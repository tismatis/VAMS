namespace ConsoleApp1.Symbol;

public class Pop : Symbol
{
    public Pop() : base(null) {}

    public Pop(string[] args) : base(args) {}

    public override string GetCommand() => "POP";
        
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        runtime.Stack.Pop();
    }
}