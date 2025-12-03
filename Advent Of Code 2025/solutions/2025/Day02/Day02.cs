namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day02(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day02(input), Part2Day02(input));
    }

    public static long Part1Day02(string input)
    {

        List<(long min, long max)> values = [];
        foreach (var id_combo in input.Split(','))
        {
            var split = id_combo.Split('-', 2);
            values.Add((long.Parse(split[0]), long.Parse(split[1])));
        }

        long count = 0;
        for (long i = 1; ; i++)
        {
            long log10 = (long)Math.Log10(i);
            long factor = (long)Math.Pow(10, log10 + 1) + 1L;
            long value = factor * i;

            bool any = false;
            foreach (var (min, max) in values)
            {
                if (value <= max)
                {
                    any = true;
                    if (value >= min)
                    {
                        count += value;
                    }
                }
            }

            if (!any)
                break;
        }

        return count;
    }

    public static long Part2Day02(string input)
    {
        HashSet<long> check = [];

        List<(long min, long max)> values = [];
        foreach (var id_combo in input.Split(','))
        {
            var split = id_combo.Split('-', 2);
            values.Add((long.Parse(split[0]), long.Parse(split[1])));
        }

        long count = 0;
        for (long i = 1; ; i++)
        {
            long log10 = (long)Math.Log10(i);
            long factor = (long)Math.Pow(10, log10 + 1) + 1L;

            bool all_any = false;
            while (true)
            {
                long value = factor * i;

                bool added = check.Add(value);

                bool any = false;
                foreach (var (min, max) in values)
                {
                    if (value <= max)
                    {
                        all_any = any = true;
                        if (!added)
                            break;

                        if (value >= min)
                        {
                            count += value;
                        }
                    }
                }

                if (!any)
                    break;

                factor = (long)(factor * Math.Pow(10, log10 + 1)) + 1L;
            }

            if (!all_any)
                break;
        }

        return count;
    }
}