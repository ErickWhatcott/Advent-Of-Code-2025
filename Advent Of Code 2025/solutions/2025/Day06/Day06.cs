using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCode;

public static partial class Solution2025
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (long, long) Day06(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day06(input), Part2Day06(input));
    }

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

        do
        {
            results[i] = (should_multiply[i] = operands[0] == '*') ? 1 : 0;
            operands = operands[1..].TrimStart();
            i++;
        } while (operands.Length > 0);

        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            int curr;
            var line = lines[line_enu.Current].Trim(' ');
            i = 0;

            index = line.IndexOf(' ');

            while (index != -1)
            {
                curr = int.Parse(line[..index]);
                results[i] = should_multiply[i] ? (results[i] * curr) : (results[i] + curr);

                line = line[(index + 1)..].TrimStart(' ');
                index = line.IndexOf(' ');
                i++;
            }

            curr = int.Parse(line);
            results[i] = should_multiply[i] ? (results[i] * curr) : (results[i] + curr);
        }

        return Day06_Sum(results);
    }

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
        int start = 0;
        do
        {
            results[i] = (start, 0, 0, 0, 0);
            should_multiply[i] = operands[0] == '*';

            var temp = operands[1..].TrimStart();
            start += operands.Length - temp.Length;
            operands = temp;

            i++;
        } while (operands.Length > 0);

        fixed (void* p = results)
        {
            var line_enu = lines.Split('\n');
            while (line_enu.MoveNext())
            {
                var line = lines[line_enu.Current];
                fixed (char* reference = line)
                {
                    (int, int, int, int, int)* pt = ((int, int, int, int, int)*)p;

                    do
                    {
                        fixed (char* actual = line)
                        {
                            index = line.IndexOf(' ');

                            var num = line[..(index == -1 ? line.Length : index)];
                            uint curr = uint.Parse(num);
                            uint* ptr = (uint*)pt;
                            uint offset = ((uint)(actual - reference)) - (*ptr) + 1;
                            ptr += offset;

                            switch (num.Length)
                            {
                                case 4:
                                    *ptr = 10 * (*ptr) + ((curr / 1000) % 10);
                                    ptr++;
                                    goto case 3;

                                case 3:
                                    *ptr = 10 * (*ptr) + ((curr / 100) % 10);
                                    ptr++;
                                    goto case 2;

                                case 2:
                                    *ptr = 10 * (*ptr) + ((curr / 10) % 10);
                                    ptr++;
                                    goto case 1;

                                case 1:
                                    *ptr = 10 * (*ptr) + (curr % 10);
                                    break;
                            }

                            line = line[(index + 1)..].TrimStart(' ');
                            pt++;
                        }
                    } while (index != -1);
                }
            }
        }

        long sum = 0;
        for (i = 0; i < results.Length; i++)
        {
            switch (should_multiply[i])
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

    // Copying the source code for IEnumerable.Sum
    // (Minus things like overflow checks because that's for nerds)
    // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Linq/src/System/Linq/Sum.cs
    private static long Day06_Sum(Span<long> results)
    {
        ref long ptr = ref MemoryMarshal.GetReference(results);

        nuint length = (nuint)results.Length;
        Vector<long> accumulator = Vector<long>.Zero;

        nuint index = 0;
        nuint limit = length - (nuint)Vector<long>.Count * 4;
        do
        {
            // Switch accumulators with each step to avoid an additional move operation
            Vector<long> data = Vector.LoadUnsafe(ref ptr, index);
            Vector<long> accumulator2 = accumulator + data;

            data = Vector.LoadUnsafe(ref ptr, index + (nuint)Vector<long>.Count);
            accumulator = accumulator2 + data;

            data = Vector.LoadUnsafe(ref ptr, index + (nuint)Vector<long>.Count * 2);
            accumulator2 = accumulator + data;

            data = Vector.LoadUnsafe(ref ptr, index + (nuint)Vector<long>.Count * 3);
            accumulator = accumulator2 + data;

            index += (nuint)Vector<long>.Count * 4;
        } while (index < limit);

        // Process remaining vectors, if any, without unrolling
        limit = length - (nuint)Vector<long>.Count;
        if (index < limit)
        {
            do
            {
                Vector<long> data = Vector.LoadUnsafe(ref ptr, index);
                Vector<long> accumulator2 = accumulator + data;
                accumulator = accumulator2;

                index += (nuint)Vector<long>.Count;
            } while (index < limit);
        }

        // Add the elements in the vector horizontally.
        // Vector.Sum doesn't perform overflow checking, instead add elements individually.
        long result = 0L;
        for (int i = 0; i < Vector<long>.Count; i++)
        {
            result += accumulator[i];
        }

        // Add any remaining elements
        while (index < length)
        {
            result += Unsafe.Add(ref ptr, index);
            index++;
        }

        return result;
    }
}