using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SimpleBenchmark
{
    public class MemoryProfiling
    {

        #region Properties
        private int _processId { get; set; }
        private string _sessionName { get; set; }
        private TraceEventSession _session { get; set; }
        private MemoryStat _stat { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryProfiling"/> class.
        /// </summary>
        public MemoryProfiling()
        {
            _processId = Process.GetCurrentProcess().Id;
            // Sessions can outlive the process that created them.  Thus you need a way of 
            // naming the session so that you can 'reconnect' to it from another process.   This is what the name
            // is for.  It can be anything, but it should be descriptive and unique.   If you expect mulitple versions
            // of your program to run simultaneously, you need to generate unique names (e.g. add a process ID suffix) 
            _sessionName = string.Format("GC-{0}_{1}", Process.GetCurrentProcess().ProcessName, _processId);

            // By default, if you hit Ctrl-C your .NET objects may not be disposed, so force it to.  It is OK if dispose is called twice.
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            {
                _session.Dispose();
            };
        }

        /// <summary>
        /// Profiles the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="action">The action.</param>
        public void Profile(string description, int iterations, Action action)
        {
            // Today you have to be Admin to turn on ETW events (anyone can write ETW events).   
            if (!(TraceEventSession.IsElevated() ?? false))
            {
                Console.WriteLine("To turn on ETW events you need to be Administrator, please run from an Admin process.");
                return;
            }

            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up
            action();

            var watch = new Stopwatch();
            _stat = new MemoryStat();
            using (_session = new TraceEventSession(_sessionName, null))  // the null second parameter means 'real time session'
            {
                // Note that sessions create a OS object (a session) that lives beyond the lifetime of the process
                // that created it (like Filles), thus you have to be more careful about always cleaning them up. 
                // An importanty way you can do this is to set the 'StopOnDispose' property which will cause the session to 
                // stop (and thus the OS object will die) when the TraceEventSession dies.   Because we used a 'using'
                // statement, this means that any exception in the code below will clean up the OS object.   
                _session.StopOnDispose = true;

                // The Parameter matchAnyKeyWord is important to specify what you want to watch.
                _session.EnableProvider(ClrTraceEventParser.ProviderGuid, TraceEventLevel.Verbose, (ulong)(ClrTraceEventParser.Keywords.GC));

                Task.Factory.StartNew(StartProcessingEvents, TaskCreationOptions.LongRunning);
                //Wait for the process Starts
                System.Threading.Thread.Sleep(3000);

                //Run The Code
                watch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    action();
                }
                watch.Stop();
            }

            Console.WriteLine("MemoryProfile via an Action - " + description);
            Console.WriteLine("{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/millisecond.",
                            watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations, (double)iterations / watch.ElapsedMilliseconds);
            Console.WriteLine("Total_Gen0 {0:N0}, Total_Gen0 {1:N0}, Total_Gen0 {2:N0}, {3:N0} bytes_allocated/ops",
                            _stat.GenCounts[0], _stat.GenCounts[1], _stat.GenCounts[2], _stat.AllocatedByOperation);
        }

        private void StartProcessingEvents()
        {
            _session.Source.Clr.GCAllocationTick += gcData =>
            {
                if (_processId == gcData.ProcessID)
                {
                    _stat.TotalOperations++;
                    _stat.AllocatedBytes += gcData.AllocationAmount64;
                }
            };

            _session.Source.Clr.GCStart += gcData =>
            {
                if (_processId == gcData.ProcessID)
                {
                    var genCounts = _stat.GenCounts;
                    if (gcData.Depth >= 0 && gcData.Depth < genCounts.Length)
                    {
                        if (gcData.Reason != GCReason.Induced)
                            genCounts[gcData.Depth]++;
                    }
                    else
                    {
                        //string.Format("Error Process{0}, Unexpected GC Depth: {1}, Count: {2} -> Reason: {3}", gcData.ProcessID, gcData.Depth, gcData.Count, gcData.Reason));
                    }
                }
            };

            _session.Source.Process();
        }

        class MemoryStat
        {
            public long TotalOperations { get; set; }

            public int[] GenCounts = new int[4];
            public long AllocatedBytes { get; set; }

            public long AllocatedByOperation
            {
                get
                {
                    long result = 0;
                    if (TotalOperations != 0)
                        result = (long)AllocatedBytes / TotalOperations;

                    return result;
                }
            }
        }
    }
}
