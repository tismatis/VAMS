using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Symbol
{
    public class Add : Symbol
    {
        public Add() : base(null) {}

        public Add(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            if (args.Length != 1)
                throw new InvalidOperationException("ADD REQUIRES EXACTLY 1 ARGUMENT");

            Type type;
            try
            {
                type = Type.GetType(args[0]);
            }
            catch
            {
                throw new InvalidOperationException("ADD REQUIRES A TYPE");
            }
            
            if(type == typeof(int))
                Execute = runtime => runtime.Stack.Push((int)runtime.Stack.Pop() + (int)runtime.Stack.Pop());
            else if(type == typeof(float))
                Execute = runtime => runtime.Stack.Push((float)runtime.Stack.Pop() + (float)runtime.Stack.Pop());
            else if(type == typeof(string))
                Execute = runtime => runtime.Stack.Push((string)runtime.Stack.Pop() + (string)runtime.Stack.Pop());
            else
                throw new InvalidOperationException("ADD NOT IMPLEMENTED FOR THIS");
        }

        public override string GetCommand() => "ADD";
    }
}