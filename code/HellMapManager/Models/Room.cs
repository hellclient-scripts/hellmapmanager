using System;
using System.Collections.Generic;

namespace HellMapManager.Models;

//房间的数据结构
public class Room
{
    //房间的key,必须唯一，不能为空
    public string Key { get; set; } = "";
    //房间的名称，显示用
    public string Name { get; set; } = "";
    //房间的描述，显示用
    public string Desc { get; set; } = "";
    //房间的区域，筛选用
    public string Zone { get; set; } = "";
    //房间的分组名，筛选用
    public string Group { get; set; } = "";
    //标签列表，筛选用
    public List<string> Tags = [];
    //禁用标记位
    public bool Disabled = false;
    //房间出口列表
    public List<Exit> Exits = [];
}