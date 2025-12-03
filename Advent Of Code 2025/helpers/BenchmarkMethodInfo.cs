using BenchmarkDotNet.Attributes;
using System.Reflection;

public class MethodInfoBenchmark
{
    public static MethodInfo? Method;
    public static object? Instance;
    public static object[]? Args;

    public MethodInfoBenchmark() { }

    [Benchmark]
    public object? Run()
    {
        ArgumentNullException.ThrowIfNull(Method);
        return Method.Invoke(Instance, Args);
    }
}
