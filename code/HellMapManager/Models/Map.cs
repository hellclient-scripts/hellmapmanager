using System;
using System.Collections.Generic;
using System.Text;

namespace HellMapManager.Models;

public enum MapEncoding
{
    Default,
    GB18030,
}
public partial class MapInfo
{
    public MapInfo()
    {
    }
    public string Name { get; set; } = "";
    public string NameLabel { get => Name == "" ? "<未命名>" : Name; }
    public string Desc { get; set; } = "";
    public string DescLabel { get => Desc == "" ? "<无描述>" : Desc; }
    public long UpdatedTime { get; set; } = 0;
    public static MapInfo Empty(string name, string desc)
    {
        var info = new MapInfo
        {
            UpdatedTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            Name = name,
            Desc = desc,
        };
        return info;
    }
    public bool Validated()
    {
        return UpdatedTime > -1;
    }
    public const string EncodeKey = "Info";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Name),//0
                HMMFormatter.Escape(UpdatedTime.ToString()),//1
                HMMFormatter.Escape(Desc),//2
            ])
        );
    }
    public static MapInfo Decode(string val)
    {
        var result = new MapInfo();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Name = HMMFormatter.UnescapeAt(list, 0);
        result.UpdatedTime = HMMFormatter.UnescapeIntAt(list, 0, -1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        return result;
    }
}


public partial class Map
{
    public MapEncoding Encoding { get; set; } = MapEncoding.Default;
    public static string CurrentVersion = "1.0";
    public bool Compressed { get; set; } = false;

    public MapInfo Info { get; set; } = new MapInfo();
    public List<Room> Rooms { get; set; } = [];
    public List<Alias> Aliases { get; set; } = [];
    public List<Landmark> Landmarks { get; set; } = [];
    public List<Variable> Variables { get; set; } = [];
    public List<Route> Routes { get; set; } = [];
    public List<Region> Regions { get; set; } = [];
    public List<Trace> Traces { get; set; } = [];
    public List<Shortcut> Shortcuts { get; set; } = [];
    public List<Snapshot> Snapshots { get; set; } = [];
    public List<Query> Querys { get; set; } = [];

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
    public void MarkAsModified()
    {
        Map.Info.UpdatedTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        Modified = true;
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