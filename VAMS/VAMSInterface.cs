using System;

namespace VAMS
{
    public static class VAMSInterface
    {
        public static event Action<object> ConsoleOutput;
        public static void OnConsoleOutput(object obj)
        {
            ConsoleOutput?.Invoke(obj);
        }
    }
}