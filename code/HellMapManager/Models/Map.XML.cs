using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics.CodeAnalysis;
namespace HellMapManager.Models;

public class MapXML
{

}
public partial class Map
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Map))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MapInfo))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Alias))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Room))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Exit))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Route))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Variable))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Landmark))]
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
    public static Map? FromXML(string data)
    {
        using (StringReader reader = new StringReader(data))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            return (Map?)serializer.Deserialize(reader);
        }

    }
}