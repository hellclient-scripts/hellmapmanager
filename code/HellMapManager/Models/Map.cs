using System;
using System.Collections.Generic;
using System.Text;

namespace HellMapManager.Models;

public class MapSettings
{
    public string Name { get; set; } = "";
    public string Desc { get; set; } = "";
    public MapEncoding Encoding { get; set; } = MapEncoding.Default;
}
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
        result.UpdatedTime = HMMFormatter.UnescapeIntAt(list, 1, -1);
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

    public void Arrange()
    {
        Rooms.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Aliases.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Routes.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Traces.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Regions.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Landmarks.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Shortcuts.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Variables.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
        Snapshots.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : (x.Key != y.Key ? x.Key.CompareTo(y.Key) : x.Timestamp.CompareTo(y.Timestamp)));
        Querys.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));


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
    public void InsertRoom(Room room)
    {
        Map.Rooms.RemoveAll(r => r.Key == room.Key);
        Map.Rooms.Add(room);
        Cache.Rooms[room.Key] = room;
    }
    public void RemoveRoom(string key)
    {
        Map.Rooms.RemoveAll(r => r.Key == key);
        Cache.Rooms.Remove(key);
    }
    public void ImportRooms(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            InsertRoom(room);
        }
    }

    public void ImportAliases(List<Alias> aliases)
    {
        foreach (var alias in aliases)
        {
            InsertAlias(alias);
        }
    }
    public void InsertAlias(Alias alias)
    {
        Map.Aliases.RemoveAll(r => r.Key == alias.Key);
        Map.Aliases.Add(alias);
        Cache.Aliases[alias.Key] = alias;
    }
    public void RemoveAlias(string key)
    {
        Map.Aliases.RemoveAll(r => r.Key == key);
        Cache.Aliases.Remove(key);
    }
    public void ImportRoutes(List<Route> routes)
    {
        foreach (var route in routes)
        {
            InsertRoute(route);
        }
    }
    public void InsertRoute(Route route)
    {
        Map.Routes.RemoveAll(r => r.Key == route.Key);
        Map.Routes.Add(route);
        Cache.Routes[route.Key] = route;
    }
    public void RemoveRoute(string key)
    {
        Map.Routes.RemoveAll(r => r.Key == key);
        Cache.Routes.Remove(key);
    }


    public void RebuldCache()
    {
        Cache = new Cache();
        foreach (var room in Map.Rooms)
        {
            Cache.Rooms[room.Key] = room;
        }
    }
    public MapSettings ToSettings()
    {
        return new MapSettings
        {
            Name = Map.Info.Name,
            Desc = Map.Info.Desc,
            Encoding = Map.Encoding,
        };
    }
}