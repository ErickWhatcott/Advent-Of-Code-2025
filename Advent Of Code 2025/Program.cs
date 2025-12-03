// See https://aka.ms/new-console-template for more information

// Make it so that whether launching from the command line or vscode run button
// the working directory is the same.
using System.ComponentModel;
using System.Reflection;
using AdventOfCode;
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

ConsoleColor color;
int solution_length = "Solution".Length;
foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(a => a.Name.StartsWith("Solution")))
{
    if(type.Name.Length <= solution_length)
        continue;
    string year = type.Name[solution_length..];
    if(!int.TryParse(year, out var year_num))
        continue;
    
    var methods = type
        .GetMethods(BindingFlags.Public | BindingFlags.Static);

    bool first = true;
    foreach (var v in methods.Where(a => a.Name.StartsWith("Day")))
    {
        int parameters = v.GetParameters().Length;
        if (parameters > 1)
            continue;

        if (!int.TryParse(v.Name[3..], out var day))
            continue;


        if (v.GetCustomAttribute<RunAlwaysAttribute>() is RunAlwaysAttribute raa
            || (day == DateTime.Today.Day && year_num == DateTime.Today.Year && v.GetCustomAttribute<CompletedAttribute>() is null))
        {
            if(first)
            {
                first = false;
                color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string('-', 19));
                Console.WriteLine($"-----Year {year}-----");
                Console.WriteLine(new string('-', 19));
                Console.ForegroundColor = color;
            }
            
            string? input = null;
            if (parameters == 1 && v.GetCustomAttribute<DefineInputAttribute>() is DefineInputAttribute ia)
                input = Solution.TryReadInput(ia.File, year, v.Name);

            Console.WriteLine(new string('-', 19));
            Console.WriteLine($"{new string('-', 3)}Running {v.Name}{new string('-', 3)}");
            Console.WriteLine(new string('-', 19));
            var res = v.Invoke(null, input is null ? [] : [input]);
            if (res is not null)
                Console.WriteLine(res);
            Console.WriteLine(new string('-', 19));
        }
    }

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
        if(first)
        {
            first = false;
            color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 19));
            Console.WriteLine($"-----Year {year}-----");
            Console.WriteLine(new string('-', 19));
            Console.ForegroundColor = color;
        }

        string name = v.Name;
        if (name.StartsWith("Part"))
        {
            name = name[5..];
        }

        int parameters = v.GetParameters().Length;

        string? input = null;
        if (parameters == 1 && v.GetCustomAttribute<DefineInputAttribute>() is DefineInputAttribute ia)
            input = Solution.TryReadInput(ia.File, year, name);
        else if (parameters == 1)
            input = Solution.TryReadFullInput(year, name);

        MethodInfoBenchmark.Method = v;
        MethodInfoBenchmark.Instance = null;
        MethodInfoBenchmark.Args = input is null ? [] : [input];

        Console.WriteLine(new string('-', 19));
        Console.WriteLine($"--{v.Name} Benchmark--");
        Console.WriteLine(new string('-', 19));

#if DEBUG
        color = Console.ForegroundColor;
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
            foreach (var error in str)
                Console.WriteLine(error);
            Console.WriteLine(new string('-', 19));
        }
        else
        {
            Console.WriteLine($"-Mean Ns: {report.MeanNanoseconds}-");
            Console.WriteLine($"-Mean Us: {report.MeanMicroseconds}-");
            Console.WriteLine($"-Mean Ms: {report.MeanMilliseconds}-");
            Console.WriteLine(new string('-', 19));
        }
    }
}