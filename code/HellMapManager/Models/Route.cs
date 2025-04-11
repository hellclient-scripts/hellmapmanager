using System.Collections.Generic;

namespace HellMapManager.Models;

public class Route
{
    public Route() { }
    public string Key { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Group { get; set; } = "";

    public List<string> Rooms = [];
    public bool Validated()
    {
        return Key != "";
    }

}