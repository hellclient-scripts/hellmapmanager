using System;
using System.Collections.Generic;

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
    public static MapInfo Create(string name, string desc)
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
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey,
            HMMFormatter.EncodeList(HMMFormatter.Level1, [
                HMMFormatter.Escape(Name),//0
                HMMFormatter.Escape(UpdatedTime.ToString()),//1
                HMMFormatter.Escape(Desc),//2
            ])
        );
    }
    public static MapInfo Decode(string val)
    {
        var result = new MapInfo();
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, val);
        var list = HMMFormatter.DecodeList(HMMFormatter.Level1, kv.Value);
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
    public static Map Create(string name, string desc)
    {
        return new Map
        {
            Info = MapInfo.Create(name, desc),
        };
    }
}


