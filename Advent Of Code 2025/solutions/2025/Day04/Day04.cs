namespace AdventOfCode;

public static partial class Solution2025
{
    [TimePart]
    [DefineInput(InputType.FullInput)]
    public static (int, int) Day04(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day04(input), Part2Day04(input));
    }

    [TimePart]
    public static int Part1Day04(string input)
    {
        int accessible = 0;

        var span = input.AsSpan();;
        int width = span.IndexOf('\n');
        int height = (span.Length / width) - 1;

        for (int i = 0, j = 0, k = 0; i < span.Length; i++)
        {
            bool hasright = j != (width - 1);

            if (span[i] == '@')
            {
                int total = 0;

                bool hasleft = j != 0;
                bool hastop = k != 0;
                bool hasbottom = k != height;

                if (hasleft)
                {
                    if (span[i - 1] == '@')
                        total++;
                    if (hastop && span[i - width - 2] == '@')
                        total++;
                    if (hasbottom && span[i + width] == '@')
                        total++;
                }

                if (hasright)
                {
                    if (span[i + 1] == '@')
                        total++;
                    if (hastop && span[i - width] == '@')
                        total++;
                    if (hasbottom && span[i + width + 2] == '@')
                        total++;
                }

                if (hastop && span[i - width - 1] == '@')
                    total++;

                if (hasbottom && span[i + width + 1] == '@')
                    total++;

                if (total < 4)
                    accessible++;
            }

            if (!hasright)
            {
                j = 0;
                i++;
                k++;
            }
            else
            {
                j++;
            }
        }
        return accessible;
    }

    [TimePart]
    public static unsafe int Part2Day04(string input)
    {
        int accessible = 0;
        int removed;

        // Prevent the string from moving around so we can modify it in place.
        fixed (char* p = input)
        {
            var span = new Span<char>(p, input.Length);
            int width = span.IndexOf('\n');
            int height = (span.Length / width) - 1;

            do
            {
                removed = 0;

                for (int i = 0, j = 0, k = 0; i < span.Length; i++)
                {
                    bool hasright = j != (width - 1);

                    if (span[i] == '@')
                    {
                        int total = 0;

                        bool hasleft = j != 0;
                        bool hastop = k != 0;
                        bool hasbottom = k != height;

                        if (hasleft)
                        {
                            if (span[i - 1] == '@')
                                total++;
                            if (hastop && span[i - width - 2] == '@')
                                total++;
                            if (hasbottom && span[i + width] == '@')
                                total++;
                        }

                        if (hasright)
                        {
                            if (span[i + 1] == '@')
                                total++;
                            if (hastop && span[i - width] == '@')
                                total++;
                            if (hasbottom && span[i + width + 2] == '@')
                                total++;
                        }

                        if (hastop && span[i - width - 1] == '@')
                            total++;

                        if (hasbottom && span[i + width + 1] == '@')
                            total++;

                        if (total < 4)
                        {
                            accessible++;
                            span[i] = '.';
                            removed++;
                        }
                    }

                    if (!hasright)
                    {
                        j = 0;
                        i++;
                        k++;
                    }
                    else
                    {
                        j++;
                    }
                }
            } while (removed > 0);

            return accessible;
        }
    }
}