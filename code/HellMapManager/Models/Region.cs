

using System.Collections.Generic;

namespace HellMapManager.Models;

public enum RegionItemType
{
    Room,
    Zone,
}
public class RegionItem(RegionItemType type, Condition value)
{
    public RegionItemType Type { get; set; } = type;
    public Condition Value { get; set; } = value;
    public bool Validated()
    {
        return Value.Key != "";
    }

}

public class Region
{

    public string Key { get; set; } = "";

    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public List<RegionItem> Items { get; set; } = [];
    public bool Validated()
    {
        return Key != "";
    }
    public const string EncodeKey = "Region";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList2(Items.ConvertAll(d=>HMMFormatter.EncodeKeyAndValue2(HMMFormatter.Escape(d.Type==RegionItemType.Zone?"Zone":"Room"),HMMFormatter.EncodeToggleValue(ToggleValue.FromCondition(d.Value))))),//3
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
        result.Items = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(d =>
        {
            var kv = HMMFormatter.DecodeKeyValue2(d);
            return new RegionItem(kv.UnescapeKey() == "Zone" ? RegionItemType.Zone : RegionItemType.Room, HMMFormatter.DecodeToggleValue(kv.Value).ToCondition());
        });
        return result;
    }

}