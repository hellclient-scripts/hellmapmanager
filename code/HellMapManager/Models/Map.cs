using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
namespace HellMapManager.Models;

[XmlRootAttribute("Info")]
public partial class MapInfo
{
    public MapInfo()
    {
    }
    public static string CurrentVersion = "1.0";
    [XmlAttribute]
    public string Name { get; set; } = "";
    public string NameLabel{get=>Name==""?"<未命名>":Name;}
    [XmlText]
    public string Desc { get; set; } = "";
    public string DescLabel{get=>Name==""?"<无描述>":Name;}
    [XmlAttribute]
    public string Version { get; set; } = "";
    [XmlAttribute]
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
[XmlRootAttribute("Map")]
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
    [XmlArray(ElementName = "Rooms")]
    [XmlArrayItem(typeof(Room))]
    public List<Room> Rooms { get; set; } = [];
    [XmlArray(ElementName = "Aliases")]
    [XmlArrayItem(typeof(Alias))]
    public List<Alias> Aliases { get; set; } = [];
    [XmlArray(ElementName = "Landmarks")]
    [XmlArrayItem(typeof(Landmark))]
    public List<Landmark> Landmarks { get; set; } = [];
    [XmlArray(ElementName = "Variables")]
    public List<Variable> Variables { get; set; } = [];
    [XmlArray(ElementName = "Routes")]
    [XmlArrayItem(typeof(Route))]
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

    public MapFile()
    {
        Map = new Map();
    }
    public Map Map { get; set; }
    public string Path = "";
    public bool Modified = true;
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
}