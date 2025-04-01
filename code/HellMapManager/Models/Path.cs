using System;
using System.Collections.Generic;

namespace HellMapManager.Models;

public class Path
{
    //唯一ID，UUID
    public String ID { get; set; } = "";
    //路径指令
    public String Command { get; set; } = "";
    //触发房间
    public String From { get; set; } = "";
    //目标房间
    public String To { get; set; } = "";
    public List<String> Tags = [];
    public List<String> ExTags = [];
    public int Cost = 1;
    //禁用标记位
    public bool Disabled = false;
    //最后更新时间
}
