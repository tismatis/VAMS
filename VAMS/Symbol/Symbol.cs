using System;

namespace VAMS.Symbol
{
    public abstract class Symbol
    {
        public Symbol(string[] args) {}
        public abstract string GetCommand();
        public Action<FunctionRuntime> Execute;
    }
}