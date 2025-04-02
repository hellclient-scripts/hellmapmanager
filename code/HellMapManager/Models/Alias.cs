namespace HellMapManager.Models;
using System.Diagnostics.CodeAnalysis;

public class Alias
{
    public Alias() { }
    public string Key{get; set;} = "";
    public string Value{get; set;} = "";
    public string Desc{get; set;} = "";
    public bool Disabled{get; set;}=false;
}