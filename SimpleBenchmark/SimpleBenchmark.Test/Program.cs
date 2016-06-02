using System;

namespace SimpleBenchmark.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 100000;

            double result;

            // CPU profiler
            var profiler = new Profiling();
            Log.WriteColoredLine(ConsoleColor.Yellow, "First CPU Time Sqrt Test");
            profiler.Profile("Call sqrt", counter, () => {
                //Code Here
                result = Math.Sqrt(23);
            });


            //Memory Profiler
            var memProfiler = new MemoryProfiling();
            Log.WriteColoredLine(ConsoleColor.Yellow, "First Memory Sqrt Test");
            memProfiler.Profile("Call sqrt", counter, () => {
                //Code Here
                result = Math.Sqrt(23);
            });

            Console.ReadKey();
        }
    }
}
