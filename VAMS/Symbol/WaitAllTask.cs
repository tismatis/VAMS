using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class WaitAllTask : Symbol
    {
        public WaitAllTask() : base(null) {}

        public WaitAllTask(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Execute = runtime =>
            {
                runtime.TaskWait = true;
                runtime.TaskWaitAll = true;
            };
        }

        public override string GetCommand() => "WAIT_ALL_TASK";
    }
}