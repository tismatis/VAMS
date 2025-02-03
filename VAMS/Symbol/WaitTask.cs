using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol;

public class WaitTask : Symbol
{
    public WaitTask() : base(null) {}

    public WaitTask(string[] args) : base(args) {}

    public override string GetCommand() => "WAIT_TASK";
        
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void Execute(FunctionRuntime runtime)
    {
        runtime.TaskWait = true;
    }
}