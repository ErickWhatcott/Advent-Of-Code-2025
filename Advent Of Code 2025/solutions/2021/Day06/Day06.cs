namespace AdventOfCode;

public static partial class Solution2021
{
    [TimePart]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day06(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day06(input), Part2Day06(input));
    }

    [TimePart]
    public static long Part1Day06(string input)
    {
        checked
        {
            const int days = 80;

            long[,] arr = new long[days + 1, 9];
            for (int j = 0; j < 9; j++)
                arr[0, j] = 1;

            for (int i = 1; i <= days; i++)
            {
                arr[i, 0] = arr[i - 1, 6] + arr[i - 1, 8];
                for (int j = 1; j < 9; j++)
                {
                    arr[i, j] = arr[i - 1, j - 1];
                }
            }

            long sum = 0;
            for (int i = 0; i < input.Length; i += 2)
                sum += arr[days, input[i] - '0'];
            return sum;
        }
    }

    [TimePart]
    public static long Part2Day06(string input)
    {
        checked
        {
            const int days = 256;

            long[,] arr = new long[days + 1, 9];
            for (int j = 0; j < 9; j++)
                arr[0, j] = 1;

            for (int i = 1; i <= days; i++)
            {
                arr[i, 0] = arr[i - 1, 6] + arr[i - 1, 8];
                for (int j = 1; j < 9; j++)
                {
                    arr[i, j] = arr[i - 1, j - 1];
                }
            }

            long sum = 0;
            for (int i = 0; i < input.Length; i += 2)
                sum += arr[days, input[i] - '0'];
            return sum;
        }
    }
}