using System.Runtime.CompilerServices;

namespace AdventOfCode2025;

public static partial class Solution
{
    public static string ReadFullInput([CallerMemberName] string? caller = null)
        => ReadInput("FullInput", caller);

    public static string ReadSampleInput([CallerMemberName] string? caller = null)
        => ReadInput("SampleInput", caller);
        
    public static string ReadInput(string name, [CallerMemberName] string? caller = null)
    {
        if(Path.GetExtension(name) == string.Empty)
            name += ".txt";
        
        if(string.IsNullOrEmpty(caller))
            throw new ArgumentException("Caller is null");

        return File.ReadAllText(Path.Combine("solutions", caller, name));
    }
}