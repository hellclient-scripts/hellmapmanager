using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace HellMapManager.Models;

public class Exit
{
    public Exit() { }
    //路径指令
    public string Command { get; set; } = "";
    //触发房间
    public string From { get; set; } = "";
    //目标房间
    public string To { get; set; } = "";
    [XmlArray(ElementName = "Tags")]
    [XmlArrayItem(typeof(string))]

    public List<string> Tags = [];
    [XmlArray(ElementName = "ExTags")]
    [XmlArrayItem(typeof(string))]

    public List<string> ExTags = [];
    public int Cost = 1;
    //禁用标记位
    public bool Disabled = false;
    //最后更新时间
}
