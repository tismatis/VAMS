using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Task : Symbol
    {
        public Task() : base(null) {}

        public Task(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            var address = int.Parse(args[0]);
            var count = 1;
            if(args.Length == 2)
                count = int.Parse(args[0]);

            Execute = runtime =>
            {
                for (int i = 0; i != count; i++)
                {
                    runtime.Stack.Push(System.Threading.Tasks.Task.Factory.StartNew(() =>
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
                    }));
                }
            };
        }

        public override string GetCommand() => "TASK";
    }
}