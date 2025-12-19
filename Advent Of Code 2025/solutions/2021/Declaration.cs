using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static partial class Solution2021
{
    public static string ReadFullInput([CallerMemberName] string? caller = null)
        => ReadInput("FullInput", caller);

    public static string ReadSampleInput([CallerMemberName] string? caller = null)
        => ReadInput("SampleInput", caller);
        
    public static string ReadInput(string name, [CallerMemberName] string? caller = null)
        => Solution.ReadInput(name, "2021", caller);
}