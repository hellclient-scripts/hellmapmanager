



using System.Collections.Generic;

namespace HellMapManager.Models;

public partial class Marker
{
    public Marker() { }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Group { get; set; } = "";
    public string Message { get; set; } = "";
    public bool Validated()
    {
        return ItemKey.Validate(Key) && Value != "";
    }
    public Marker Clone()
    {
        return new Marker
        {
            Key = Key,
            Value = Value,
            Desc = Desc,
            Group = Group,
            Message = Message,
        };
    }
    public const string EncodeKey = "Marker";
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue(HMMFormatter.Level1, EncodeKey,
            HMMFormatter.EncodeList(HMMFormatter.Level1, [
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Value),//1
                HMMFormatter.Escape(Group),//2
                HMMFormatter.Escape(Desc),//3
                HMMFormatter.Escape(Message),//4

            ])
        );
    }
    public static Marker Decode(string val)
    {
        var result = new Marker();
        var kv = HMMFormatter.DecodeKeyValue(HMMFormatter.Level1, val);
        var list = HMMFormatter.DecodeList(HMMFormatter.Level1, kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Value = HMMFormatter.UnescapeAt(list, 1);
        result.Group = HMMFormatter.UnescapeAt(list, 2);
        result.Desc = HMMFormatter.UnescapeAt(list, 3);
        result.Message = HMMFormatter.UnescapeAt(list, 4);
        return result;
    }
    public static void Sort(List<Marker> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
    }
}
public partial class Marker
{
    public bool Filter(string val)
    {
        if (Key.Contains(val) ||
            Value.Contains(val) ||
            Desc.Contains(val) ||
            Group.Contains(val) ||
            Message.Contains(val)
            )
        {
            return true;
        }
        return false;
    }
    public void Arrange()
    { }
    public bool Equal(Marker model)
    {
        return Key == model.Key && Value == model.Value && Desc == model.Desc && Group == model.Group && Message == model.Message;
    }
}