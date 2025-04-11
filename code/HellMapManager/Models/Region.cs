using System;
using System.Collections.Generic;

namespace HellMapManager.Models;

public enum RegionItemType
{
    Room,
    Zone,
}
public class RegionItem(RegionItemType type, string value)
{
    public RegionItemType Type { get; set; } = type;
    public string Value { get; set; } = value;
    public bool Validated()
    {
        return Value != "";
    }

}

public class Region()
{

    public string Key { get; set; } = "";
    public string Desc { get; set; } = "";

    public string Group { get; set; } = "";
    public List<RegionItem> Items { get; set; } = [];
    public bool Validated()
    {
        return Key != "";
    }
}