namespace ConsoleApp1.Symbol;

public class AddDecimal : Symbol
{
    public AddDecimal() : base(null) {}

    public AddDecimal(string[] args) : base(args) {}

    public override string GetCommand() => "ADD_DECIMAL";
        
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        var a = runtime.Stack.Pop();
        var b = runtime.Stack.Pop();
        runtime.Stack.Push((double)a + (double)b);
    }
}