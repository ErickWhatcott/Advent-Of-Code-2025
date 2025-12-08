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
        var lines = input.AsSpan();
        var length = lines.IndexOf('\n');
        var stride = (length + 1) * 2;

        Span<long> curr = stackalloc long[length];
        Span<long> next = stackalloc long[length];

        curr[lines.IndexOf('S')] = 1;
        
        int count = 0;
        
        do
        {
            lines = lines[stride..];

            for (int i = 0; i < length; i++)
            {
                if (curr[i] == 0)
                    continue;

                if (lines[i] == '^')
                {
                    next[i - 1] += curr[i];
                    next[i + 1] = curr[i] + curr[i + 1];

                    curr[i] = 0;
                    curr[i + 1] = 0;

                    count++;
                    i++;
                }
                else
                {
                    next[i] += curr[i];
                    curr[i] = 0;
                }
            }

            var temp = curr;
            curr = next;
            next = temp;
        }while (lines.Length > stride);

        return (count, Day06_Sum(curr));
    }

    public static int Part1Day07(string input)
    {
        var lines = input.AsSpan();
        var length = lines.IndexOf('\n');
        var stride = (length + 1) * 2;

        Span<bool> curr = stackalloc bool[length];
        Span<bool> next = stackalloc bool[length];

        int count = 0;
        curr[lines.IndexOf('S')] = true;
                
        do
        {
            lines = lines[stride..];

            for (int i = 0; i < length; i++)
            {
                if (!curr[i])
                    continue;

                if (lines[i] == '^')
                {
                    next[i - 1] = true;
                    next[i + 1] = true;

                    curr[i] = false;
                    curr[i + 1] = false;

                    count++;
                    i++;
                }
                else
                {
                    next[i] |= curr[i];
                    curr[i] = false;
                }
            }

            var temp = curr;
            curr = next;
            next = temp;
        }while (lines.Length > stride);

        return count;
    }

    public static long Part2Day07(string input)
    {
        var lines = input.AsSpan();
        var length = lines.IndexOf('\n');
        var stride = (length + 1) * 2;

        Span<long> curr = stackalloc long[length];
        Span<long> next = stackalloc long[length];

        curr[lines.IndexOf('S')] = 1;
                
        do
        {
            lines = lines[stride..];

            for (int i = 0; i < length; i++)
            {
                if (curr[i] == 0)
                    continue;

                if (lines[i] == '^')
                {
                    next[i - 1] += curr[i];
                    next[i + 1] = curr[i] + curr[i + 1];

                    curr[i] = 0;
                    curr[i + 1] = 0;

                    i++;
                }
                else
                {
                    next[i] += curr[i];
                    curr[i] = 0;
                }
            }

            var temp = curr;
            curr = next;
            next = temp;
        }while (lines.Length > stride);

        return Day06_Sum(curr);
    }
}