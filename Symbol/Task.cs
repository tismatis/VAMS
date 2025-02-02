using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol;

public class Task : Symbol
{
    private int _address;
    private int _count = 1;
    public Task() : base(null) {}

    public Task(string[] args) : base(args)
    {
        if (Parser.Loading == ParserState.Loading)
            return;
        
        _address = int.Parse(args[0]);
        if(args.Length == 2)
            _count = int.Parse(args[0]);
    }

    public override string GetCommand() => "TASK";
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Execute(FunctionRuntime runtime)
    {
        for(int i = 0; i!=_count; i++)
        {
            runtime.Stack.Push(System.Threading.Tasks.Task.Factory.StartNew(() =>
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
            }));
        }
    }
}