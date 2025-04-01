using System.Collections.Generic;

namespace HellMapManager.Models;

public class Route
{
    public string Key { get; set; } = "";
    public string Desc { get; set; } = "";
    public List<Room> Rooms = [];
}