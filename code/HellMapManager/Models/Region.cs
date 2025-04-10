using System;
using System.Collections.Generic;

namespace HellMapManager.Models;

public enum RegionItemType
{
    Room,
    Zoom,
}
public class RegionItem(RegionItemType type, string key)
{
    public RegionItemType Type { get; set; } = type;
    public string Key { get; set; } = key;

}

public class Region()
{
    public string Key { get; set; } = "";
    public string Desc { get; set; } = "";

    public string Group { get; set; } = "";
    public List<RegionItem> Items { get; set; } = [];
    public DateTime Updated { get; set; }

}