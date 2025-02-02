using System.Runtime.CompilerServices;

namespace ConsoleApp1;

public class ClassDescriptor
{
    public string Name;
    private List<FunctionObject> _functions = new();
    
    public ClassDescriptor(string name)
    {
        Name = name;
    }
    
    public void AddFunction(FunctionObject function)
    {
        _functions.Add(function);
    }
    
    public void Setup()
    {
        foreach (FunctionObject f in _functions)
            f.Setup();
    }
    
    public void Execute(FunctionRuntime runtime, string function, params object[] args)
    {
        foreach (FunctionObject f in _functions)
        {
            if (f.Name == function)
            {
                if(f.IsAsync)
                    f.ExecuteAsync(runtime, args);
                else
                    f.Execute(runtime, args);
                return;
            }
        }
        
        throw new Exception($"Function {function} not found in class {Name}.");
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(string function, params object[] args)
    {
        Execute(new FunctionRuntime(), function, args);
    }
}

public class RuntimeException : Exception
{
    public RuntimeException(string message, int address) : base($"An error occured in the VM during runtime at address {address}: {message}") {}
}