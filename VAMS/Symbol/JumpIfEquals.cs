using System;

namespace ConsoleApp1.Symbol
{
    public class JumpIfEquals : Symbol
    {
        public JumpIfEquals() : base(null) {}

        public JumpIfEquals(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;
        
            var address = int.Parse(args[0]);
            Type type;
            try
            {
                type = Type.GetType(args[1]);
            }
            catch
            {
                throw new InvalidOperationException("JUMP_IF_EQUALS REQUIRES A TYPE");
            }
            
            if(type == typeof(int))
                Execute = runtime => { if((int)runtime.Stack.Peek() == (int)runtime.Stack.Peek(1)) runtime.Address = address; };
            else if(type == typeof(float))
                Execute = runtime => { if((float)runtime.Stack.Peek() == (float)runtime.Stack.Peek(1)) runtime.Address = address; };
            else
                throw new InvalidOperationException("JUMP_IF_EQUALS NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "JUMP_IF_EQUALS";
    }
}