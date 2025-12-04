namespace AdventOfCode;

public static partial class Solution2024
{
    [Completed]
    [DefineInput(InputType.FullInput)]
    public static (int, int) Day01(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day01(input), Part2Day01(input));
    }

    public static int Part1Day01(string input)
    {
        List<int> arr1 = [];
        List<int> arr2 = [];

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var index = line.IndexOf(' ');
            arr1.Add(int.Parse(line[..index]));
            arr2.Add(int.Parse(line[index..]));
        }

        arr1.Sort();
        arr2.Sort();

        return arr1.Zip(arr2).Sum(a => Math.Abs(a.First - a.Second));
    }

    public static int Part2Day01(string input)
    {
        List<int> arr1 = [];
        Dictionary<int, int> arr2 = [];

        var lines = input.AsSpan();
        var line_enu = lines.Split('\n');
        while (line_enu.MoveNext())
        {
            var line = lines[line_enu.Current];
            var index = line.IndexOf(' ');

            arr1.Add(int.Parse(line[..index]));

            int num_2 = int.Parse(line[index..]);
            arr2[num_2] = arr2.GetValueOrDefault(num_2) + 1;
        }

        int sum = 0;
        foreach (var v in arr1)
            sum += v * arr2.GetValueOrDefault(v);
        return sum;
    }
}