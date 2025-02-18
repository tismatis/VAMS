using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class WaitTask : Symbol
    {
        public WaitTask() : base(null) {}

        public WaitTask(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            Execute = runtime => runtime.TaskWait = true;
        }

        public override string GetCommand() => "WAIT_TASK";
    }
}