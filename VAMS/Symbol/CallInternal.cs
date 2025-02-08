using System;

namespace ConsoleApp1.Symbol
{
    public class CallInternal : Symbol
    {
        public string Path;
        public string Name;
        public bool NeedPush;

        public CallInternal() : base(null) {}

        public CallInternal(string[] args) : base(args)
        {
            if (Parser.Loading == ParserState.Loading)
                return;

            Path = args[0];
            Name = args[1];
            NeedPush = bool.Parse(args[2]);
        }

        public override string GetCommand() => "CALL_INTERNAL";
        
        public override void Execute(FunctionRuntime runtime)
        {
            VAMSInterface.OnConsoleOutput("Executing internal function " + Path + "." + Name);

            if(NeedPush)
                runtime.Stack.Push(runtime.Vm.Execute(Path, Name));
            else
                runtime.Vm.Execute(Path, Name);
        }
    }
}