using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol;

public class WaitTask : Symbol
{
    public WaitTask() : base(null) {}

    public WaitTask(string[] args) : base(args) {}

    public override string GetCommand() => "WAIT_TASK";
        
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        /*if(runtime.Stack.Peek() is System.Threading.Tasks.Task task)
        {
            if(task.IsCompleted)
                runtime.Stack.Pop();
            else
            {
                address--;
                runtime.TaskWait = true;
            }
        }*/
        runtime.TaskWait = true;
    }
}