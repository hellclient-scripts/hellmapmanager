

using System.Collections.Generic;

namespace HellMapManager.Models;

public enum RegionItemType
{
    Room,
    Zone,
}
public class RegionItem(RegionItemType type, string value, bool not)
{
    public bool Not { get; set; } = not;
    public RegionItemType Type { get; set; } = type;
    public string Value { get; set; } = value;
    public bool Validated()
    {
        return Value != "";
    }
    public bool IsRoom
    {

        get => Type == RegionItemType.Room;
    }
    public string ExcludeLabel
    {
        get => Not ? "-" : "+";
    }
    public string Label
    {
        get => (Not ? "排除" : "加入") + (Type == RegionItemType.Room ? "房间" : "区域") + " " + Value;
    }
    public RegionItem Clone()
    {
        return new RegionItem(Type, Value, Not);
    }
    public bool Equal(RegionItem model)
    {
        if (Type != model.Type) return false;
        if (Value != model.Value) return false;
        if (Not != model.Not) return false;
        return true;
    }
}

public partial class Region
{

    public string Key { get; set; } = "";

    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public string Message { get; set; } = "";
    public List<RegionItem> Items { get; set; } = [];
    public bool Validated()
    {
        return Key != "";
    }
    public Region Clone()
    {
        return new Region()
        {
            Key = Key,
            Group = Group,
            Desc = Desc,
            Items = Items.ConvertAll(d => d.Clone()),
            Message = Message,
        };
    }
    public const string EncodeKey = "Region";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList2(Items.ConvertAll(d=>HMMFormatter.EncodeToggleKeyValue2(ToggleKeyValue.FromRegionItem(d)))),//3
                HMMFormatter.Escape(Message),//4
            ])
        );
    }
    public static Region Decode(string val)
    {
        var result = new Region();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Group = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.Items = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(d => HMMFormatter.DecodeToggleKeyValue2(d).ToRegionItem());
        result.Message = HMMFormatter.UnescapeAt(list, 4);
        return result;
    }
    public void Arrange()
    {

    }
    public static void Sort(List<Region> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
    }
}

public partial class Region
{
    public bool Filter(string val)
    {
        if (Key.Contains(val) ||
            Desc.Contains(val) ||
            Group.Contains(val) ||
            Message.Contains(val))
        {
            return true;
        }
        foreach (var item in Items)
        {
            if (item.Value.Contains(val))
            {
                return true;
            }
        }
        return false;
    }
    public bool Equal(Region model)
    {
        if (Key != model.Key) return false;
        if (Group != model.Group) return false;
        if (Desc != model.Desc) return false;
        if (Message != model.Message) return false;
        if (Items.Count != model.Items.Count) return false;
        for (int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].Equal(model.Items[i])) return false;
        }
        return true;

    }
    public int ItemsCount
    {
        get => Items.Count;
    }
}