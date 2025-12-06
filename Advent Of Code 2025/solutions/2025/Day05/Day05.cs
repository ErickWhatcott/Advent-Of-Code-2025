using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, long) Day05(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day05(input), Part2Day05(input));
    }

    public static int Part1Day05(string input)
    {
        LinkedList<(long start, long end)> ranges = [];

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');

        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            if (line.IsEmpty) break;

            var index = line.IndexOf('-');
            long l1 = long.Parse(line[..index]);
            long l2 = long.Parse(line[(index + 1)..]);

            LinkedListNode<(long, long)>? node = ranges.First;
            while (node != null)
            {
                var next = node.Next;

                var (start, end) = node.Value;
                var start_dif = Math.Max(start, l1);
                var end_dif = Math.Min(end, l2);

                if (start_dif <= end_dif)
                {
                    l1 = Math.Min(start, l1);
                    l2 = Math.Max(end, l2);

                    ranges.Remove(node);
                }

                node = next;
            }

            ranges.AddFirst((l1, l2));
        }

        var arr = ranges.ToArray();
        Array.Sort(arr);

        int fresh = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var curr = long.Parse(line);

            var start = Day05_FindStart(arr, curr);
            if (start) fresh++;
        }

        return fresh;
    }

    public static long Part2Day05(string input)
    {
        LinkedList<(long start, long end)> ranges = [];

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');

        long total = 0;

        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            if (line.IsEmpty) break;

            int index = line.IndexOf('-');


            long l1 = long.Parse(line[..index]);
            long l2 = long.Parse(line[(index + 1)..]);

            // d1 and d2 are to keep track of the coalesced boundary.
            // It is kept separate from l1 and l2 because decrementing the total requires that l1 and l2 are unchanged.
            long d1 = l1;
            long d2 = l2;

            total += l2 - l1 + 1;
            LinkedListNode<(long, long)>? node = ranges.First;
            while (node != null)
            {
                var next = node.Next;

                var (start, end) = node.Value;
                var start_dif = Math.Max(start, l1);
                var end_dif = Math.Min(end, l2);

                if (start_dif <= end_dif)
                {
                    total += start_dif - end_dif - 1;

                    d1 = Math.Min(start, d1);
                    d2 = Math.Max(end, d2);

                    ranges.Remove(node);
                }

                node = next;
            }

            ranges.AddFirst((d1, d2));
        }

        return total;
    }

    // Does a binary search to find the range that contains value.
    private static bool Day05_FindStart((long start, long end)[] arr, long value)
    {
        // Literally just copy-paste of wikipedia:
        // https://en.wikipedia.org/wiki/Binary_search
        int N = arr.Length;

        int M;
        int L = 0;
        int R = N - 1;

        while (L <= R)
        {
            M = L + ((R - L) / 2);

            if (arr[M].end < value)
                L = M + 1;
            else if (arr[M].start > value)
                R = M - 1;
            else
                return true;
        }

        return false;
    }
}