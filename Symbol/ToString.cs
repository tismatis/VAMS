namespace ConsoleApp1.Symbol;

public class ToString : Symbol
{
    public ToString() : base(null) {}

    public ToString(string[] args) : base(args) {}

    public override string GetCommand() => "TOSTRING";
        
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        runtime.Stack.Push(runtime.Stack.Pop().ToString());
    }
}