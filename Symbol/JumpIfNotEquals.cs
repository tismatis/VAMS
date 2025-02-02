using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol;

public class JumpIfNotEquals : Symbol
{
    private int _address;

    public JumpIfNotEquals() : base(null) {}

    public JumpIfNotEquals(string[] args) : base(args)
    {
        if (Parser.Loading == ParserState.Loading)
            return;
        
        _address = int.Parse(args[0]);
    }

    public override string GetCommand() => "JUMP_IF_NOT_EQUALS";
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Execute(FunctionRuntime runtime, ref int address)
    {
        if(runtime.Stack.Peek() is int a && runtime.Stack.Peek(1) is int b)
        {
            if (a != b)
            {
                address = _address;
                return;
            }
        }
        else
            throw new Exception("Cannot compare non-integer values.");
    }
}