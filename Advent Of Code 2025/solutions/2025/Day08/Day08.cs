using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AdventOfCode.Library;

namespace AdventOfCode;

public static partial class Solution2025
{
    [DefineInput(InputType.FullInput)]
    public static (int, long) Day08(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day08(input), Part2Day08(input));
    }

    [TimePart]
    public static int Part1Day08(string input)
    {
        var lines = input.AsSpan();
        var length = lines.Count('\n') + 1;
        int max_iterations = length > 50 ? 1000 : 10;

        Span<int> parents = stackalloc int[length];
        Span<int> sizes = stackalloc int[length];
        UnionFind network = new(parents, sizes);
        Span<(double dist, int i, int j)> edges = new (double dist, int i, int j)[(length * (length - 1) / 2)];
        Span<(long x, long y, long z)> nodes = stackalloc (long, long, long)[length];
        var line_enu = lines.Split('\n');

        int i = 0, k = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var enu = line.Split(',');

            enu.MoveNext();
            var x = long.Parse(line[enu.Current]);

            enu.MoveNext();
            var y = long.Parse(line[enu.Current]);

            enu.MoveNext();
            var z = long.Parse(line[enu.Current]);

            nodes[i] = (x, y, z);

            for (int j = 0; j < i; j++, k++)
            {
                var (qx, qy, qz) = nodes[j];
                var (dx, dy, dz) = (x - qx, y - qy, z - qz);
                var dist = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
                edges[k] = (dist, i, j);
            }

            i++;
        }

        edges.Sort();

        i = 0;
        int iter = 0;
        while (iter < max_iterations)
        {
            var (dist, x, y) = edges[i];

            network.Union(x, y);
            iter++;

            i++;
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

        return max1 * max2 * max3;
    }

    [TimePart]
    public static long Part2Day08(string input)
    {
        var lines = input.AsSpan();
        var length = lines.Count('\n') + 1;

        Span<int> parents = stackalloc int[length];
        Span<int> sizes = stackalloc int[length];
        UnionFind network = new(parents, sizes);
        Span<(double dist, int i, int j)> edges = new (double dist, int i, int j)[(length * (length - 1) / 2)];
        Span<(long x, long y, long z)> nodes = stackalloc (long, long, long)[length];
        var line_enu = lines.Split('\n');

        int i = 0, k = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var enu = line.Split(',');

            enu.MoveNext();
            var x = long.Parse(line[enu.Current]);

            enu.MoveNext();
            var y = long.Parse(line[enu.Current]);

            enu.MoveNext();
            var z = long.Parse(line[enu.Current]);

            nodes[i] = (x, y, z);

            for (int j = 0; j < i; j++, k++)
            {
                var (qx, qy, qz) = nodes[j];
                var (dx, dy, dz) = (x - qx, y - qy, z - qz);
                var dist = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
                edges[k] = (dist, i, j);
            }

            i++;
        }

        edges.Sort();

        i = 0;
        int lastx = -1;
        int lasty = -1;

        int iter = 0;
        while (network.Distinct > 1)
        {
            (var dist, lastx, lasty) = edges[i];

            network.Union(lastx, lasty);
            iter++;

            i++;
        }

        return nodes[lastx].x * nodes[lasty].x;
    }
}