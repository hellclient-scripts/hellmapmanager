using System.Xml.Serialization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HellMapManager.Models;
using System.IO;

namespace HellMapManager.Services.HMMFormat;

public class HMMSnapshot
{
    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public int Timestamp = 0;

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlArray(ElementName = "Data")]
    [XmlArrayItem(typeof(HMMRoomData))]
    public List<HMMRoomData> Data { get; set; } = [];

}
public class HMMShortcut
{
    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlArray(ElementName = "RoomTag")]
    [XmlArrayItem(typeof(string))]
    public List<string> RoomTags { get; set; } = [];

    [XmlArray(ElementName = "RoomExTag")]
    [XmlArrayItem(typeof(string))]
    public List<string> RoomExTags { get; set; } = [];
    [XmlAttribute]
    public string Command { get; set; } = "";

    [XmlAttribute]
    public string To { get; set; } = "";

    [XmlElement(ElementName = "Tag", Type = typeof(string))]
    public List<string> Tags { get; set; } = [];

    [XmlElement(ElementName = "ExTag", Type = typeof(string))]
    public List<string> ExTags { get; set; } = [];

    [XmlAttribute]
    public int Cost { get; set; } = 1;

    [XmlText]
    public string Desc { get; set; } = "";
}
public class HMMTrace
{
    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlArray(ElementName = "Location")]
    [XmlArrayItem(typeof(string))]
    public List<string> Locations { get; set; } = [];

    [XmlText]
    public string Desc { get; set; } = "";
}
public class HMMRegionItem
{
    [XmlAttribute]
    public string Type { get; set; } = "";

    [XmlAttribute]
    public string Value { get; set; } = "";
}

public class HMMRegion
{
    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlArray(ElementName = "Item")]
    [XmlArrayItem(typeof(string))]
    public List<HMMRegionItem> Items { get; set; } = [];

    [XmlText]
    public string Desc { get; set; } = "";

}
public class HMMRoute
{
    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlArray(ElementName = "Room")]
    [XmlArrayItem(typeof(string))]
    public List<string> Rooms = [];

    [XmlText]
    public string Desc { get; set; } = "";
}

public class HMMVariable
{
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
    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Value { get; set; } = "";

}

public class HMMExit
{
    [XmlAttribute]
    public string Command { get; set; } = "";

    [XmlAttribute]
    public string To { get; set; } = "";

    [XmlElement(ElementName = "Tag", Type = typeof(string))]
    public List<string> Tags { get; set; } = [];

    [XmlElement(ElementName = "ExTag", Type = typeof(string))]
    public List<string> ExTags { get; set; } = [];

    [XmlAttribute]
    public int Cost { get; set; } = 1;
}
public class HMMRoom
{
    public HMMRoom()
    {

    }
    [XmlAttribute]
    public string Key { get; set; } = "";

    [XmlAttribute]
    public string Name { get; set; } = "";

    [XmlText]
    public string Desc { get; set; } = "";

    [XmlAttribute]
    public string Group { get; set; } = "";

    [XmlElement(ElementName = "Tag", Type = typeof(string))]
    public List<string> Tags = [];

    [XmlElement(ElementName = "Exit", Type = typeof(HMMExit))]
    public List<HMMExit> Exits { get; set; } = [];

    [XmlElement(ElementName = "Exit", Type = typeof(HMMRoomData))]
    public List<HMMRoomData> Data { get; set; } = [];

}

[XmlRootAttribute("Map")]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMMap))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRoomData))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMExit))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRoom))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMAlias))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMLandmark))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMVariable))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRoute))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMRegion))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMTrace))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMShortcut))]
[method: DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HMMSnapshot))]
public class HMMMap()
{
    [XmlAttribute]
    public string Name { get; set; } = "";

    [XmlAttribute]
    public string Desc { get; set; } = "";
    [XmlAttribute]
    public string Version { get; set; } = "";
    [XmlAttribute]
    public long UpdatedTime { get; set; } = 0;

    [XmlArray(ElementName = "Rooms")]
    [XmlArrayItem(typeof(HMMRoom))]
    public List<HMMRoom> Rooms { get; set; } = [];

    [XmlArray(ElementName = "Aliases")]
    [XmlArrayItem(typeof(HMMAlias))]
    public List<HMMAlias> Aliases { get; set; } = [];

    [XmlArray(ElementName = "Landmarks")]
    [XmlArrayItem(typeof(HMMLandmark))]
    public List<HMMLandmark> Landmarks { get; set; } = [];

    [XmlArray(ElementName = "Variables")]
    [XmlArrayItem(typeof(HMMVariable))]
    public List<HMMVariable> Variables { get; set; } = [];

    [XmlArray(ElementName = "Routes")]
    [XmlArrayItem(typeof(HMMRoute))]
    public List<HMMRoute> Routes { get; set; } = [];

    [XmlArray(ElementName = "Regions")]
    [XmlArrayItem(typeof(HMMRegion))]
    public List<HMMRegion> Regions { get; set; } = [];

    [XmlArray(ElementName = "Traces")]
    [XmlArrayItem(typeof(HMMTrace))]
    public List<HMMTrace> Traces { get; set; } = [];

    [XmlArray(ElementName = "Shortcuts")]
    [XmlArrayItem(typeof(HMMShortcut))]
    public List<HMMShortcut> Shortcuts { get; set; } = [];

    [XmlArray(ElementName = "Shortcuts")]
    [XmlArrayItem(typeof(HMMSnapshot))]
    public List<HMMSnapshot> Snapshots { get; set; } = [];
    public void FromMap(Map map)
    {

    }
    public Map ToMap()
    {
        var map = Map.Empty(this.Name, this.Desc);
        return map;
    }
    public string ToXML()
    {
        var result = "";
        using (StringWriter writer = new StringWriter())
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(writer, this);
            result = writer.ToString();
        }
        return result;
    }
    public Map? FromXML(string data)
    {
        using (StringReader reader = new StringReader(data))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HMMMap));
            return (Map?)serializer.Deserialize(reader);
        }

    }

}
