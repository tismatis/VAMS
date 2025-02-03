namespace ConsoleApp1.Symbol;

public class Not : Symbol
{
    public Not() : base(null) {}

    public Not(string[] args) : base(args) {}

    public override string GetCommand() => "NOT";

    public override void Execute(FunctionRuntime runtime)
    {
        if(runtime.Stack.Pop() is bool b)
            runtime.Stack.Push(!b);
    }
}
