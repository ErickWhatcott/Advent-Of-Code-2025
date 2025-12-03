// See https://aka.ms/new-console-template for more information

// Make it so that whether launching from the command line or vscode run button
// the working directory is the same.
using System.ComponentModel;
using System.Reflection;
using AdventOfCode2025;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

const int depth = 4;
string dir = AppContext.BaseDirectory;
for (int i = 0; i < depth; i++) dir = Path.GetDirectoryName(dir)!;
Directory.SetCurrentDirectory(dir);

var methods = typeof(Solution)
    .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

foreach (var v in methods.Where(a => a.Name.StartsWith("Day")))
{
    int parameters = v.GetParameters().Length;
    if (parameters > 1)
        continue;

    if (!int.TryParse(v.Name[3..], out var day))
        continue;


    if (v.GetCustomAttribute<RunAlwaysAttribute>() is RunAlwaysAttribute raa
        || (day == DateTime.Today.Day && v.GetCustomAttribute<CompletedAttribute>() is null))
    {
        string? input = null;
        if (parameters == 1 && v.GetCustomAttribute<DefineInputAttribute>() is DefineInputAttribute ia)
            input = Solution.ReadInput(ia.File, v.Name);

        Console.WriteLine(new string('-', 19));
        Console.WriteLine($"{new string('-', 3)}Running {v.Name}{new string('-', 3)}");
        Console.WriteLine(new string('-', 19));
        var res = v.Invoke(null, input is null ? [] : [input]);
        if (res is not null)
            Console.WriteLine(res);
        Console.WriteLine(new string('-', 19));
        Console.WriteLine();
    }
}


// var config = ManualConfig.CreateEmpty()
//     .AddJob(Job.Default)
//     .WithOptions(
//         // ConfigOptions.DisableLogFile |
//         ConfigOptions.DisableOptimizationsValidator |
//         ConfigOptions.StopOnFirstError
//     ).AddLogger(ConsoleLogger.Default);

var config = ManualConfig.CreateEmpty()
    .WithOptions(
        ConfigOptions.DisableOptimizationsValidator |
        ConfigOptions.StopOnFirstError |
        ConfigOptions.DisableLogFile
    )
    .AddJob(Job.ShortRun.WithToolchain(InProcessEmitToolchain.Instance))
    .AddLogger(NullLogger.Instance);

foreach (var v in methods.Where(a => a.GetCustomAttribute<TimePartAttribute>() is not null))
{
    string name = v.Name;
    if (name.StartsWith("Part"))
    {
        name = name[5..];
    }

    int parameters = v.GetParameters().Length;

    string? input = null;
    if (parameters == 1 && v.GetCustomAttribute<DefineInputAttribute>() is DefineInputAttribute ia)
        input = Solution.ReadInput(ia.File, name);
    else if(parameters == 1)
        input = Solution.ReadFullInput(name);

    MethodInfoBenchmark.Method = v;
    MethodInfoBenchmark.Instance = null;
    MethodInfoBenchmark.Args = input is null ? [] : [input];

    Console.WriteLine(new string('-', 15));
    Console.WriteLine($"--{v.Name} Benchmark--");
    Console.WriteLine(new string('-', 15));

    #if DEBUG
    var color = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("This is built in DEBUG mode. Rebuild in release for better results.");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("You can use the following comand to run it in release:");
    Console.WriteLine("dotnet run -c release");
    Console.ForegroundColor = color;
    #endif
    

    var summary = BenchmarkRunner.Run<MethodInfoBenchmark>(config);
    var report = new InterpretBenchmark(summary);

    if (report.Errors is string[] str && str.Length > 0)
    {
        Console.WriteLine("Failed with errors:");
        foreach(var error in str)
            Console.WriteLine(error);
        Console.WriteLine(new string('-', 15));
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine($"-Mean Ns: {report.MeanNanoseconds}-");
        Console.WriteLine($"-Mean Us: {report.MeanMicroseconds}-");
        Console.WriteLine($"-Mean Ms: {report.MeanMilliseconds}-");
        Console.WriteLine(new string('-', 15));
        Console.WriteLine();
    }
}