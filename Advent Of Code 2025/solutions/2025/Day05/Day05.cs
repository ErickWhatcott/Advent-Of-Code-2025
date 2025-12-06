using System.Diagnostics;

namespace AdventOfCode;

public static partial class Solution2025
{
    [TimePart]
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, long) Day05(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day05(input), Part2Day05(input));
    }

    [TimePart]
    public static int Part1Day05(string input)
    {
        List<(long start, long end)> ranges = [];

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');

        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            if (line.IsEmpty) break;

            var index = line.IndexOf('-');
            ranges.Add((long.Parse(line[..index]), long.Parse(line[(index + 1)..])));
        }

        int fresh = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var curr = long.Parse(line);

            for (int i = 0; i < ranges.Count; i++)
            {
                var (start, end) = ranges[i];
                if (start <= curr && curr <= end)
                {
                    fresh++;
                    break;
                }
            }
        }

        return fresh;
    }

    [TimePart]
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
}