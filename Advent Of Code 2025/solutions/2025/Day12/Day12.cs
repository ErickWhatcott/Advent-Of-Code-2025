namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day12(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day12(input), Part2Day12(input));
    }

    public static long Part1Day12(string input)
    {
        Span<int> counts = stackalloc int[6];

        var lines = input.AsSpan(3);
        var enu = lines.Split(':');

        for(int i = 0; i < 6; i++)
        {
            enu.MoveNext();
            counts[i] = lines[enu.Current].Count('#');
        }
        
        int total = 0;
        var bounds = lines[enu.Current][^5..].Trim();
        while(enu.MoveNext())
        {
            var numbers = lines[enu.Current][1..];
            int end = numbers.LastIndexOf('\n');
            if(end != -1)
                numbers = numbers[..end];

            var index = bounds.IndexOf('x');
            int w = int.Parse(bounds[..index]);
            int h = int.Parse(bounds[(index+1)..]);

            int area = w * h;
            var space_enu = numbers.Split(' ');
            space_enu.MoveNext();

            int actual = counts[0] * int.Parse(numbers[space_enu.Current]);

            space_enu.MoveNext();
            actual += counts[1] * int.Parse(numbers[space_enu.Current]);

            space_enu.MoveNext();
            actual += counts[2] * int.Parse(numbers[space_enu.Current]);

            space_enu.MoveNext();
            actual += counts[3] * int.Parse(numbers[space_enu.Current]);

            space_enu.MoveNext();
            actual += counts[4] * int.Parse(numbers[space_enu.Current]);

            space_enu.MoveNext();
            actual += counts[5] * int.Parse(numbers[space_enu.Current]);

            if(actual < area)
                total++;

            bounds = lines[enu.Current][^5..].Trim();
        }

        return total;
    }

    public static long Part2Day12(string input)
    {
        return -1;
    }
}