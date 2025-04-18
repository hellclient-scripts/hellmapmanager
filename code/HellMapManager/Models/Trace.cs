using System.Collections.Generic;
using System.Linq;



namespace HellMapManager.Models;

public partial class Trace
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public List<string> Locations { get; set; } = [];
    public Trace Clone()
    {
        return new Trace()
        {
            Key = Key,
            Locations = Locations.GetRange(0, Locations.Count),
        };

    }
    public bool Validated()
    {
        return Key != "";
    }
    public const string EncodeKey = "Trace";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList2(Locations.ConvertAll(HMMFormatter.Escape)),//3
            ])
        );
    }
    public static Trace Decode(string val)
    {
        var result = new Trace();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Group = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.Locations = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(HMMFormatter.Unescape);
        return result;
    }

}

public partial class Trace
{
    public void Arrange()
    {
        Locations.Distinct();
        Locations.Sort((x, y) => x.CompareTo(y));
    }
    public void RemoveLocations(List<string> loctions)
    {
        foreach (var l in loctions)
        {
            Locations.Remove(l);
        }
        Arrange();
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
            Group.Contains(val))
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
    public int LocationsCount
    {
        get => Locations.Count;
    }
}