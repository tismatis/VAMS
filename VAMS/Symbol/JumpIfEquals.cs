using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol;

public class JumpIfEquals : Symbol
{
    private int _address;

    public JumpIfEquals() : base(null) {}

    public JumpIfEquals(string[] args) : base(args)
    {
        if (Parser.Loading == ParserState.Loading)
            return;
        
        _address = int.Parse(args[0]);
    }

    public override string GetCommand() => "JUMP_IF_EQUALS";
        
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void Execute(FunctionRuntime runtime)
    {
        if(runtime.Stack.Peek() is int a && runtime.Stack.Peek(1) is int b)
        {
            if(a == b)
                runtime.Address = _address;
            return;
        }
        
        throw new Exception("Cannot compare non-integer values.");
    }
}