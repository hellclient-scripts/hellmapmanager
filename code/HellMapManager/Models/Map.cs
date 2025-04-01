using System;
using System.Collections.Generic;

namespace HellMapManager.Models;
public class MapInfo
{
    public static String CurrentVersion = "1.0";
    public String Name{get;set;} = "";
    public String Desc{get;set;} = "";
    public String Version{get;set;} = "";
    public required DateTime UpdatedTime{get;set;}
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
    public required MapInfo Info{get;set;}
    public List<Room> Rooms{get;set;} = [];

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
    public required Map Map{get;set;}
    public String Path = "";
    public bool Modified = true;
    public static MapFile Empty(String name,String desc){
        return new MapFile{
            Map=Map.Empty(name,desc),
        };
    }
}