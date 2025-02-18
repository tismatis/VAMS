using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class MainThreadTask : Symbol
    {
        public MainThreadTask() : base(null) {}

        public MainThreadTask(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            var address = int.Parse(args[0]);
            Execute = runtime =>
            {
                runtime.Vm.MainThreadTasks.Enqueue(() =>
                {
                    try
                    {
                        FunctionRuntime newRuntime = new FunctionRuntime(runtime);
                        runtime.Function.Execute(newRuntime, address);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                });
            };
        }

        public override string GetCommand() => "MAIN_THREAD_TASK";
    }
}