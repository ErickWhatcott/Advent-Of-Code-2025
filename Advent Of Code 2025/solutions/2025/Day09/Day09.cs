namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day09(string? input)
    {
        input ??= ReadFullInput();
        return (Part1Day09(input), Part2Day09(input));
    }

    public static long Part1Day09(string input)
    {
        var lines = input.AsSpan();
        var length = lines.Count('\n') + 1;
        Span<(int x, int y)> nodes = stackalloc (int, int)[length];
        var line_enu = lines.Split('\n');

        int i = 0;
        long max_area = 0;
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var index = line.IndexOf(',');
            int x = int.Parse(line[..index]);
            int y = int.Parse(line[(index + 1)..]);
            nodes[i] = (x, y);

            for (int j = 0; j < i; j++)
            {
                var (px, py) = nodes[j];
                max_area = Math.Max(max_area, (Math.Abs((long)px - x) + 1) * (Math.Abs((long)py - y) + 1));
            }

            i++;
        }

        return max_area;
    }

    public static unsafe long Part2Day09(string input)
    {
            var lines = input.AsSpan();
            var length = lines.Count('\n') + 1;
            Span<(int x, int y)> nodes = stackalloc (int, int)[length];
            Dictionary<int, int> x_map = new(length);
            Dictionary<int, int> y_map = new(length);
            SortedSet<int> x_queue = [];
            SortedSet<int> y_queue = [];

            var line_enu = lines.Split('\n');

            for (int i = 0; line_enu.MoveNext(); i++)
            {
                var line = lines[line_enu.Current];
                var index = line.IndexOf(',');
                int x = int.Parse(line[..index]);
                int y = int.Parse(line[(index + 1)..]);
                nodes[i] = (x, y);
                x_queue.Add(x);
                y_queue.Add(y);
            }

            int x_pos = 0;
            int y_pos = 0;
            foreach (var v in x_queue)
                x_map.Add(v, x_pos++);
            foreach (var v in y_queue)
                y_map.Add(v, y_pos++);

            int[,] map = new int[x_pos, y_pos];
            for (int i = 0; i < length; i++)
            {
                var p1 = nodes[i];
                var (x1, y1) = (x_map[p1.x], y_map[p1.y]);
                var p2 = nodes[(i + 1) % length];
                var (x2, y2) = (x_map[p2.x], y_map[p2.y]);

                if (x1 > x2)
                    (x1, x2) = (x2, x1);
                if (y1 > y2)
                    (y1, y2) = (y2, y1);


                for (int j = x1; j <= x2; j++)
                    for (int k = y1; k <= y2; k++)
                        if (map[j, k] == 0)
                            map[j, k] = 1;

                map[x1, y1] = 2;
                map[x2, y2] = 2;
            }

            // Find a side of the polygon, and use this to find an internal position to flood fill.
            int pos_x = int.MaxValue;
            int pos_y = int.MaxValue;
            for (int i = 0; i < x_pos; i++)
            {
                fixed (int* curr = &map[i, 0])
                {
                    Span<int> span = new(curr, y_pos);
                    int index = span.IndexOf(1);
                    if (index != -1 && index < pos_y)
                    {
                        pos_y = index;
                        pos_x = i;
                    }
                }
            }

            // Flood fill the inside
            Stack<(int, int)> stack = [];
            stack.Push((pos_x, pos_y + 1));
            while(stack.Count > 0)
            {
                var (x, y) = stack.Pop();
                if(map[x, y] != 0)
                    continue;

                map[x, y] = 1;
                stack.Push((x, y + 1));
                stack.Push((x, y - 1));
                stack.Push((x + 1, y));
                stack.Push((x - 1, y));
            }

            long max_area = 0;
            for (int i1 = 0; i1 < length; i1++)
            {
                for (int i2 = i1 + 1; i2 < length; i2++)
                {
                    var p1 = nodes[i1];
                    var (x1, y1) = (x_map[p1.x], y_map[p1.y]);
                    var p2 = nodes[i2];
                    var (x2, y2) = (x_map[p2.x], y_map[p2.y]);

                    if (Day09_Satisfactory(map, x1, y1, x2, y2))
                    {
                        max_area = Math.Max(max_area, (Math.Abs((long)p1.x - p2.x) + 1) * (Math.Abs((long)p1.y - p2.y) + 1));
                    }
                }
            }

            return max_area;
    }

    private static bool Day09_Satisfactory(int[,] map, int x1, int y1, int x2, int y2)
    {
        if (x1 > x2)
            (x1, x2) = (x2, x1);
        if (y1 > y2)
            (y1, y2) = (y2, y1);

        for (int j = x1; j <= x2; j++)
        {
            for (int k = y1; k <= y2; k++)
            {
                if (map[j, k] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }
}