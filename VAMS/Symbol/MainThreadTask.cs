using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class MainThreadTask : Symbol
    {
        private int _address;
        private int _count = 1;
        public MainThreadTask() : base(null) {}

        public MainThreadTask(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            _address = int.Parse(args[0]);
        }

        public override string GetCommand() => "MAIN_THREAD_TASK";
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Execute(FunctionRuntime runtime)
        {
            for(int i = 0; i!=_count; i++)
            {
                runtime.Vm.MainThreadTasks.Enqueue(() =>
                {
                    try
                    {
                        FunctionRuntime newRuntime = new FunctionRuntime(runtime);
                        runtime.Function.Execute(newRuntime, _address);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                });
            }
        }
    }
}