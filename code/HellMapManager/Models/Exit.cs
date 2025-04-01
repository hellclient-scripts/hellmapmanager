using System;
using System.Collections.Generic;

namespace HellMapManager.Models;

public class Exit
{
    //路径指令
    public string Command { get; set; } = "";
    //触发房间
    public string From { get; set; } = "";
    //目标房间
    public string To { get; set; } = "";
    public List<string> Tags = [];
    public List<string> ExTags = [];
    public int Cost = 1;
    //禁用标记位
    public bool Disabled = false;
    //最后更新时间
}
