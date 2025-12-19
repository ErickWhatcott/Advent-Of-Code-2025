using System.Collections;

namespace AdventOfCode;

public static partial class Solution2024
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, int) Day02(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day02(input), Part2Day02(input));
    }

    public static int Part1Day02(string input)
    {
        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        int count = 0;

        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var enu = line.Split(' ');

            enu.MoveNext();
            int last_num = int.Parse(line[enu.Current]);
            enu.MoveNext();
            int curr_num = int.Parse(line[enu.Current]);

            int last_dif = last_num - curr_num;

            while (enu.MoveNext())
            {
                if (last_dif == 0 || Math.Abs(last_dif) > 3)
                    goto end;

                last_num = curr_num;
                curr_num = int.Parse(line[enu.Current]);
                int curr_dif = last_num - curr_num;

                if (curr_dif < 0 != last_dif < 0)
                    goto end;
                last_dif = curr_dif;
            }

            if (last_dif != 0 && Math.Abs(last_dif) <= 3)
                count++;

        end:
            continue;
        }

        return count;
    }

    public static int Part2Day02(string input)
    {
        Span<int> buffer = stackalloc int[16];
        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        int count = 0;

        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var enu = line.Split(' ');

            int i = 0;
            while (enu.MoveNext())
                buffer[i++] = int.Parse(line[enu.Current]);

            var curr_buffer = buffer[..i];
            for (int j = -1; j < i; j++)
                if (IsSatisfactory(curr_buffer, j))
                {
                    count++;
                    break;
                }
        }

        return count;
    }

    private static bool IsSatisfactory(Span<int> buffer, int skip)
    {
        var enu = new SkipIndexEnumerator(buffer, skip);
        enu.MoveNext();
        int last_num = enu.Current;
        enu.MoveNext();
        int curr_num = enu.Current;

        int last_dif = last_num - curr_num;

        while (enu.MoveNext())
        {
            if (last_dif == 0 || Math.Abs(last_dif) > 3)
                return false;

            last_num = curr_num;
            curr_num = enu.Current;
            int curr_dif = last_num - curr_num;

            if (curr_dif < 0 != last_dif < 0)
                return false;
            last_dif = curr_dif;
        }

        if (last_dif != 0 && Math.Abs(last_dif) <= 3)
            return true;
        else
            return false;
    }

    private ref struct SkipIndexEnumerator : IEnumerator<int>
    {
        private Span<int> _buffer;
        private int _skip;
        private int _i;
        public SkipIndexEnumerator(Span<int> buffer, int skip)
        {
            _buffer = buffer;
            _skip = skip;
            _i = -1;
        }

        public readonly int Current => _buffer[_i];
        readonly object IEnumerator.Current => Current;

        public readonly void Dispose() { }

        public bool MoveNext()
        {
            _i++;
            if (_i == _skip)
                _i++;
            return _i < _buffer.Length;
        }

        public void Reset()
        {
            _i = -1;
        }
    }
}