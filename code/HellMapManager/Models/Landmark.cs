
using System.Collections.Generic;


using HellMapManager.Utils;

namespace HellMapManager.Models;

public class LandmarkKey(string key, string type)
{
    public string Key { get; set; } = key;
    public string Type { get; set; } = type;

    public override string ToString()
    {
        return UniqueKeyUtil.Join([Key, Type]);
    }
    public bool Equal(LandmarkKey obj)
    {
        return Key == obj.Key && Type == obj.Type;
    }
}
public partial class Landmark
{
    public Landmark() { }
    public string Key { get; set; } = "";
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
    public const string EncodeKey = "Landmark";
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey,
            HMMFormatter.EncodeList(HMMFormatter.Level1, [
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Type),//1
                HMMFormatter.Escape(Value),//2
                HMMFormatter.Escape(Group),//3
                HMMFormatter.Escape(Desc),//4
            ])
        );
    }
    public static Landmark Decode(string val)
    {
        var result = new Landmark();
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, val);
        var list = HMMFormatter.DecodeList(HMMFormatter.Level1, kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Type = HMMFormatter.UnescapeAt(list, 1);
        result.Value = HMMFormatter.UnescapeAt(list, 2);
        result.Group = HMMFormatter.UnescapeAt(list, 3);
        result.Desc = HMMFormatter.UnescapeAt(list, 4);
        return result;
    }
    public void Arrange()
    {

    }
    public Landmark Clone()
    {
        return new Landmark()
        {
            Key = Key,
            Type = Type,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }
    public static void Sort(List<Landmark> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : (x.Key != y.Key ? x.Key.CompareTo(y.Key) : x.Type.CompareTo(y.Type)));
    }
}

public partial class Landmark
{
    public bool Filter(string filter)
    {
        if (Key.Contains(filter) || Type.Contains(filter) || Value.Contains(filter) || Group.Contains(filter) || Desc.Contains(filter))
        {
            return true;
        }
        return false;
    }
    public bool Equal(Landmark model)
    {
        if (Key == model.Key && Type == model.Type && Value == model.Value && Group == model.Group && Desc == model.Desc)
        {
            return true;
        }
        return false;
    }
    public LandmarkKey UniqueKey()
    {
        return new(Key, Type);
    }
}