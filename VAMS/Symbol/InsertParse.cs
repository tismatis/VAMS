using System;

namespace ConsoleApp1.Symbol
{
    
    public class InsertParse : Symbol
    {
        public object value;
    
        public InsertParse() : base(null) {}

        public InsertParse(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            var type = Type.GetType(args[0]);
            if (type == null)
                throw new InvalidOperationException($"Type '{args[0]}' not found.");

            switch (type.FullName)
            {
                case "System.Single":
                    value = float.Parse(args[1]);
                    break;
                case "System.Double":
                    value = double.Parse(args[1]);
                    break;
                case "System.Decimal":
                    value = decimal.Parse(args[1]);
                    break;
                default:
                    throw new NotImplementedException("Type not implemented.");
            }
        }

        public override string GetCommand() => "INSERT_PARSE";
        
        public override void Execute(FunctionRuntime runtime)
        {
            runtime.Stack.Push(value);
        }
    }
}