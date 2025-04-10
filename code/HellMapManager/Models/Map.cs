using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace HellMapManager.Models;

public partial class MapInfo
{
    public MapInfo()
    {
    }
    public static string CurrentVersion = "1.0";
    public string Name { get; set; } = "";
    public string NameLabel { get => Name == "" ? "<未命名>" : Name; }
    public string Desc { get; set; } = "";
    public string DescLabel { get => Name == "" ? "<无描述>" : Name; }
    public string Version { get; set; } = "";
    public long UpdatedTime { get; set; } = 0;
    public static MapInfo Empty(string name, string desc)
    {
        var info = new MapInfo
        {
            Version = CurrentVersion,
            UpdatedTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            Name = name,
            Desc = desc,
        };
        return info;
    }
}
public partial class Map
{
    public Map()
    {
        Info = MapInfo.Empty("", "");
        typeof(List<Room>).GetDefaultMembers();
        typeof(List<Alias>).GetDefaultMembers();
        typeof(List<Landmark>).GetDefaultMembers();
        typeof(List<Variable>).GetDefaultMembers();
        typeof(List<Route>).GetDefaultMembers();
    }

    public MapInfo Info { get; set; }
    public List<Room> Rooms { get; set; } = [];
    public List<Alias> Aliases { get; set; } = [];
    public List<Landmark> Landmarks { get; set; } = [];
    public List<Variable> Variables { get; set; } = [];
    public List<Route> Routes { get; set; } = [];
    public List<Region> Regions { get; set; } = [];
    public List<Trace> Traces { get; set; } = [];
    public List<Shortcut> Shortcuts { get; set; } = [];
    public List<Snapshot> Snapshots { get; set; } = [];
    public void Sort()
    {
        this.Rooms.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
    }
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

    public MapFile()
    {
        Map = new Map();
    }
    public Map Map { get; set; }
    public string Path = "";
    public bool Modified = true;
    public Cache Cache = new Cache();
    public static MapFile Empty(string name, string desc)
    {
        return new MapFile
        {
            Map = Map.Empty(name, desc),
        };
    }
    public RecentFile ToRecentFile()
    {
        return new RecentFile(Map.Info.Name, Path);
    }
    private void InsertRoom(Room room)
    {
        this.Map.Rooms.RemoveAll(r => r.Key == room.Key);
        this.Map.Rooms.Add(room);
        this.Cache.Rooms[room.Key] = room;
    }
    public void ImportRooms(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            InsertRoom(room);
        }
    }
    public void RebuldCache()
    {
        this.Cache = new Cache();
        foreach (var room in this.Map.Rooms)
        {
            this.Cache.Rooms[room.Key] = room;
        }
    }
}