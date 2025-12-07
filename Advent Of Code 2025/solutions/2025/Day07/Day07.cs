using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, long) Day07(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day07(input), Part2Day07(input));
    }

    public static int Part1Day07(string input)
    {
        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        line_enu.MoveNext();

        var line = lines[line_enu.Current];
        Span<bool> curr = stackalloc bool[line.Length];
        Span<bool> next = stackalloc bool[line.Length];

        next.Clear();

        for (int i = 0; i < line.Length; i++)
            curr[i] = line[i] == 'S';

        int count = 0;
        while (line_enu.MoveNext())
        {
            line = lines[line_enu.Current];
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '^' && curr[i])
                {
                    next[i - 1] = true;
                    next[i + 1] = true;

                    count++;
                }
                else
                {
                    next[i] |= curr[i];
                }

                curr[i] = false;
            }

            var temp = curr;
            curr = next;
            next = temp;
        }

        return count;
    }

    public static long Part2Day07(string input)
    {
        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        line_enu.MoveNext();

        var line = lines[line_enu.Current];
        Span<long> curr = stackalloc long[line.Length];
        Span<long> next = stackalloc long[line.Length];

        next.Clear();

        for (int i = 0; i < line.Length; i++)
            curr[i] = line[i] == 'S' ? 1 : 0;

        while (line_enu.MoveNext())
        {
            line = lines[line_enu.Current];
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '^')
                {
                    next[i - 1] += curr[i];
                    next[i + 1] += curr[i];
                }
                else
                {
                    next[i] += curr[i];
                }

                curr[i] = 0;
            }

            var temp = curr;
            curr = next;
            next = temp;
        }

        return Day06_Sum(curr);
    }
}