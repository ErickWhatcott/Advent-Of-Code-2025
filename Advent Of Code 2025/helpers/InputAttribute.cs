namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Method)]
public class DefineInputAttribute : Attribute
{
    public string File;
    public DefineInputAttribute(InputType it) : this(it.ToString()){}
    public DefineInputAttribute(string name) => File = name;
}