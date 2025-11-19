using System;
using System.Linq;
namespace HellMapManager.Models;

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
    public static MapFile Create(string name, string desc)
    {
        return new MapFile
        {
            Map = Map.Create(name, desc),
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
        room.Arrange();
        Map.Rooms.Add(room);
        Cache.Rooms[room.Key] = room;
    }
    public void RemoveRoom(string key)
    {
        Map.Rooms.RemoveAll(r => r.Key == key);
        Cache.Rooms.Remove(key);
    }

    public void InsertMarker(Marker marker)
    {
        Map.Markers.RemoveAll(r => r.Key == marker.Key);
        marker.Arrange();
        Map.Markers.Add(marker);
        Cache.Markers[marker.Key] = marker;
    }
    public void RemoveMarker(string key)
    {
        Map.Markers.RemoveAll(r => r.Key == key);
        Cache.Markers.Remove(key);
    }
    public void InsertRoute(Route route)
    {
        Map.Routes.RemoveAll(r => r.Key == route.Key);
        route.Arrange();
        Map.Routes.Add(route);
        Cache.Routes[route.Key] = route;
    }
    public void RemoveRoute(string key)
    {
        Map.Routes.RemoveAll(r => r.Key == key);
        Cache.Routes.Remove(key);
    }

    public void InsertTrace(Trace trace)
    {
        Map.Traces.RemoveAll(r => r.Key == trace.Key);
        trace.Arrange();
        Map.Traces.Add(trace);
        Cache.Traces[trace.Key] = trace;
    }
    public void RemoveTrace(string key)
    {
        Map.Traces.RemoveAll(r => r.Key == key);
        Cache.Traces.Remove(key);
    }
    public void InsertRegion(Region region)
    {
        Map.Regions.RemoveAll(r => r.Key == region.Key);
        region.Arrange();
        Map.Regions.Add(region);
        Cache.Regions[region.Key] = region;
    }
    public void RemoveRegion(string key)
    {
        Map.Regions.RemoveAll(r => r.Key == key);
        Cache.Regions.Remove(key);
    }

    public void InsertLandmark(Landmark landmark)
    {
        Map.Landmarks.RemoveAll(r => r.Key == landmark.Key && r.Type == landmark.Type);
        landmark.Arrange();
        Map.Landmarks.Add(landmark);
        Cache.Landmarks[landmark.UniqueKey().ToString()] = landmark;
    }
    public void RemoveLandmark(LandmarkKey key)
    {
        Map.Landmarks.RemoveAll(r => r.UniqueKey().Equal(key));
        Cache.Landmarks.Remove(key.ToString());
    }

    public void InsertShortcut(Shortcut model)
    {
        Map.Shortcuts.RemoveAll(r => r.Key == model.Key);
        model.Arrange();
        Map.Shortcuts.Add(model);
        Cache.Shortcuts[model.Key] = model;
    }
    public void RemoveShortcut(string key)
    {
        Map.Shortcuts.RemoveAll(r => r.Key == key);
        Cache.Shortcuts.Remove(key);
    }
    public void InsertVariable(Variable model)
    {
        Map.Variables.RemoveAll(r => r.Key == model.Key);
        model.Arrange();
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
        RemoveSnapshot(model.UniqueKey());
        model.Arrange();
        Map.Snapshots.Add(model);
        Cache.Snapshots[model.UniqueKey().ToString()] = model;
    }
    public void TakeSnapshot(string key, string type, string value, string group)
    {
        var snapshotKey = new SnapshotKey(key, type, value);
        var item = Map.Snapshots.FirstOrDefault(r => r.UniqueKey().Equal(snapshotKey));
        if (item != null)
        {
            item.Repeat();
        }
        else
        {
            var model = Snapshot.Create(key, type, value, group);
            model.Arrange();
            Map.Snapshots.Add(model);
            Cache.Snapshots[model.UniqueKey().ToString()] = model;
        }
    }
    public void RemoveSnapshot(SnapshotKey key)
    {
        Map.Snapshots.RemoveAll(r => r.UniqueKey().Equal(key));
        Cache.Snapshots.Remove(key.ToString());
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