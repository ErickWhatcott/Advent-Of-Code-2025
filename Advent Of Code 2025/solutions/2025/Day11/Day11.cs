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
        const int max_values = 1028;
        const long mask = 0x0000FFFFFFFFFFFF;
        const long you_bits = 502518448249;
        const long out_bits = 498223874159;

        Dictionary<long, (int offset, int length)> Info = new(max_values);
        Dictionary<long, int> incoming = new(max_values);
        Dictionary<long, int> final_values = new(max_values);
        Span<long> dependencies = stackalloc long[max_dependencies];
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


        var queue = new Queue<long>();
        foreach (var key in Info.Keys)
        {
            if (!incoming.ContainsKey(key))
            {
                queue.Enqueue(key);
            }
        }

        int you_pos = 0;
        current_offset = 0;
        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            if (n == you_bits) you_pos = current_offset;
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
        for (int i = you_pos; i < current_offset; i++)
        {
            var curr = sorted[i];
            if (final_values.TryGetValue(curr, out var curr_val))
            {
                var (offset, length) = Info.GetValueOrDefault(curr);
                for (int j = 0; j < length; j++)
                {
                    var dep = dependencies[offset + j];
                    final_values[dep] = final_values.GetValueOrDefault(dep) + curr_val;
                }
            }
        }

        return final_values[out_bits];
    }

    public static unsafe long Part2Day11(string input)
    {
        const int max_dependencies = 4096;
        const int max_values = 1024;
        const long mask = 0x0000FFFFFFFFFFFF;
        const long svr_bits = 489634005107;
        const long dac_bits = 425208119396;
        const long fft_bits = 498222891110;
        const long out_bits = 498223874159;

        Dictionary<long, (int offset, int length)> info = new(max_values);
        Dictionary<long, int> incoming = new(max_values);
        Dictionary<long, (long neither, long dac, long fft, long both)> final_values = new(max_values);
        Span<long> dependencies = stackalloc long[max_dependencies];
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

                info[value] = (current_offset, length);

                for (int i = 0; i < length; i++, current_offset++)
                {
                    ptr++;
                    var current_value = (*ptr) >> 16;
                    dependencies[current_offset] = current_value;
                    incoming[current_value] = incoming.GetValueOrDefault(current_value) + 1;
                }
            }
        }


        var queue = new Queue<long>();
        foreach (var key in info.Keys)
        {
            if (!incoming.ContainsKey(key))
            {
                queue.Enqueue(key);
            }
        }

        int svr_pos = 0;
        current_offset = 0;
        while (queue.Count > 0)
        {
            var n = queue.Dequeue();
            if (n == svr_bits) svr_pos = current_offset;
            sorted[current_offset] = n;
            var (offset, length) = info.GetValueOrDefault(n);
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
        for (int i = svr_pos; i < current_offset; i++)
        {
            var curr = sorted[i];
            if (final_values.TryGetValue(curr, out var curr_val))
            {
                var (curr1, curr2, curr3, curr4) = curr_val;
                var (offset, length) = info.GetValueOrDefault(curr);
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
        }

        return final_values[out_bits].both;
    }
}