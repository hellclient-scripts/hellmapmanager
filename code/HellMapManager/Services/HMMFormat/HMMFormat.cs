using System.Xml.Serialization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HellMapManager.Models;
using System.IO;
using System.Linq;
using System;

namespace HellMapManager.Services.HMMFormat;

public class HMMSnapshot
{
    public static HMMSnapshot FromModel(Snapshot model)
    {
        return new HMMSnapshot()
        {
            Key = model.Key,
            Data = model.Data.ConvertAll(HMMRoomData.FromModel),
            Group = model.Group,
            Timestamp = model.Timestamp,
        };
    }
    public Snapshot ToModel()
    {
        return new Snapshot
        {
            Key = Key,
            Data = [.. Data.ConvertAll(d => d.ToModel()).Where(d => d.Validated())],
            Group = Group,
            Timestamp = Timestamp,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public int Timestamp = 0;

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlElement(ElementName = "Data")]
    public List<HMMRoomData> Data { get; set; } = [];

}
public class HMMShortcut
{
    public static HMMShortcut FromModel(Shortcut model)
    {
        return new HMMShortcut()
        {
            Key = model.Key,
            Group = model.Group,
            Desc = model.Desc,
            RoomTags = model.RoomTags,
            RoomExTags = model.RoomExTags,
            Command = model.Command,
            To = model.To,
            Tags = model.Tags,
            ExTags = model.ExTags,
            Cost = model.Cost,
        };
    }
    public Shortcut ToModel()
    {
        return new Shortcut
        {
            Key = Key,
            Group = Group,
            Desc = Desc,
            RoomTags = RoomTags,
            RoomExTags = RoomExTags,
            Command = Command,
            To = To,
            Tags = Tags,
            ExTags = ExTags,
            Cost = Cost,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlElement(ElementName = "RoomTag")]
    public List<string> RoomTags { get; set; } = [];

    [XmlElement(ElementName = "RoomExTag")]
    public List<string> RoomExTags { get; set; } = [];
    [XmlAttribute]
    public string Command { get; set; } = "";


    [XmlAttribute]
    public string To { get; set; } = "";

    [XmlElement(ElementName = "Tag")]
    public List<string> Tags { get; set; } = [];

    [XmlElement(ElementName = "ExTag")]
    public List<string> ExTags { get; set; } = [];

    [XmlAttribute]
    public int Cost { get; set; } = 1;

    [XmlText]
    public string Desc { get; set; } = "";
}
public class HMMTrace
{
    public static HMMTrace FromModel(Trace model)
    {
        return new HMMTrace()
        {
            Key = model.Key,
            Locations = model.Locations,
            Group = model.Group,
            Desc = model.Desc,
        };
    }
    public Trace ToModel()
    {
        return new Trace
        {
            Key = Key,
            Locations = Locations,
            Group = Group,
            Desc = Desc,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlElement(ElementName = "Location")]
    public List<string> Locations { get; set; } = [];

    [XmlText]
    public string Desc { get; set; } = "";
}
public class HMMRegionItem
{
    public static HMMRegionItem FromModel(RegionItem model)
    {
        return new HMMRegionItem()
        {
            Type = model.Type == RegionItemType.Zone ? "Zone" : "Room",
            Value = model.Value,
        };
    }
    public RegionItem ToModel()
    {
        return new RegionItem(Type == "Zone" ? RegionItemType.Zone : RegionItemType.Room, Value);
    }

    [XmlAttribute]
    public string Type { get; set; } = "";

