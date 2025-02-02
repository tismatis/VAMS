namespace ConsoleApp1.Symbol;

public class Jump : Symbol
{
    private int _address;
    
    public Jump() : base(null) {}

    public Jump(string[] args) : base(args)
    {
        if (Parser.Loading == ParserState.Loading)
            return;
        
        _address = int.Parse(args[0]);
    }

    public override string GetCommand() => "JUMP";
        
    public override void Execute(FunctionRuntime runtime)
    {
        runtime.Address = _address;
    }
}