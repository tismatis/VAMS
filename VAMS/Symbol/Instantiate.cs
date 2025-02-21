using System;
using System.Collections.Generic;
using System.Linq;

namespace VAMS.Symbol
{
    public class Instantiate : Symbol
    {
        public Instantiate() : base(null) {}

        public Instantiate(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
            
            var type = Utilities.Tools.SearchType(args[0]);
            if (type == null)
                throw new InvalidOperationException($"Type '{args[0]}' not found.");
            
            Execute = runtime => runtime.Stack.Push(Activator.CreateInstance(type));
        }

        public override string GetCommand() => "INSTANTIATE";
    }
}