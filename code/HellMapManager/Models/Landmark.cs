namespace HellMapManager.Models;

public class Landmark
{
    public Landmark() { }
    public string Key { get; set; } = "";
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Group { get; set; } = "";
    public bool Disabled { get; set; } = false;
}