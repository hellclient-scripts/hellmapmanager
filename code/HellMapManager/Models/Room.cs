using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using HellMapManager.Utils.Formatter;
namespace HellMapManager.Models;


public class RoomData(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
    public bool Validated()
    {
        return Key != "" && Value != "";
    }
    public RoomData Clone()
    {
        return new RoomData(Key, Value);
    }
}

//房间的数据结构
public partial class Room
{
    public string Key { get; set; } = "";
    //房间的名称，显示用
    public string Name { get; set; } = "";
    //房间的描述，显示用
    public string Desc { get; set; } = "";
    //房间的区域，筛选用
    public string Group { get; set; } = "";
    //标签列表，筛选用
    public List<string> Tags = [];
    //房间出口列表
    public List<Exit> Exits { get; set; } = [];
    public List<RoomData> Data { get; set; } = [];
    public bool Validated()
    {
        return Key != "";
    }
    public Room Clone()
    {
        return new Room()
        {
            Key = Key,
            Name = Name,
            Desc = Desc,
            Group = Group,
            Tags = Tags.GetRange(0, Tags.Count),
            Data = Data.ConvertAll(d => d.Clone()),
        };
    }
}
public partial class Room
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Room))]
    public Room()
    {
    }
    //房间的key,必须唯一，不能为空
    public int ExitsCount
    {
        get => Exits.Count;
    }
    public string AllTags
    {
        get => String.Join(" , ", this.Tags.ToArray());
    }
    public bool HasExitTo(string key)
    {
        foreach (var exit in Exits)
        {
            if (exit.To == key)
            {
                return true;
            }
        }
        return false;
    }
    public void Sort()
    {
        Data.Sort((x, y) => x.Key.CompareTo(y.Key));
    }
    private void AddData(RoomData rd)
    {
        Data.RemoveAll((d) => d.Key == rd.Key);
        if (rd.Value != "")
        {
            Data.Add(rd);
        }
    }
    public void SetDatas(List<RoomData> list)
    {
        foreach (var rd in list)
        {
            AddData(rd);
        }
        Data.Sort((x, y) => x.Key.CompareTo(y.Key));
    }
}

