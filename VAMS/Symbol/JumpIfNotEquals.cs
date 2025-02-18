using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class JumpIfNotEquals : Symbol
    {
        public JumpIfNotEquals() : base(null) {}

        public JumpIfNotEquals(string[] args) : base(args)
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
                throw new InvalidOperationException("JUMP_IF_NOT_EQUALS REQUIRES A TYPE");
            }
            
            if(type == typeof(int))
                Execute = runtime => { if((int)runtime.Stack.Peek() != (int)runtime.Stack.Peek(1)) runtime.Address = address; };
            else if(type == typeof(float))
                Execute = runtime => { if((float)runtime.Stack.Peek() != (float)runtime.Stack.Peek(1)) runtime.Address = address; };
            else
                throw new InvalidOperationException("JUMP_IF_NOT_EQUALS NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "JUMP_IF_NOT_EQUALS";
    }
}