using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace HellMapManager.Models;

public class Exit
{
    public Exit() { }
    //路径指令
    [XmlAttribute]
    public string Command { get; set; } = "";
    [XmlAttribute]
    //目标房间
    public string To { get; set; } = "";
    public List<string> Tags = [];
    [XmlElement(ElementName = "ExTag", Type = typeof(string))]
    public List<string> ExTags = [];
    [XmlAttribute]
    public int Cost = 1;
    //禁用标记位
}
