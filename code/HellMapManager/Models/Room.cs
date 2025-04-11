using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public const string EncodeKey = "Room";
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
            Exits = Exits.ConvertAll(e => e.Clone()),
            Data = Data.ConvertAll(d => d.Clone()),
        };
    }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Name),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.Escape(Group),//3
                HMMFormatter.EncodeList2(Tags.ConvertAll(HMMFormatter.Escape)),//4
                HMMFormatter.EncodeList2(Exits.ConvertAll(//5
                    e=>HMMFormatter.EncodeList3([
                        HMMFormatter.Escape(e.Command),//5-0
                        HMMFormatter.Escape(e.To),//5-1
                    HMMFormatter.EncodeList4(e.Tags.ConvertAll(HMMFormatter.Escape)),//5-2
                    HMMFormatter.EncodeList4(e.ExTags.ConvertAll(HMMFormatter.Escape)),//5-3
                    HMMFormatter.Escape(HMMFormatter.Escape(e.Cost.ToString())),//5-4
                    ])
                )),
                HMMFormatter.EncodeList2(//6
                    Data.ConvertAll(
                        d=>HMMFormatter.EncodeKeyValue3(HMMFormatter.Escape(d.Key),HMMFormatter.Escape(d.Value))
                        )
                    ),
             ])
        );
    }
    public static Room Decode(string val)
    {
        var result = new Room();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Name = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.Group = HMMFormatter.UnescapeAt(list, 3);
        result.Tags = HMMFormatter.DecodeList2(HMMFormatter.At(list, 4)).ConvertAll(HMMFormatter.Unescape);
        result.Exits = HMMFormatter.DecodeList2(HMMFormatter.At(list, 5)).ConvertAll(d =>
        {
            var list = HMMFormatter.DecodeList3(d);
            return new Exit()
            {
                Command = HMMFormatter.UnescapeAt(list, 0),
                To = HMMFormatter.UnescapeAt(list, 1),
                Tags = HMMFormatter.DecodeList4(HMMFormatter.At(list, 2)).ConvertAll(HMMFormatter.Unescape),
                ExTags = HMMFormatter.DecodeList4(HMMFormatter.At(list, 3)).ConvertAll(HMMFormatter.Unescape),
                Cost = HMMFormatter.UnescapeInt(HMMFormatter.At(list, 4), 0),
            };
        });
        result.Data = HMMFormatter.DecodeList2(HMMFormatter.At(list, 6)).ConvertAll(
            d =>
            {
                var kv = HMMFormatter.DecodeKeyValue3(d);
                return new RoomData(kv.UnescapeKey(), kv.UnescapeValue());
            }
        );
        return result;
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

