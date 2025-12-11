using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using AdventOfCode.Library;
using Microsoft.Diagnostics.Tracing.StackSources;
using Microsoft.Z3;

namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day11(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day11(input), Part2Day11(input));
    }

    public static unsafe long Part1Day11(string input)
    {
        const int max_dependencies = 4096;
        const int max_values = 2048;
        const long mask = 0x0000FFFFFFFFFFFF;
        const long you_bits = 502518448249;
        const long out_bits = 498223874159;

        Dictionary<long, (int offset, int length)> Info = [];
        Dictionary<long, int> incoming = [];
        Dictionary<long, int> final_values = [];
        Span<long> dependencies = stackalloc long[max_values + max_dependencies];
        Span<long> sorted = stackalloc long[max_values];

        int current_offset = 0;

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            fixed (char* str = line)
            {
                long* ptr = (long*)str;
                long value = (*ptr) & mask;
                int length = (line.Length / 4) - 1;

                Info[value] = (current_offset, length);

                for (int i = 0; i < length; i++, current_offset++)
                {
                    ptr++;
                    var current_value = (*ptr) >> 16;
                    dependencies[current_offset] = current_value;
                    incoming[current_value] = incoming.GetValueOrDefault(current_value) + 1;
                }
            }
        }

        current_offset = 0;
        var queue = new Queue<long>();
        foreach (var key in Info.Keys)
        {
            if (!incoming.ContainsKey(key))
            {
                final_values[key] = 0;
                queue.Enqueue(key);
            }
        }

        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            sorted[current_offset] = n;
            var (offset, length) = Info.GetValueOrDefault(n);
            for (int i = 0; i < length; i++)
            {
                var m = dependencies[offset + i];
                var new_incoming = --incoming[m];
                if (new_incoming == 0)
                    queue.Enqueue(m);
            }

            current_offset++;
        }

        final_values[you_bits] = 1;
        for (int i = 0; i < current_offset; i++)
        {
            var curr = sorted[i];
            var (offset, length) = Info.GetValueOrDefault(curr);
            for (int j = 0; j < length; j++)
            {
                var dep = dependencies[offset + j];
                final_values[dep] = final_values.GetValueOrDefault(dep) + final_values[curr];
            }
        }

        return final_values[out_bits];
    }

    public static unsafe long Part2Day11(string input)
    {
        const int max_dependencies = 4096;
        const int max_values = 2048;
        const long mask = 0x0000FFFFFFFFFFFF;
        const long svr_bits = 489634005107;
        const long dac_bits = 425208119396;
        const long fft_bits = 498222891110;
        const long out_bits = 498223874159;

        Dictionary<long, (int offset, int length)> Info = [];
        Dictionary<long, int> incoming = [];
        Dictionary<long, (long neither, long dac, long fft, long both)> final_values = [];
        Span<long> dependencies = stackalloc long[max_values + max_dependencies];
        Span<long> sorted = stackalloc long[max_values];

        int current_offset = 0;

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            fixed (char* str = line)
            {
                long* ptr = (long*)str;
                long value = (*ptr) & mask;
                int length = (line.Length / 4) - 1;

                Info[value] = (current_offset, length);

                for (int i = 0; i < length; i++, current_offset++)
                {
                    ptr++;
                    var current_value = (*ptr) >> 16;
                    dependencies[current_offset] = current_value;
                    incoming[current_value] = incoming.GetValueOrDefault(current_value) + 1;
                }
            }
        }

        current_offset = 0;
        var queue = new Queue<long>();
        foreach (var key in Info.Keys)
        {
            if (!incoming.ContainsKey(key))
            {
                final_values[key] = default;
                queue.Enqueue(key);
            }
        }

        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            sorted[current_offset] = n;
            var (offset, length) = Info.GetValueOrDefault(n);
            for (int i = 0; i < length; i++)
            {
                var m = dependencies[offset + i];
                var new_incoming = --incoming[m];
                if (new_incoming == 0)
                    queue.Enqueue(m);
            }

            current_offset++;
        }

        final_values[svr_bits] = (1, 0, 0, 0);
        for (int i = 0; i < current_offset; i++)
        {
            var curr = sorted[i];
            var (curr1, curr2, curr3, curr4) = final_values[curr];
            var (offset, length) = Info.GetValueOrDefault(curr);
            for (int j = 0; j < length; j++)
            {
                var dep = dependencies[offset + j];
                var (dep1, dep2, dep3, dep4) = final_values.GetValueOrDefault(dep);

                if (dep is fft_bits)
                {
                    final_values[dep] = (
                        dep1,
                        dep2,
                        curr1 + curr2 + dep3,
                        curr3 + curr4 + dep4
                    );
                }
                else if (dep is dac_bits)
                {
                    final_values[dep] = (
                        dep1, 
                        curr2 + curr1 + dep2,
                        dep3, 
                        curr3 + curr4 + dep4
                    );
                }
                else
                {
                    final_values[dep] = (
                        curr1 + dep1, 
                        curr2 + dep2, 
                        curr3 + dep3, 
                        curr4 + dep4
                    );
                }
            }
        }

        return final_values[out_bits].both;
    }
}