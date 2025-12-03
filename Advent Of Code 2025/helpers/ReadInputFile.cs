using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class Solution
{
    public static string? TryReadFullInput(string year, [CallerMemberName] string? caller = null)
        => TryReadInput("FullInput", year, caller);

    public static string? TryReadSampleInput(string year, [CallerMemberName] string? caller = null)
        => TryReadInput("SampleInput", year, caller);
        
    public static string? TryReadInput(string name, string year, [CallerMemberName] string? caller = null)
    {
        if(Path.GetExtension(name) == string.Empty)
            name += ".txt";
        
        if(string.IsNullOrEmpty(caller))
            throw new ArgumentException("Caller is null");
        
        string path = Path.Combine("solutions", year, caller, name);
        if(File.Exists(path))
            return File.ReadAllText(path);
        else
            return null;
    }

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