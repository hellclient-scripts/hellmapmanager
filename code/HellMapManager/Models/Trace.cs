using System.Collections.Generic;

namespace HellMapManager.Models;

public partial class Trace
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Message { get; set; } = "";

    public List<string> Locations { get; set; } = [];
    public Trace Clone()
    {
        return new Trace()
        {
            Key = Key,
            Locations = [.. Locations],
            Desc = Desc,
            Group = Group,
            Message = Message,
        };

    }
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
    public const string EncodeKey = "Trace";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey,
            HMMFormatter.EncodeList(HMMFormatter.Level1, [
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList(HMMFormatter.Level2,Locations.ConvertAll(HMMFormatter.Escape)),//3
                HMMFormatter.Escape(Message),//4
            ])
        );
    }
    public static Trace Decode(string val)
    {
        var result = new Trace();
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, val);
        var list = HMMFormatter.DecodeList(HMMFormatter.Level1, kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Group = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.Locations = HMMFormatter.DecodeList(HMMFormatter.Level2, HMMFormatter.At(list, 3)).ConvertAll(HMMFormatter.Unescape);
        result.Message = HMMFormatter.UnescapeAt(list, 4);
        return result;
    }
    public static void Sort(List<Trace> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
    }
}

public partial class Trace
{
    public void Arrange()
    {
        Locations.Sort((x, y) => x.CompareTo(y));
    }
    public void RemoveLocations(List<string> loctions)
    {
        foreach (var l in loctions)
        {
            Locations.Remove(l);
        }
    }
    public void AddLocations(List<string> loctions)
    {
        foreach (var l in loctions)
        {
            Locations.Remove(l);
            Locations.Add(l);
        }
        Arrange();
    }
    public bool Filter(string val)
    {
        if (Key.Contains(val) ||
            Desc.Contains(val) ||
            Group.Contains(val) ||
            Message.Contains(val))
        {
            return true;
        }
        foreach (var room in Locations)
        {
            if (room.Contains(val))
            {
                return true;
            }
        }
        return false;
    }
    public bool Equal(Trace model)
    {
        if (Key != model.Key ||
        Desc != model.Desc ||
        Group != model.Group ||
        Message != model.Message)
        {
            return false;
        }
        if (Locations.Count != model.Locations.Count)
        {
            return false;
        }
        for (var i = 0; i < Locations.Count; i++)
        {
            if (Locations[i] != model.Locations[i])
            {
                return false;
            }
        }
        return true;
    }
    public int LocationsCount
    {
        get => Locations.Count;
    }
}