    [XmlAttribute]
    public string Value { get; set; } = "";
}

public class HMMRegion
{
    public static HMMRegion FromModel(Region model)
    {
        return new HMMRegion()
        {
            Key = model.Key,
            Items = model.Items.ConvertAll(HMMRegionItem.FromModel),
            Group = model.Group,
            Desc = model.Desc,
        };
    }
    public Region ToModel()
    {
        return new Region
        {
            Key = Key,
            Items = [.. Items.ConvertAll(i => i.ToModel()).Where(i => i.Validated())],
            Group = Group,
            Desc = Desc,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlElement(ElementName = "Item")]
    public List<HMMRegionItem> Items { get; set; } = [];

    [XmlText]
    public string Desc { get; set; } = "";

}
public class HMMRoute
{
    public static HMMRoute FromModel(Route model)
    {
        return new HMMRoute()
        {
            Key = model.Key,
            Rooms = model.Rooms,
            Group = model.Group,
            Desc = model.Desc,
        };
    }
    public Route ToModel()
    {
        return new Route
        {
            Key = Key,
            Rooms = Rooms,
            Group = Group,
            Desc = Desc,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlElement(ElementName = "Room")]
    public List<string> Rooms = [];

    [XmlText]
    public string Desc { get; set; } = "";
}

public class HMMVariable
{
    public static HMMVariable FromModel(Variable model)
    {
        return new HMMVariable()
        {
            Key = model.Key,
            Value = model.Value,
            Group = model.Group,
            Desc = model.Desc,
        };
    }
    public Variable ToModel()
    {
        return new Variable
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Value { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlText]
    public string Desc { get; set; } = "";
}
public class HMMLandmark
{
    public static HMMLandmark FromModel(Landmark model)
    {
        return new HMMLandmark()
        {
            Key = model.Key,
            Value = model.Value,
            Type = model.Type,
            Desc = model.Desc,
        };
    }
    public Landmark ToModel()
    {
        return new Landmark
        {
            Key = Key,
            Type = Type,
            Value = Value,
            Desc = Desc,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Type { get; set; } = "";

    [XmlAttribute]
    public string Value { get; set; } = "";

    [XmlText]
    public string Desc { get; set; } = "";

}

public class HMMAlias
{
    public static HMMAlias FromModel(Alias model)
    {
        return new HMMAlias()
        {
            Key = model.Key,
            Value = model.Value,
            Group = model.Group,
            Desc = model.Desc,
        };
    }
    public Alias ToModel()
    {
        return new Alias
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Value { get; set; } = "";

    [XmlText]
    public string Desc { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

}

public class HMMRoomData
{
    public static HMMRoomData FromModel(RoomData model)
    {
        return new HMMRoomData()
        {
            Key = model.Key,
            Value = model.Value,
        };
    }
    public RoomData ToModel()
    {
        return new RoomData(Key, Value);
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Value { get; set; } = "";

}

public class HMMExit
{
    public static HMMExit FromModel(Exit model)
    {
        return new HMMExit
        {
            Command = model.Command,
            To = model.To,
            Tags = model.Tags,
            ExTags = model.ExTags,
            Cost = model.Cost,
        };
    }
    public Exit ToModel()
    {

        return new Exit()
        {
            Command = Command,
            To = To,
            Tags = Tags,
            ExTags = ExTags,
            Cost = Cost,
        };
    }
    [XmlAttribute]
    public string Command { get; set; } = "";
    [XmlAttribute]
    public string To { get; set; } = "";
    [XmlElement(ElementName = "Tag")]
    public List<string> Tags { get; set; } = [];
    [XmlElement(ElementName = "ExTag")]
    public List<string> ExTags { get; set; } = [];
    [XmlAttribute]
    public int Cost { get; set; } = 1;
}
public class HMMRoom
{
    public static HMMRoom FromModel(Room model)
    {
        return new HMMRoom()
        {
            Key = model.Key,
            Name = model.Name,
            Desc = model.Desc,
            Group = model.Group,
            Tags = model.Tags,
            Exits = model.Exits.ConvertAll(HMMExit.FromModel),
            Data = model.Data.ConvertAll(HMMRoomData.FromModel)
        };
    }
    public Room ToModel()
    {
        return new Room
        {
            Key = Key,
            Name = Name,
            Desc = Desc,
            Group = Group,
            Tags = Tags,
            Exits = [.. Exits.ConvertAll(e => e.ToModel()).Where(e => e.Validated())],
            Data = [.. Data.ConvertAll(d => d.ToModel()).Where(d => d.Validated())],
        };
    }

    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Name { get; set; } = "";

    [XmlText]
    public string Desc { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlElement(ElementName = "Tag")]
    public List<string> Tags = [];

    [XmlElement(ElementName = "Exit")]
    public List<HMMExit> Exits { get; set; } = [];

    [XmlElement(ElementName = "Data")]
    public List<HMMRoomData> Data { get; set; } = [];

}

[XmlRootAttribute("Map")]
public class HMMMap
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMMap))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRoomData))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMExit))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRoom))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMAlias))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMLandmark))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMVariable))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRoute))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRegion))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMTrace))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMShortcut))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMSnapshot))]

    public HMMMap()
    {
        typeof(List<HMMRoom>).GetDefaultMembers();
        typeof(List<HMMAlias>).GetDefaultMembers();
        typeof(List<HMMLandmark>).GetDefaultMembers();
        typeof(List<HMMVariable>).GetDefaultMembers();
        typeof(List<HMMRoute>).GetDefaultMembers();
        typeof(List<HMMRegion>).GetDefaultMembers();
        typeof(List<HMMTrace>).GetDefaultMembers();
        typeof(List<HMMShortcut>).GetDefaultMembers();
        typeof(List<HMMSnapshot>).GetDefaultMembers();
        typeof(List<HMMRoomData>).GetDefaultMembers();
        typeof(List<HMMExit>).GetDefaultMembers();
        typeof(List<HMMRegionItem>).GetDefaultMembers();
    }
    [XmlAttribute]
    public string Name { get; set; } = "";

    [XmlAttribute]
    public string Desc { get; set; } = "";
    [XmlAttribute]
    public string Version { get; set; } = "";
    [XmlAttribute]
    public long UpdatedTime { get; set; } = 0;

    [XmlArray(ElementName = "Rooms")]
    [XmlArrayItem(typeof(HMMRoom), ElementName = "Room")]
    public List<HMMRoom> Rooms { get; set; } = [];


    [XmlArray(ElementName = "Aliases")]
    [XmlArrayItem(typeof(HMMAlias), ElementName = "Alias")]
    public List<HMMAlias> Aliases { get; set; } = [];


    [XmlArray(ElementName = "Landmarks")]
    [XmlArrayItem(typeof(HMMLandmark), ElementName = "Landmark")]
    public List<HMMLandmark> Landmarks { get; set; } = [];


    [XmlArray(ElementName = "Variables")]
    [XmlArrayItem(typeof(HMMVariable), ElementName = "Variable")]
    public List<HMMVariable> Variables { get; set; } = [];


    [XmlArray(ElementName = "Routes")]
    [XmlArrayItem(typeof(HMMRoute), ElementName = "Route")]
    public List<HMMRoute> Routes { get; set; } = [];


    [XmlArray(ElementName = "Regions")]
    [XmlArrayItem(typeof(HMMRegion), ElementName = "Region")]
    public List<HMMRegion> Regions { get; set; } = [];


    [XmlArray(ElementName = "Traces")]
    [XmlArrayItem(typeof(HMMTrace), ElementName = "Trace")]
    public List<HMMTrace> Traces { get; set; } = [];


    [XmlArray(ElementName = "Shortcuts")]
    [XmlArrayItem(typeof(HMMShortcut), ElementName = "Shortcut")]
    public List<HMMShortcut> Shortcuts { get; set; } = [];


    [XmlArray(ElementName = "Snapshot")]
    [XmlArrayItem(typeof(HMMSnapshot), ElementName = "Snapshot")]
    public List<HMMSnapshot> Snapshots { get; set; } = [];
    public void FromModel(Map map)
    {
        this.Version = MapInfo.CurrentVersion;
        Name = map.Info.Name;
        Desc = map.Info.Desc;
        UpdatedTime = map.Info.UpdatedTime;
        Rooms = map.Rooms.ConvertAll(HMMRoom.FromModel);
        Aliases = map.Aliases.ConvertAll(HMMAlias.FromModel);
        Landmarks = map.Landmarks.ConvertAll(HMMLandmark.FromModel);
        Variables = map.Variables.ConvertAll(HMMVariable.FromModel);
        Routes = map.Routes.ConvertAll(HMMRoute.FromModel);
        Regions = map.Regions.ConvertAll(HMMRegion.FromModel);
        Traces = map.Traces.ConvertAll(HMMTrace.FromModel);
        Shortcuts = map.Shortcuts.ConvertAll(HMMShortcut.FromModel);
        Snapshots = map.Snapshots.ConvertAll(HMMSnapshot.FromModel);
    }
    public Map ToModel()
    {
        var map = new Map
        {
            Info = new MapInfo
            {
                Name = Name,
                Desc = Desc,
                Version = MapInfo.CurrentVersion,
                UpdatedTime = UpdatedTime,
            },
            Rooms = [.. Rooms.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Aliases = [.. Aliases.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Landmarks = [.. Landmarks.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Variables = [.. Variables.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Routes = [.. Routes.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Regions = [.. Regions.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Traces = [.. Traces.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Shortcuts = [.. Shortcuts.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
            Snapshots = [.. Snapshots.ConvertAll(r => r.ToModel()).Where(r => r.Validated())],
        };
        return map;
    }
    public string ToXML()
    {
        var result = "";
        using (StringWriter writer = new())
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HMMMap));
            serializer.Serialize(writer, this);
            result = writer.ToString();
        }
        return result;
    }
    public static HMMMap? FromXML(string data)
    {
        using StringReader reader = new StringReader(data);
        XmlSerializer serializer = new XmlSerializer(typeof(HMMMap));
        return (HMMMap?)serializer.Deserialize(reader);

    }

}
