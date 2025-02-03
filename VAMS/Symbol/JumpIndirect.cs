namespace ConsoleApp1.Symbol;

public class JumpIndirect : Symbol
{
    public JumpIndirect() : base(null) {}

    public JumpIndirect(string[] args) : base(args) {}

    public override string GetCommand() => "JUMP_INDIRECT";
        
    public override void Execute(FunctionRuntime runtime)
    {
        runtime.Address = (int)runtime.Stack.Pop();
    }
}