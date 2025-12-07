using System.Diagnostics;

namespace AdventOfCode;

public static partial class Solution2025
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
        var lines = input.AsSpan();
        var index = lines.LastIndexOf('\n');
        var operands = lines[(index + 1)..];
        lines = lines[..index];

        int i = operands.Length - operands.Count(' ');

        Span<bool> should_multiply = stackalloc bool[i];
        Span<long> results = stackalloc long[i];

        i = 0;
        var enu = operands.Split(' ');
        while (enu.MoveNext())
        {
            if ((enu.Current.End.Value - enu.Current.Start.Value) != 0)
            {
                results[i] = (should_multiply[i] = operands[enu.Current.Start] == '*') ? 1 : 0;
                i++;
            }
        }

        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            enu = line.Split(' ');
            i = 0;

            while (enu.MoveNext())
            {
                if ((enu.Current.End.Value - enu.Current.Start.Value) != 0)
                {
                    int curr = int.Parse(line[enu.Current]);
                    results[i] = should_multiply[i] ? (results[i] * curr) : (results[i] + curr);
                    i++;
                }
            }
        }

        long sum = 0;
        for (i = 0; i < results.Length; i++)
            sum += results[i];

        return sum;
    }

    [TimePart]
    public static unsafe long Part2Day06(string input)
    {
        var lines = input.AsSpan();
        var index = lines.LastIndexOf('\n');
        var operands = lines[(index + 1)..];
        lines = lines[..index];

        int i = operands.Length - operands.Count(' ');

        Span<bool> should_multiply = stackalloc bool[i];
        Span<(int offset, int value1, int value2, int value3, int value4)> results = stackalloc (int, int, int, int, int)[i];

        i = 0;
        var enu = operands.Split(' ');
        while (enu.MoveNext())
        {
            if ((enu.Current.End.Value - enu.Current.Start.Value) != 0)
            {
                results[i] = (enu.Current.Start.Value, 0, 0, 0, 0);
                should_multiply[i] = operands[enu.Current.Start] == '*';
                i++;
            }
        }

        fixed (void* p = results)
        {
            var line_enu = lines.Split('\n');
            while (line_enu.MoveNext())
            {
                var line = lines[line_enu.Current];
                enu = line.Split(' ');
                (int, int, int, int, int)* pt = ((int, int, int, int, int)*)p;

                while (enu.MoveNext())
                {
                    uint start = (uint)enu.Current.Start.Value;
                    if ((enu.Current.End.Value - start) != 0)
                    {
                        var num = line[enu.Current].Trim();
                        uint curr = uint.Parse(num);
                        uint* ptr = (uint*)pt;
                        uint offset = start - (*ptr) + 1;
                        ptr += offset;

                        switch (num.Length)
                        {
                            case 4:
                                *(ptr) = 10 * (*(ptr)) + ((curr / 1000) % 10);
                                ptr++;
                                goto case 3;
                            
                            case 3:
                                *(ptr) = 10 * (*(ptr)) + ((curr / 100) % 10);
                                ptr++;
                                goto case 2;

                            case 2:
                                *(ptr) = 10 * (*(ptr)) + ((curr / 10) % 10);
                                ptr++;
                                goto case 1;
                            
                            case 1:
                                *(ptr) = 10 * (*(ptr)) + (curr % 10);
                                break;
                        }

                        pt++;
                    }
                }
            }
        }

        long sum = 0;
        for (i = 0; i < results.Length; i++)
        {
            switch(should_multiply[i])
            {
                case true:
                    sum += Math.Max(results[i].value1, 1L)
                        * Math.Max(results[i].value2, 1L)
                        * Math.Max(results[i].value3, 1L)
                        * Math.Max(results[i].value4, 1L);

                    break;

                case false:
                    sum += results[i].value1
                        + results[i].value2
                        + results[i].value3
                        + results[i].value4;

                    break;
            }
        }

        return sum;
    }
}