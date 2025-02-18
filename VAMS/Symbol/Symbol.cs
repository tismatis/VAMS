using System;

namespace ConsoleApp1.Symbol
{
    public abstract class Symbol
    {
        public Symbol(string[] args) {}
        public abstract string GetCommand();
        public Action<FunctionRuntime> Execute;
    }
}