using System.Diagnostics;
using AdventOfCode.Library;

namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, long) Day08(string? input = null)
    {
        input ??= ReadFullInput();
        var lines = input.AsSpan();
        var length = lines.Count('\n') + 1;
        int max_iterations = length > 50 ? 1000 : 10;

        Span<int> parents = stackalloc int[length];
        Span<int> sizes = stackalloc int[length];
        UnionFind network = new(parents, sizes);
        PriorityQueue<(int i, int j), long> queue = new(length * (length - 1) / 2);
        Span<(int x, int y, int z)> nodes = stackalloc (int, int, int)[length];
        var line_enu = lines.Split('\n');

        int i = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var enu = line.Split(',');

            enu.MoveNext();
            var x = int.Parse(line[enu.Current]);

            enu.MoveNext();
            var y = int.Parse(line[enu.Current]);

            enu.MoveNext();
            var z = int.Parse(line[enu.Current]);

            nodes[i] = (x, y, z);

            for (int j = 0; j < i; j++)
            {
                var (qx, qy, qz) = nodes[j];
                var (dx, dy, dz) = ((long)x - qx, (long)y - qy, (long)z - qz);
                var dist = (dx * dx) + (dy * dy) + (dz * dz);
                queue.Enqueue((i, j), dist);
            }

            i++;
        }

        i = 0;
        (int x1, int x2) value = default;

        int iter = 0;

        while (iter < max_iterations)
        {
            value = queue.Dequeue();

            network.Union(value.x1, value.x2);
            iter++;
        }

        int max1 = 0;
        int max2 = 0;
        int max3 = 0;

        for (i = 0; i < length; i++)
        {
            if (parents[i] != i)
                continue;

            int size = sizes[i];
            if (size > max1)
            {
                max3 = max2;
                max2 = max1;
                max1 = size;
            }
            else if (size > max2)
            {
                max3 = max2;
                max2 = size;
            }
            else if (size > max3)
            {
                max3 = size;
            }
        }

        int p1 = max1 * max2 * max3;

        while (network.Distinct > 1)
        {
            value = queue.Dequeue();
            network.Union(value.x1, value.x2);
        }

        Debug.Assert(p1 == Part1Day08(input));
        Debug.Assert(((long)nodes[value.x1].x * nodes[value.x2].x) == Part2Day08(input));

        return (p1, (long)nodes[value.x1].x * nodes[value.x2].x);
    }

    public static int Part1Day08(string input)
    {
        var lines = input.AsSpan();
        var length = lines.Count('\n') + 1;
        int max_iterations = length > 50 ? 1000 : 10;

        Span<int> parents = stackalloc int[length];
        Span<int> sizes = stackalloc int[length];
        UnionFind network = new(parents, sizes);
        PriorityQueue<(int i, int j), long> queue = new(max_iterations);
        Span<(int x, int y, int z)> nodes = stackalloc (int, int, int)[length];
        var line_enu = lines.Split('\n');

        int i = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var enu = line.Split(',');

            enu.MoveNext();
            var x = int.Parse(line[enu.Current]);

            enu.MoveNext();
            var y = int.Parse(line[enu.Current]);

            enu.MoveNext();
            var z = int.Parse(line[enu.Current]);

            nodes[i] = (x, y, z);

            for (int j = 0; j < i; j++)
            {
                var (qx, qy, qz) = nodes[j];
                var (dx, dy, dz) = ((long)x - qx, (long)y - qy, (long)z - qz);
                var dist = -((dx * dx) + (dy * dy) + (dz * dz));

                if (queue.Count < max_iterations)
                {
                    queue.Enqueue((i, j), -dist);
                }
                else if (queue.TryPeek(out _, out var other_dist) && -dist > other_dist)
                {
                    queue.DequeueEnqueue((i, j), -dist);
                }
            }

            i++;
        }

        while (queue.TryDequeue(out var value, out _))
            network.Union(value.i, value.j);

        int max1 = 0;
        int max2 = 0;
        int max3 = 0;

        for (i = 0; i < length; i++)
        {
            if (parents[i] != i)
                continue;

            int size = sizes[i];
            if (size > max1)
            {
                max3 = max2;
                max2 = max1;
                max1 = size;
            }
            else if (size > max2)
            {
                max3 = max2;
                max2 = size;
            }
            else if (size > max3)
            {
                max3 = size;
            }
        }

        return max1 * max2 * max3;
    }

    public static long Part2Day08(string input)
    {
        var lines = input.AsSpan();
        var length = lines.Count('\n') + 1;

        Span<int> parents = stackalloc int[length];
        Span<int> sizes = stackalloc int[length];
        UnionFind network = new(parents, sizes);
        PriorityQueue<(int i, int j), long> queue = new(length * (length - 1) / 2);
        Span<(int x, int y, int z)> nodes = stackalloc (int, int, int)[length];
        var line_enu = lines.Split('\n');

        int i = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var enu = line.Split(',');

            enu.MoveNext();
            var x = int.Parse(line[enu.Current]);

            enu.MoveNext();
            var y = int.Parse(line[enu.Current]);

            enu.MoveNext();
            var z = int.Parse(line[enu.Current]);

            nodes[i] = (x, y, z);

            for (int j = 0; j < i; j++)
            {
                var (qx, qy, qz) = nodes[j];
                var (dx, dy, dz) = ((long)x - qx, (long)y - qy, (long)z - qz);
                var dist = (dx * dx) + (dy * dy) + (dz * dz);
                queue.Enqueue((i, j), dist);
            }

            i++;
        }

        i = 0;
        (int x, int y) value = default;

        while (network.Distinct > 1)
        {
            value = queue.Dequeue();

            network.Union(value.x, value.y);
        }

        return (long)nodes[value.x].x * nodes[value.y].x;
    }
}