namespace AdventOfCode2025;

public static partial class Solution
{
    [TimePart]
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, long) Day03(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day03(input), Part2Day03(input));
    }
    
    public static int Part1Day03(string input)
    {
        int sum = 0;

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];

            char max_digit = '0';
            int pos = -1;

            for (int i = 0; i < line.Length - 1; i++)
            {
                if (line[i] > max_digit)
                {
                    max_digit = line[i];
                    pos = i;
                }
            }

            int res = (max_digit - '0') * 10;
            max_digit = '0';
            for (int i = pos + 1; i < line.Length; i++)
            {
                if (line[i] > max_digit)
                {
                    max_digit = line[i];
                }
            }

            sum += res + (max_digit - '0');
        }

        return sum;
    }

    public static long Part2Day03(string input)
    {
        checked{
        long sum = 0;

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];

            long res = 0;
            for(int i = 11; i >= 0; i--)
            {
                int pos = -1;
                char max_digit = '0';
                
                for (int j = 0; j < line.Length - i; j++)
                {
                    if (line[j] > max_digit)
                    {
                        max_digit = line[j];
                        pos = j;
                    }
                }

                res *= 10;
                res += max_digit - '0';
                line = line[(pos+1)..];
            }
            
            sum += res;
        }

        return sum;
        }
    }
}