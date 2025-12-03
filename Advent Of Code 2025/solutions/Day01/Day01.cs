namespace AdventOfCode2025;

public static partial class Solution
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, int) Day01(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day01(input), Part2Day01(input));
    }

    public static int Part1Day01(string input)
    {
        int count = 0;
        int pos = 50;

        var span = input.AsSpan();
        var enu = span.Split('\n');

        while (enu.MoveNext())
        {
            var curr = span[enu.Current];
            if (curr.Length == 0)
                continue;

            switch (curr[0])
            {
                case 'L':
                    pos -= int.Parse(curr[1..]);
                    break;

                case 'R':
                    pos += int.Parse(curr[1..]);
                    break;

                default:
                    throw new ArgumentException(new string(curr));
            }

            if (pos < 0)
                pos = (100 + (pos % 100)) % 100;
            else if (pos > 99)
                pos %= 100;

            if (pos == 0)
                count++;
        }

        return count;
    }

    public static int Part2Day01(string input)
    {
        // 7101
        int count = 0;
        int pos = 50;

        var span = input.AsSpan();
        var enu = span.Split('\n');

        while (enu.MoveNext())
        {
            var curr = span[enu.Current];
            if (curr.Length == 0)
                continue;

            switch (curr[0])
            {
                case 'L':
                    int delta = int.Parse(curr[1..]);

                    if(pos == 0) pos = 100;
                    if (delta >= pos)
                        count += 1 + ((delta - pos) / 100);
                    pos = (pos - (delta % 100) + 100) % 100;
                    break;

                case 'R':
                    var (div, rem) = Math.DivRem(pos + int.Parse(curr[1..]), 100);
                    count += div;
                    pos = rem;
                    break;

                default:
                    throw new ArgumentException(new string(curr));
            }
        }

        return count;
    }
}