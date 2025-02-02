using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    class Program
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            Parser parser = new Parser();
            stopwatch.Stop();
            Console.WriteLine("Parsing time: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            parser.Parse("program.lasil");
            stopwatch.Stop();
            Console.WriteLine("Compiling time: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            VM vm = parser.GenerateVM();
            stopwatch.Stop();
            Console.WriteLine("Generating VM time: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            vm.Execute("Program", "Main");
            stopwatch.Stop();
            Console.WriteLine("Execution time: " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}