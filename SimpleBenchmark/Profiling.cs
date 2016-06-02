
using System;
using System.Diagnostics;
using System.Reflection;

namespace SimpleBenchmark
{
    public class Profiling
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profiling" /> class.
        /// Done thanks of the excellent article of Matt Warren <see cref="https://mattwarrendotorg.wordpress.com/category/performance/"/>
        /// </summary>
        public Profiling()
        {
            this.CheckProgramState();
        }

        /// <summary>
        /// Profiles the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="action">The action.</param>
        /// <remarks>Taken from <see cref="http://stackoverflow.com/questions/1047218/benchmarking-small-code-samples-in-c-can-this-implementation-be-improved"/></remarks>
        public void Profile(string description, int iterations, Action action)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up
            action();

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                action();
            }
            watch.Stop();
            Console.WriteLine("Profile via an Action - " + description);
            Console.WriteLine("{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/millisecond.\n",
                            watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations, (double)iterations / watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Profiles usnig consume Extension.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="func">The function.</param>
        public void ProfileWithConsume<T>(string description, int iterations, Func<T> func)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up
            func();

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                func().Consume();
            }
            watch.Stop();

            Console.WriteLine("ProfileWithConsume via a Func<T> - " + description);
            Console.WriteLine("{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/millisecond.\n",
                            watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations, (double)iterations / watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Profiles using consume value Extension.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="func">The function.</param>
        public void ProfileWithConsumeValue<T>(string description, int iterations, Func<T> func)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up
            func();

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                func().ConsumeValue();
            }
            watch.Stop();
            Console.WriteLine("ProfileWithConsumeValue via a Func<T> - " + description);
            Console.WriteLine("{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/millisecond.\n",
                            watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations, (double)iterations / watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Profiles using consume no inlining Extension.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="func">The function.</param>
        public void ProfileWithConsumeNoInlining<T>(string description, int iterations, Func<T> func)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up
            func();

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                func().ConsumeNoInlining();
            }
            watch.Stop();
            Console.WriteLine("ProfileWithConsumeNoInlining via a Func<T> - " + description);
            Console.WriteLine("{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/millisecond.\n",
                            watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations, (double)iterations / watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Profiles storing the return value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="func">The function.</param>
        public void ProfileWithStore<T>(string description, int iterations, Func<T> func)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            T result = default(T);

            // warm up
            func();


            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                result = func();
            }
            watch.Stop();
            Console.WriteLine("ProfileWithStore via a Func<T> - " + description);
            Console.WriteLine("{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/millisecond.\n",
                            watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations, (double)iterations / watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Profiles using consume no inlining Extension unrolledx20.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="func">The function.</param>
        public void ProfileWithConsumeNoInliningUnrolledx20<T>(string description, int iterations, Func<T> func)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up
            func();

            var watch = new Stopwatch();
            var loops = iterations / 20;
            watch.Start();
            for (int i = 0; i < loops; i++)
            {
                #region unrolled 20 times
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                func().ConsumeNoInlining();
                #endregion
            }
            watch.Stop();
            Console.WriteLine("ProfileWithConsumeNoInliningUnrolled x20 via a Func<T> - " + description);
            Console.WriteLine("{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/millisecond.\n",
                            watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations, (double)iterations / watch.ElapsedMilliseconds);
        }

        #region Private Methods
        /// <summary>
        /// Checks the state of the program.
        /// </summary>
        private void CheckProgramState()
        {
            // Shamelessy stolen from MeasureIt
            // This equates to the "Optimise" flag on the Project -> Properties -> Build page
            DebuggableAttribute debugAttribute = (DebuggableAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DebuggableAttribute));
            if (debugAttribute != null && debugAttribute.IsJITOptimizerDisabled)
            {
                Log.WriteColoredLine(ConsoleColor.Red, "JIT Optimizer DISABLED\n\tIsJITOptimizerDisabled: {0}, IsJITTrackingEnabled: {1}",
                                        debugAttribute.IsJITOptimizerDisabled, debugAttribute.IsJITTrackingEnabled);
            }
            else if (debugAttribute != null && debugAttribute.IsJITOptimizerDisabled == false)
            {
                Log.WriteColoredLine(ConsoleColor.Green, "JIT Optimizer ENABLED\n\tIsJITOptimizerDisabled: {0}, IsJITTrackingEnabled: {1}",
                                        debugAttribute.IsJITOptimizerDisabled, debugAttribute.IsJITTrackingEnabled);
            }
            else // unknown
            {
                Log.WriteColoredLine(ConsoleColor.White, "JIT Optimizer UNKNOWN");
            }

#if DEBUG
            Log.WriteColoredLine(ConsoleColor.Red, "Running a DEBUG Build", ConsoleColor.Red);
#else
            Log.WriteColoredLine(ConsoleColor.Green, "Running a RELEASE build");
#endif

            Log.WriteColoredLine(ConsoleColor.Green, "Application is {0}-bit", Environment.Is64BitProcess ? "64" : "32");
            Log.WriteColoredLine(ConsoleColor.Green, "Test starts with {0:N0} bytes", Process.GetCurrentProcess().PrivateMemorySize64);

            Console.WriteLine("");
        }
        #endregion
    }
}
