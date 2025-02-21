using System;
using System.Reflection;
using VAMS.Utilities;

namespace VAMS.Symbol
{
    public class SetPropertyExternal : Symbol
    {
        public SetPropertyExternal() : base(null) {}

        public SetPropertyExternal(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            var type = Tools.SearchType(args[0]);
            if (type == null)
                throw new InvalidOperationException($"Type '{args[0]}' not found.");
            
            var field = type.GetProperty(args[1], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
                throw new InvalidOperationException($"Field '{args[1]}' not found in type '{args[0]}'.");
            
            Execute = runtime => field.SetValue(runtime.Stack.Peek(), runtime.Stack.Peek(1), null);
        }

        public override string GetCommand() => "SET_PROPERTY_EXTERNAL";
    }
}