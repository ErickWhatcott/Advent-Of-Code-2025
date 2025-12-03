using System;
using System.Linq;
using BenchmarkDotNet.Reports;

namespace AdventOfCode
{
    public class InterpretBenchmark
    {
        private readonly Summary _summary;

        public InterpretBenchmark(Summary summary)
        {
            _summary = summary ?? throw new ArgumentNullException(nameof(summary));
        }

        public BenchmarkReport[] Reports => _summary.Reports.ToArray();

        /// <summary>
        /// Average mean time (nanoseconds) across all reports that have statistics.
        /// Null if none have meaningful statistics.
        /// </summary>
        public double? MeanNanoseconds
        {
            get
            {
                var stats = Reports.Select(r => r.ResultStatistics).Where(s => s != null).ToArray();
                if (stats.Length == 0) return null;
                return stats.Average(s => s!.Mean);
            }
        }

        public double? MeanMicroseconds => MeanNanoseconds.HasValue ? MeanNanoseconds.Value / 1_000.0 : (double?)null;
        public double? MeanMilliseconds => MeanNanoseconds.HasValue ? MeanNanoseconds.Value / 1_000_000.0 : (double?)null;

        /// <summary>
        /// Allocated bytes per operation for each report (nullable if GC stats are unavailable).
        /// </summary>
        public long?[] BytesAllocatedPerOperation => Reports
            .Select(r => r.GcStats.GetBytesAllocatedPerOperation(r.BenchmarkCase))
            .ToArray();

        /// <summary>
        /// Number of GC collections (Gen0/Gen1/Gen2) per report.
        /// </summary>
        public int[] Gen0Collections => Reports.Select(r => r.GcStats.Gen0Collections).ToArray();
        public int[] Gen1Collections => Reports.Select(r => r.GcStats.Gen1Collections).ToArray();
        public int[] Gen2Collections => Reports.Select(r => r.GcStats.Gen2Collections).ToArray();

        /// <summary>
        /// Raw error messages from ExecuteResults (if any).
        /// </summary>
        public string[] Errors => Reports
            .SelectMany(r => r.ExecuteResults)
            .SelectMany(er => er.Errors ?? Enumerable.Empty<string>())
            .ToArray();

        /// <summary>
        /// Pretty summaries per report.
        /// </summary>
        public string[] ReportSummaries => Reports
            .Select(r =>
            {
                var methodName = r.BenchmarkCase.Descriptor.WorkloadMethod.Name;
                var meanStr = r.ResultStatistics != null
                    ? $"{r.ResultStatistics.Mean:N2} ns"
                    : "N/A";
                var alloc = r.GcStats.GetBytesAllocatedPerOperation(r.BenchmarkCase);
                var allocStr = alloc.HasValue ? $"{alloc.Value} B/op" : "n/a";
                return $"Method: {methodName}, Mean: {meanStr}, Alloc: {allocStr}";
            })
            .ToArray();
    }
}
