# SimpleBenchmark
a easy way to bench your .net code

# For the CPU time profiling

'''
var profiler = new Profiling();
Log.WriteColoredLine(ConsoleColor.Yellow, "First CPU Time Sqrt Test");
profiler.Profile("Call sqrt", counter, () => {
    //Code Here
    result = Math.Sqrt(23);
});
'''


#For the Memory Profiling

It was a bit hard, I don't use GC.GetTotalMemory(b); because I was not happy with the result. 
I prefered (but it was harder:D ) to use Microsoft.Diagnostics.Tracing.TraceEvent package.
I have also to check some points to see if it works perfectly but ...

'''
var memProfiler = new MemoryProfiling();
Log.WriteColoredLine(ConsoleColor.Yellow, "First Memory Sqrt Test");
memProfiler.Profile("Call sqrt", counter, () => {
    //Code Here
    result = Math.Sqrt(23);
});
'''
