using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using AdventOfCode.Library;
using Microsoft.Diagnostics.Tracing.StackSources;
using Microsoft.Z3;

namespace AdventOfCode;

public static partial class Solution2025
{
    // (this is commented out so it will run Part 1 still instead of throwing)
    // [RequiresX64]
    // [Completed]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day10(string? input = null)
    {
        input ??= ReadFullInput();
        if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
            return (Part1Day10(input), -1);
        else
            return (Part1Day10(input), Part2Day10(input));
    }

    [TimePart]
    public static unsafe long Part1Day10(string input)
    {
        // Size represents the largest number of values that can be handled.
        const int max_bits = 9;

        // +7 so down the road we don't have to do bounds checks.
        const int size = (2 << max_bits) + 7;

        // Represents the maximum number of buttons.
        const int max_buttons = 32;

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');

        short* mask = stackalloc short[max_buttons];
        byte* grid = stackalloc byte[size];

        int count = 0;
        while (line_enu.MoveNext())
        {
            // +1 to skip the '['
            var line = lines[(line_enu.Current.Start.Value + 1)..line_enu.Current.End];

            short target_mask;
            int width;
            {
                int current_mask = 0x0;
                int index = line.IndexOf(']');
                for (int j = index - 1; j >= 0; j--)
                    current_mask = (current_mask << 1) + (line[j] == '.' ? 0 : 1);
                width = 1 << index;
                target_mask = (short)current_mask;
            }

            var enu = line.Split('(');
            enu.MoveNext(); // Move after the starting stuff.

            int height = 0;
            for (int i = 0; enu.MoveNext(); i++, height++)
            {
                var span = line[enu.Current];
                short current_mask = 0x0;

                while (span[1] != ')')
                {
                    current_mask |= (short)(1 << (span[0] - '0'));
                    span = span[2..];
                }

                current_mask |= (short)(1 << (span[0] - '0'));
                mask[i] = current_mask;
            }

            Unsafe.InitBlockUnaligned(grid+1, byte.MaxValue, (uint)width - 1);
            grid[0] = 0;

            for (int i = 0; i < height; i++) // i iterates over the masks
            {
                int mask_value = mask[i];

                // We don't need to clean up because extra padding was added to grid
                // So it will write the memory, but it won't be used.
                for (int j = 0; j < width; j += 8)
                {
                    int calc1 = grid[j ^ mask_value] + 1;
                    int calc2 = grid[(j + 1) ^ mask_value] + 1;
                    int calc3 = grid[(j + 2) ^ mask_value] + 1;
                    int calc4 = grid[(j + 3) ^ mask_value] + 1;
                    int calc5 = grid[(j + 4) ^ mask_value] + 1;
                    int calc6 = grid[(j + 5) ^ mask_value] + 1;
                    int calc7 = grid[(j + 6) ^ mask_value] + 1;
                    int calc8 = grid[(j + 7) ^ mask_value] + 1;

                    if (calc1 < grid[j])
                        grid[j] = (byte)calc1;

                    if (calc2 < grid[j + 1])
                        grid[j + 1] = (byte)calc2;

                    if (calc3 < grid[j + 2])
                        grid[j + 2] = (byte)calc3;

                    if (calc4 < grid[j + 3])
                        grid[j + 3] = (byte)calc4;

                    if (calc5 < grid[j + 4])
                        grid[j + 4] = (byte)calc5;

                    if (calc6 < grid[j + 5])
                        grid[j + 5] = (byte)calc6;

                    if (calc7 < grid[j + 6])
                        grid[j + 6] = (byte)calc7;
                    
                    if (calc8 < grid[j + 7])
                        grid[j + 7] = (byte)calc8;
                }
            }

            count += grid[target_mask];
        }

        return count;
    }

    [RequiresX64]
    public static long Part2Day10(string input)
    {
        checked
        {
            // Represents the maximum number of buttons.
            const int max_buttons = 32;

            var lines = input.AsSpan();
            var line_enu = lines.Split('\n');

            Span<short> mask = stackalloc short[max_buttons];

            int count = 0;
            while (line_enu.MoveNext())
            {
                // +1 to skip the '['
                var line = lines[(line_enu.Current.Start.Value + 1)..line_enu.Current.End];

                var enu = line.Split('(');
                enu.MoveNext(); // Move after the starting stuff.

                int height = 0;
                for (int i = 0; enu.MoveNext(); i++, height++)
                {
                    var span = line[enu.Current];
                    short current_mask = 0x0;

                    while (span[1] != ')')
                    {
                        current_mask |= (short)(1 << (span[0] - '0'));
                        span = span[2..];
                    }

                    current_mask |= (short)(1 << (span[0] - '0'));
                    mask[i] = current_mask;
                }

                Debug.Assert(height < max_buttons);

                var slice = line[(line.IndexOf('{') + 1)..^1];
                enu = slice.Split(',');

                using Context context = new();
                using Optimize optimizer = context.MkOptimize();

                // make the button count variables.
                ArithExpr total = context.MkInt(0);
                IntExpr[] press_counts = new IntExpr[height];
                for (int i = 0; i < height; i++)
                {
                    var curr = context.MkIntConst("b" + i);
                    total = context.MkAdd(curr, total);
                    press_counts[i] = curr;
                    optimizer.Add(context.MkGe(curr, context.MkInt(0)));
                }

                // assert that the sum of the times each button this depends on adds up to the value.
                for (int i = 0; enu.MoveNext(); i++)
                {
                    ArithExpr sum = context.MkInt(0);
                    for (int j = 0; j < height; j++)
                        if ((mask[j] & (1 << i)) != 0)
                            sum = context.MkAdd(sum, press_counts[j]);

                    optimizer.Add(context.MkEq(context.MkInt(short.Parse(slice[enu.Current])), sum));
                }

                // Set the objective to minimizing the total presses
                optimizer.MkMinimize(total);

                // Check if it has been solved, and if not throw.
                // If we really cared about optimization we could get rid of this in release
                var status = optimizer.Check();
                if (status == Status.SATISFIABLE && optimizer.Model.Evaluate(total) is IntNum num)
                {
                    count += num.Int;
                }
                else
                {
                    throw new InvalidOperationException($"Solver's status is: {status}");
                }
            }

            return count;
        }
    }
}