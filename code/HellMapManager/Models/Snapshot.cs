
using System.Collections.Generic;
namespace HellMapManager.Models;
public partial class Snapshot
{
    public string Key { get; set; } = "";
    public int Timestamp = 0;
    public string Group { get; set; } = "";
    public string Type { get; set; } = "";

    public string Value { get; set; } = "";
    public bool Validated()
    {
        return Key != "" && Timestamp > 0;
    }
    public const string EncodeKey = "Snapshot";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Timestamp.ToString()),//1
                HMMFormatter.Escape(Group),//2
                HMMFormatter.Escape(Type),//3
                                HMMFormatter.Escape(Value),//4
            ])
        );
    }
    public static Snapshot Decode(string val)
    {
        var result = new Snapshot();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Timestamp = HMMFormatter.UnescapeIntAt(list, 1, -1);
        result.Group = HMMFormatter.UnescapeAt(list, 2);
        result.Type = HMMFormatter.UnescapeAt(list, 3);
        result.Value = HMMFormatter.UnescapeAt(list, 4);
        return result;
    }

}