namespace HellMapManager.Models;
using System.Diagnostics.CodeAnalysis;

public class Variable
{
    public Variable() { }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";

    public string Desc { get; set; } = "";
}