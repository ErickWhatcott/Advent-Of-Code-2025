```

BenchmarkDotNet v0.15.8, macOS Tahoe 26.1 (25B78) [Darwin 25.1.0]
Apple M3 Pro, 1 CPU, 11 logical and 11 physical cores
.NET SDK 9.0.101
  [Host] : .NET 9.0.0 (9.0.0, 9.0.24.52809), Arm64 RyuJIT armv8.0-a

Job=ShortRun  Toolchain=InProcessEmitToolchain  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
| Method | Mean     | Error    | StdDev   |
|------- |---------:|---------:|---------:|
| Run    | 58.03 μs | 17.50 μs | 0.959 μs |
