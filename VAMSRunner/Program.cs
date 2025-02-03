using System.Diagnostics;
using ConsoleApp1;

namespace VAMSRunner;

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
            
        stopwatch.Start();
            
        VM vm;
            
        using (Parser parser = new Parser())
        {
            stopwatch.Stop();
            Console.WriteLine("Parsing time: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            parser.Parse("program.lasil");
            stopwatch.Stop();
            Console.WriteLine("Compiling time: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();
            
            stopwatch.Start();
            vm = parser.GenerateVM();
            stopwatch.Stop();
            Console.WriteLine("Generating VM time: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();
        }
            
            
        stopwatch.Start();
        vm.Execute("Program", "Main");
        stopwatch.Stop();
        Console.WriteLine("Execution time: " + stopwatch.ElapsedMilliseconds + "ms");
    }
}