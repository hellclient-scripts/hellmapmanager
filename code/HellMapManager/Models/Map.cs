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
    public MapInfo Clone()
    {
        return new MapInfo()
        {
            Name = Name,
            Desc = Desc,
            UpdatedTime = UpdatedTime,
        };
    }
    public bool Equal(MapInfo model)
    {
        if (Name != model.Name)
        {
            return false;
        }
        if (Desc != model.Desc)
        {
            return false;
        }
        if (UpdatedTime != model.UpdatedTime)
        {
            return false;
        }
        return true;

    }
}


public partial class Map
{
    public MapEncoding Encoding { get; set; } = MapEncoding.Default;
    public const string CurrentVersion = "1.0";

    public MapInfo Info { get; set; } = new MapInfo();
    public List<Room> Rooms { get; set; } = [];
    public List<Marker> Markers { get; set; } = [];
    public List<Landmark> Landmarks { get; set; } = [];
    public List<Variable> Variables { get; set; } = [];
    public List<Route> Routes { get; set; } = [];
    public List<Region> Regions { get; set; } = [];
    public List<Trace> Traces { get; set; } = [];
    public List<Shortcut> Shortcuts { get; set; } = [];
    public List<Snapshot> Snapshots { get; set; } = [];

    public void Arrange()
    {
        Room.Sort(Rooms);
        Marker.Sort(Markers);
        Route.Sort(Routes);
        Trace.Sort(Traces);
        Region.Sort(Regions);
        Landmark.Sort(Landmarks);
        Shortcut.Sort(Shortcuts);
        Variable.Sort(Variables);
        Snapshot.Sort(Snapshots);
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
    public Cache Cache = new();
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

    public void ImportMarkers(List<Marker> markers)
    {
        foreach (var marker in markers)
        {
            InsertMarker(marker);
        }
    }
    public void InsertMarker(Marker marker)
    {
        Map.Markers.RemoveAll(r => r.Key == marker.Key);
        Map.Markers.Add(marker);
        Cache.Markers[marker.Key] = marker;
    }
    public void RemoveMarker(string key)
    {
        Map.Markers.RemoveAll(r => r.Key == key);
        Cache.Markers.Remove(key);
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

    public void ImportTraces(List<Trace> traces)
    {
        foreach (var trace in traces)
        {
            InsertTrace(trace);
        }
    }
    public void InsertTrace(Trace trace)
    {
        Map.Traces.RemoveAll(r => r.Key == trace.Key);
        Map.Traces.Add(trace);
        Cache.Traces[trace.Key] = trace;
    }
    public void RemoveTrace(string key)
    {
        Map.Regions.RemoveAll(r => r.Key == key);
        Cache.Regions.Remove(key);
    }
    public void ImportRegions(List<Region> regions)
    {
        foreach (var region in regions)
        {
            InsertRegion(region);
        }
    }
    public void InsertRegion(Region region)
    {
        Map.Regions.RemoveAll(r => r.Key == region.Key);
        Map.Regions.Add(region);
        Cache.Regions[region.Key] = region;
    }
    public void RemoveRegion(string key)
    {
        Map.Regions.RemoveAll(r => r.Key == key);
        Cache.Regions.Remove(key);
    }

    public void ImportLandmarks(List<Landmark> models)
    {
        foreach (var model in models)
        {
            InsertLandmark(model);
        }
    }
    public void InsertLandmark(Landmark landmark)
    {
        Map.Landmarks.RemoveAll(r => r.Key == landmark.Key && r.Type == landmark.Type);
        Map.Landmarks.Add(landmark);
        Cache.Landmarks[landmark.UniqueKey] = landmark;
    }
    public void RemoveLandmark(string key, string Type)
    {
        Map.Landmarks.RemoveAll(r => r.Key == key && r.Type == Type);
        Cache.Landmarks.Remove(key);
    }

    public void ImportShortcuts(List<Shortcut> models)
    {
        foreach (var model in models)
        {
            InsertShortcut(model);
        }
    }
    public void InsertShortcut(Shortcut model)
    {
        Map.Shortcuts.RemoveAll(r => r.Key == model.Key);
        Map.Shortcuts.Add(model);
        Cache.Shortcuts[model.Key] = model;
    }
    public void RemoveShortcut(string key)
    {
        Map.Shortcuts.RemoveAll(r => r.Key == key);
        Cache.Shortcuts.Remove(key);
    }
    public void ImportVariables(List<Variable> models)
    {
        foreach (var model in models)
        {
            InsertVariable(model);
        }
    }
    public void InsertVariable(Variable model)
    {
        Map.Variables.RemoveAll(r => r.Key == model.Key);
        Map.Variables.Add(model);
        Cache.Variables[model.Key] = model;
    }
    public void RemoveVariable(string key)
    {
        Map.Variables.RemoveAll(r => r.Key == key);
        Cache.Variables.Remove(key);
    }
    public void InsertSnapshot(Snapshot model)
    {
        RemoveSnapshot(model.Key, model.Type, model.Value);
        Map.Snapshots.Add(model);
    }
    public void RemoveSnapshot(string key, string type, string value)
    {
        Map.Snapshots.RemoveAll(r => r.Key == key && r.Type == type && r.Value == value);
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