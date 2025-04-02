using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace HellMapManager.Models;

public class Exit
{
    public Exit() { }
    [XmlAttribute]
    //路径指令

    public string Command { get; set; } = "";
    [XmlAttribute]
    //目标房间
    public string To { get; set; } = "";
    [XmlText]
    public string Desc { get; set; } = "";
    [XmlElement(ElementName = "Tag", Type = typeof(string))]

    public List<string> Tags = [];
    [XmlElement(ElementName = "ExTag", Type = typeof(string))]

    public List<string> ExTags = [];
    [XmlAttribute]
    public int Cost = 1;
    //禁用标记位
    [XmlIgnore]
    public bool Disabled = false;
    [XmlAttribute("Disabled")]
    public int DisabledInXML
    {
        get => Disabled ? 1 : 0;
        set { Disabled = (value == 1); }
    }
}
