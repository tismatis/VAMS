namespace ConsoleApp1.Symbol;

public abstract class Symbol
{
    public Symbol(string[] args) {}
    public abstract string GetCommand();
    public abstract void Execute(FunctionRuntime runtime);
}