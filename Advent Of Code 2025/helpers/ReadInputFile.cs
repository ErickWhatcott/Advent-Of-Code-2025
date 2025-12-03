using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class Solution
{
    public static string ReadFullInput(string year, [CallerMemberName] string? caller = null)
        => ReadInput("FullInput", year, caller);

    public static string ReadSampleInput(string year, [CallerMemberName] string? caller = null)
        => ReadInput("SampleInput", year, caller);
        
    public static string ReadInput(string name, string year, [CallerMemberName] string? caller = null)
    {
        if(Path.GetExtension(name) == string.Empty)
            name += ".txt";
        
        if(string.IsNullOrEmpty(caller))
            throw new ArgumentException("Caller is null");

        return File.ReadAllText(Path.Combine("solutions", year, caller, name));
    }
}