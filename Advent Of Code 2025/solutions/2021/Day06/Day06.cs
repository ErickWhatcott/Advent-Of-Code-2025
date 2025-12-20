namespace AdventOfCode;

public static partial class Solution2021
{
    // [Completed]
    [TimePart]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day06(string? input = null)
    {
        input ??= ReadFullInput();
        const int days1 = 80;
        const int days2 = 256;
        const int days = 256;

        long[,] arr = new long[days + 1, 9];
        for (int j = 0; j < 9; j++)
            arr[0, j] = 1;

        for (int i = 1; i <= days; i++)
        {
            arr[i, 0] = arr[i - 1, 6] + arr[i - 1, 8];
            arr[i, 1] = arr[i - 1, 0];
            arr[i, 2] = arr[i - 1, 1];
            arr[i, 3] = arr[i - 1, 2];
            arr[i, 4] = arr[i - 1, 3];
            arr[i, 5] = arr[i - 1, 4];
            arr[i, 6] = arr[i - 1, 5];
            arr[i, 7] = arr[i - 1, 6];
            arr[i, 8] = arr[i - 1, 7];
        }

        long p1 = 0, p2 = 0;
        for (int i = 0; i < input.Length; i += 2)
        {
            p1 += arr[days1, input[i] - '0'];
            p2 += arr[days2, input[i] - '0'];
        }
        return (p1, p2);
    }

    public static long Part1Day06(string input)
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

    public static long Part2Day06(string input)
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