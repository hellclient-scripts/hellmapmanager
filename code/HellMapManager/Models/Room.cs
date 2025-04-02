using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace HellMapManager.Models;

//房间的数据结构
[XmlRootAttribute("Room")]
public class Room
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Room))]
    public Room()
    {
    }
    //房间的key,必须唯一，不能为空
    public string Key { get; set; } = "";
    //房间的名称，显示用
    public string Name { get; set; } = "";
    //房间的描述，显示用
    public string Desc { get; set; } = "";
    //房间的区域，筛选用
    public string Zone { get; set; } = "";
    //标签列表，筛选用
    [XmlArray(ElementName = "Tags")]
    [XmlArrayItem(typeof(string))]
    public List<string> Tags = [];
    //禁用标记位
    public bool Disabled = false;
    //房间出口列表
    [XmlArray(ElementName = "Exits")]
    [XmlArrayItem(typeof(Exit))]
    public List<Exit> Exits = [];
}