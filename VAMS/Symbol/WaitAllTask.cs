using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class WaitAllTask : Symbol
    {
        public WaitAllTask() : base(null) {}

        public WaitAllTask(string[] args) : base(args) {}

        public override string GetCommand() => "WAIT_ALL_TASK";
        
        public override void Execute(FunctionRuntime runtime)
        {
            runtime.TaskWait = true;
            runtime.TaskWaitAll = true;
        }
    }
}