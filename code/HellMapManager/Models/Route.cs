using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HellMapManager.Models;

public class Route
{
    public Route() { }
    public string Key { get; set; } = "";
    public string Desc { get; set; } = "";
    public List<Room> Rooms = [];
}