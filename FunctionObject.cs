using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1;

public class FunctionObject
{
    public string Name;
    public List<Symbol.Symbol>? Symbols = new();
    private Symbol.Symbol[]? _symbols;
    public int MaxStackSize;
    public bool IsAsync;
    
    public FunctionObject(string name, bool isAsync)
    {
        Name = name;
        IsAsync = isAsync;
    }

    public void Setup()
    {
        if(Symbols == null)
            throw new Exception("Cannot setup function twice.");
        
        _symbols = Symbols.ToArray();
        Symbols.Clear();
        Symbols = null;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(FunctionRuntime runtime, params object[] args)
    {
        runtime.Function = this;
        Stopwatch sw = new();
        Symbol.Symbol[] localSymbols = _symbols;
        int length = localSymbols.Length;
        MaxStackSize = length;
        sw.Start();
        for (int address = 0; address != length; address++)
            localSymbols[address].Execute(runtime, ref address);
        sw.Stop();
        Console.WriteLine($"Function {Name} executed in {sw.ElapsedMilliseconds}ms");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ExecuteAsync(FunctionRuntime runtime, params object[] args)
    {
        runtime.Function = this;
        Stopwatch sw = new();
        Symbol.Symbol[] localSymbols = _symbols;
        int length = localSymbols.Length;
        MaxStackSize = length;
        sw.Start();
        for (int address = 0; address != length; address++)
        {
            if (runtime.TaskWait)
            {
                if(runtime.TaskWaitAll)
                {
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
                    runtime.TaskWaitAll = false;
                }
                else if(runtime.Stack.Pop() is Task task)
                {
                    task.Wait();
                }
                runtime.TaskWait = false;
            }
            localSymbols[address].Execute(runtime, ref address);
        }
        sw.Stop();
        Console.WriteLine($"Function {Name} executed in {sw.ElapsedMilliseconds}ms");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(FunctionRuntime runtime, int defaultAddress, params object[] args)
    {
        Symbol.Symbol[] localSymbols = _symbols;
        int length = localSymbols.Length;
        MaxStackSize = length;
        for (int address = defaultAddress; address != length; address++)
        {
            localSymbols[address].Execute(runtime, ref address);
        }
    }
}

public class FunctionRuntime
{
    public Stack<object> Stack;
    public List<Task> Task;
    public FunctionObject Function;
    public bool TaskWait;
    public bool TaskWaitAll;

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

public static class ThreadExtensions
{
    public static void WaitTicks(this Thread thread, int ticks)
    {
        Thread.SpinWait(ticks);
    }
}