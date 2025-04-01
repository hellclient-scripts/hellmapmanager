using System;
using System.Collections.Generic;

namespace HellMapManager.Models;

//房间的数据结构
public class Room
{
    //房间的key,必须唯一，不能为空
    public String Key { get; set; } = "";
    //房间的名称，显示用
    public String Name { get; set; } = "";
    //房间的描述，显示用
    public String Desc { get; set; } = "";
    //房间的区域，筛选用
    public String Zone { get; set; } = "";
    //房间的分组名，筛选用
    public String Group { get; set; } = "";
    //标签列表，筛选用
    public List<String> Tags = [];
    //禁用标记位
    public bool Disabled = false;
    //最后更新时间

    public List<Path> Paths = [];
}