using System;
using System.Collections.Generic;

namespace HellMapManager.Models;


public class RoomFilter
{
    public bool Validate(Room room)
    {
        //TODO
        return true;
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
    public List<Data> Data { get; set; } = [];
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
    public Room Clone()
    {
        return new Room()
        {
            Key = Key,
            Name = Name,
            Group = Group,
            Desc = Desc,
            Tags = [.. Tags],
            Exits = Exits.ConvertAll(e => e.Clone()),
            Data = Data.ConvertAll(d => d.Clone()),
        };
    }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Name),//1
                HMMFormatter.Escape(Group),//2
                HMMFormatter.Escape(Desc),//3
                HMMFormatter.EncodeList2(Tags.ConvertAll(HMMFormatter.Escape)),//4
                HMMFormatter.EncodeList2(Exits.ConvertAll(//5
                    e=>HMMFormatter.EncodeList3([
                        HMMFormatter.Escape(e.Command),//5-0
                        HMMFormatter.Escape(e.To),//5-1
                    HMMFormatter.EncodeList4(e.Conditions.ConvertAll(c=>HMMFormatter.EncodeToggleValue(ToggleValue.FromCondition(c)))),//5-2
                    HMMFormatter.Escape(HMMFormatter.Escape(e.Cost.ToString())),//5-4
                    ])
                )),
                HMMFormatter.EncodeList2(//6
                    Data.ConvertAll(
                        d=>HMMFormatter.EncodeKeyValue3(KeyValue.FromData(d))
                        )
                    ),
             ])
        );
    }
    public static void Sort(List<Room> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));

    }
    public static Room Decode(string val)
    {
        var result = new Room();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Name = HMMFormatter.UnescapeAt(list, 1);
        result.Group = HMMFormatter.UnescapeAt(list, 2);
        result.Desc = HMMFormatter.UnescapeAt(list, 3);
        result.Tags = HMMFormatter.DecodeList2(HMMFormatter.At(list, 4)).ConvertAll(HMMFormatter.Unescape);
        result.Exits = HMMFormatter.DecodeList2(HMMFormatter.At(list, 5)).ConvertAll(d =>
        {
            var list = HMMFormatter.DecodeList3(d);
            return new Exit()
            {
                Command = HMMFormatter.UnescapeAt(list, 0),
                To = HMMFormatter.UnescapeAt(list, 1),
                Conditions = HMMFormatter.DecodeList4(HMMFormatter.At(list, 2)).ConvertAll(d => HMMFormatter.DecodeToggleValue(d).ToCondition()),
                Cost = HMMFormatter.UnescapeInt(HMMFormatter.At(list, 3), 0),
            };
        });
        result.Data = HMMFormatter.DecodeList2(HMMFormatter.At(list, 6)).ConvertAll(
            d => HMMFormatter.DecodeKeyValue3(d).ToData());
        return result;
    }
}
public partial class Room
{
    //房间的key,必须唯一，不能为空
    public int ExitsCount
    {
        get => Exits.Count;
    }
    public string AllTags
    {
        get => String.Join(",", Tags.ToArray());
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
    private void DoAddData(Data rd)
    {
        Data.RemoveAll((d) => d.Key == rd.Key);
        if (rd.Value != "")
        {
            Data.Add(rd);
        }
    }
    public void SetData(Data rd)
    {
        DoAddData(rd);
        Arrange();
    }
    public void SetDatas(List<Data> list)
    {
        foreach (var rd in list)
        {
            DoAddData(rd);
        }
        Arrange();
    }

    public void Arrange()
    {
        Data.Sort((x, y) => x.Key.CompareTo(y.Key));
        Tags.Sort((x, y) => x.CompareTo(y));
        Exits.ForEach(e => e.Arrange());
    }
    public bool Filter(string val)
    {
        if (Key.Contains(val) ||
            Name.Contains(val) ||
            Desc.Contains(val) ||
            Group.Contains(val))
        {
            return true;
        }
        foreach (var tag in Tags)
        {
            if (tag.Contains(val))
            {
                return true;
            }
        }
        foreach (var data in Data)
        {
            if (data.Key.Contains(val) || data.Value.Contains(val))
            {
                return true;
            }
        }
        foreach (var exit in Exits)
        {
            if (exit.Command.Contains(val) || exit.To.Contains(val))
            {
                return true;
            }
        }
        return false;
    }

    public bool Equal(Room model)
    {
        if (
            Key != model.Key ||
            Name != model.Name ||
            Group != model.Group ||
            Desc != model.Desc
            )
        {
            return false;
        }
        if (Exits.Count != model.Exits.Count)
        {
            return false;
        }
        for (var i = 0; i < Exits.Count; i++)
        {
            if (!Exits[i].Equal(model.Exits[i]))
            {
                return false;
            }
        }
        if (Tags.Count != model.Tags.Count)
        {
            return false;
        }
        for (var i = 0; i < Tags.Count; i++)
        {
            if (Tags[i] != model.Tags[i])
            {
                return false;
            }
        }
        if (Data.Count != model.Data.Count)
        {
            return false;
        }
        for (var i = 0; i < Data.Count; i++)
        {
            if (!Data[i].Equal(model.Data[i]))
            {
                return false;
            }
        }
        return true;
    }

    public int NumberID
    {
        get
        {
            if (int.TryParse(Key.Trim(), out int result))
            {
                return result;
            }
            return -1;
        }
    }
}

