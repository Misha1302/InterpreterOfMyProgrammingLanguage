using System.Diagnostics;

namespace ExecutionTime;

public static class ExecutionTime
{
    public static ulong MethodExecutionTime(Func<int> method)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        method();
        stopwatch.Stop();
        return (ulong)stopwatch.ElapsedMilliseconds;
    }
    
    public static ulong MethodExecutionTime(Action method)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        method();
        stopwatch.Stop();
        return (ulong)stopwatch.ElapsedMilliseconds;
    }
}