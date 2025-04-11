using System.Collections.Generic;
using Avalonia.Input;

namespace HellMapManager.Models;

public partial class Trace
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public List<string> Locations { get; set; } = [];
    public Trace Clone()
    {
        return new Trace()
        {
            Key = Key,
            Locations = Locations.GetRange(0, Locations.Count),
        };

    }
    public bool Validated()
    {
        return Key != "";
    }

}

public partial class Trace
{
    public void Sort()
    {
        this.Locations.Sort((x, y) => x.CompareTo(y));
    }
    public void RemoveLocations(List<string> loctions)
    {
        foreach (var l in loctions)
        {
            this.Locations.Remove(l);
        }
        this.Sort();
    }
    public void AddLocations(List<string> loctions)
    {
        foreach (var l in loctions)
        {
            this.Locations.Remove(l);
            this.Locations.Add(l);
        }
        this.Sort();
    }
}