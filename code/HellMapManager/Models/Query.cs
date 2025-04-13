

using System.Collections.Generic;

namespace HellMapManager.Models;


public class Query
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public List<TypedCondition> Items { get; set; } = [];

    public bool Validated()
    {
        return Key != "";
    }
    public const string EncodeKey = "Query";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList2(Items.ConvertAll(d=>HMMFormatter.EncodeToggleKV3(ToggleKV.FromTypedCondition(d)))),//3
            ])
        );
    }
    public static Query Decode(string val)
    {
        var result = new Query();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Group = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.Items = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(d => HMMFormatter.DecodeToggleKV3(d).ToTypedCondition());
        return result;
    }

}