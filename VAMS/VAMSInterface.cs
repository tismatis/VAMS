using System;

namespace ConsoleApp1
{
    public static class VAMSInterface
    {
        public static event Action<object> ConsoleOutput;
        public static void OnConsoleOutput(object obj)
        {
            ConsoleOutput?.Invoke(obj);
        }
        
        public static void AutoSetupNet()
        {
            ConsoleOutput += Console.WriteLine;
        }
    }
}