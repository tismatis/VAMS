namespace ConsoleApp1.Symbol;

public class SubDecimal : Symbol
{
    public SubDecimal() : base(null) {}

    public SubDecimal(string[] args) : base(args) {}

    public override string GetCommand() => "SUB_DECIMAL";
        
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        var a = runtime.Stack.Pop();
        var b = runtime.Stack.Pop();
        runtime.Stack.Push((double)a - (double)b);
    }
}