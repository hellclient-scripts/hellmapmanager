using System;
using System.Collections.Generic;

namespace HellMapManager.Models;
public class MapInfo
{
    public static String CurrentVersion = "1.0";
    public String Name = "";
    public String Desc = "";
    public String Version = "";
    public required DateTime UpdatedTime;
    public static MapInfo Empty(String name, String desc)
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
    public required MapInfo Info;
    public List<Room> Rooms = [];

    public List<Path> Paths = [];
    public static Map Empty(String name, String desc)
    {
        return new Map
        {
            Info = MapInfo.Empty(name, desc),
        };
    }
}

public class MapFile
{
    public required Map Map;
    public String Path = "";
}