using System;
using System.Collections.Generic;

namespace HellMapManager.Models;
public class MapInfo
{
    public static string CurrentVersion = "1.0";
    public string Name { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Version { get; set; } = "";
    public required DateTime UpdatedTime { get; set; }
    public static MapInfo Empty(string name, string desc)
    {
        var info = new MapInfo
        {
            Version = CurrentVersion,
            UpdatedTime = new DateTime(),
            Name = name,
            Desc = desc,
        };
        return info;
    }
}
public class Map
{
    public required MapInfo Info { get; set; }
    public List<Room> Rooms { get; set; } = [];
    public List<Alias> Aliases { get; set; } = [];
    public List<Landmark> Landmarks { get; set; } = [];
    public List<Variable> Variables { get; set; } = [];
    public List<Route> Routes { get; set; } = [];
    public static Map Empty(string name, string desc)
    {
        return new Map
        {
            Info = MapInfo.Empty(name, desc),
        };
    }
}

public class MapFile
{
    public required Map Map { get; set; }
    public string Path = "";
    public bool Modified = true;
    public static MapFile Empty(string name, string desc)
    {
        return new MapFile
        {
            Map = Map.Empty(name, desc),
        };
    }
}