using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ConsoleApp1;

public class FunctionObject
{
    public string Name;
    public ImmutableArray<Symbol.Symbol> Symbols = new();
    public int MaxStackSize;
    public bool IsAsync;
    
    public FunctionObject(string name, bool isAsync, List<Symbol.Symbol> symbols)
    {
        Name = name;
        IsAsync = isAsync;
        Symbols = symbols.ToImmutableArray();
    }

    public void Setup()
    {
        // if eeded
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(FunctionRuntime runtime, params object[] args)
    {
        runtime.Function = this;
        Stopwatch sw = new();
        int length = Symbols.Length;
        MaxStackSize = length;
        sw.Start();
        for (runtime.Address = 0; runtime.Address != length; runtime.Address++)
            Symbols[runtime.Address].Execute(runtime);
        sw.Stop();
        Console.WriteLine($"Function {Name} executed in {sw.ElapsedMilliseconds}ms");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ExecuteAsync(FunctionRuntime runtime, params object[] args)
    {
        runtime.Function = this;
        Stopwatch sw = new();
        int length = Symbols.Length;
        MaxStackSize = length;
        sw.Start();
        for (runtime.Address = 0; runtime.Address != length; runtime.Address++)
        {
            if (runtime.TaskWait)
            {
                if(runtime.TaskWaitAll)
                    while (true)
                    {
                        var stackObj = runtime.Stack.Pop();
                        if(stackObj is Task task)
                            task.Wait();
                        else
                        {
                            runtime.Stack.Push(stackObj);
                            break;
                        }
                    }
                else if(runtime.Stack.Pop() is Task task)
                    task.Wait();
                runtime.TaskWaitAll = false;
                runtime.TaskWait = false;
            }
            Symbols[runtime.Address].Execute(runtime);
        }
        sw.Stop();
        Console.WriteLine($"Function {Name} executed in {sw.ElapsedMilliseconds}ms");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(FunctionRuntime runtime, int defaultAddress, params object[] args)
    {
        int length = Symbols.Length;
        MaxStackSize = length;
        for (runtime.Address = defaultAddress; runtime.Address != length; runtime.Address++)
            Symbols[runtime.Address].Execute(runtime);
    }
}

public class FunctionRuntime
{
    public Stack<object> Stack;
    public List<Task> Task;
    public FunctionObject Function;
    public bool TaskWait;
    public bool TaskWaitAll;
    public int Address;

    public FunctionRuntime()
    {
        Stack = new();
        Task = new();
    }

    public FunctionRuntime(FunctionRuntime runtime)
    {
        Stack = new(runtime.Stack);
        Task = new();
        Function = runtime.Function;
    }
}