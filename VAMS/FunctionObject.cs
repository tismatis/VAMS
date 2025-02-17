using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class FunctionObject
    {
        public string Name;
        public ImmutableArray<Symbol.Symbol> Symbols;
        public int MaxStackSize;
        public bool IsAsync;
        public bool ShouldReturn;
        
        public FunctionObject(string name, bool isAsync, bool shouldReturn, List<Symbol.Symbol> symbols)
        {
            Name = name;
            IsAsync = isAsync;
            ShouldReturn = shouldReturn;
            Symbols = symbols.ToImmutableArray();
        }

        public void Setup()
        {
            // if eeded
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Execute(FunctionRuntime runtime, params object[] args)
        {
            runtime.Function = this;
            Stopwatch sw = new Stopwatch();
            int length = Symbols.Length;
            MaxStackSize = length;
            sw.Start();
            for (runtime.Address = 0; runtime.Address != length; runtime.Address++)
                Symbols[runtime.Address].Execute(runtime);
            sw.Stop();
            Console.WriteLine($"Function {Name} executed in {sw.ElapsedMilliseconds}ms");
            
            if(ShouldReturn)
                return runtime.Stack.Pop();
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ExecuteAsync(FunctionRuntime runtime, params object[] args)
        {
            runtime.Function = this;
            Stopwatch sw = new Stopwatch();
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
            
            if(ShouldReturn)
                return runtime.Stack.Pop();
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Execute(FunctionRuntime runtime, int defaultAddress, params object[] args)
        {
            int length = Symbols.Length;
            MaxStackSize = length;
            
            for (runtime.Address = defaultAddress; runtime.Address != length; runtime.Address++)
                Symbols[runtime.Address].Execute(runtime);
            
            if(ShouldReturn)
                return runtime.Stack.Pop();
            return null;
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
        public VM Vm;

        public FunctionRuntime(VM vm)
        {
            Stack = new Stack<object>();
            Task = new List<Task>();
            Vm = vm;
        }

        public FunctionRuntime(FunctionRuntime runtime)
        {
            Stack = new Stack<object>(runtime.Stack);
            Task = new List<Task>();
            Function = runtime.Function;
            Vm = runtime.Vm;
        }
    }
}
