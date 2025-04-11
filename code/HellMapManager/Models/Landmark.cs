using HellMapManager.Utils.Formatter;


namespace HellMapManager.Models;

public class Landmark
{
    public Landmark() { }
    public string Key { get; set; } = "";
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
    public string Desc { get; set; } = "";
    public bool Validated()
    {
        return Key != "";
    }
    public const string EncodeKey = "Landmark";
    public string Encode()
    {
        return HMMFormatter.EncodeKeyValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Type),//1
                HMMFormatter.Escape(Value),//2
                HMMFormatter.Escape(Desc),//3
            ])
        );
    }
    public static Landmark Decode(string val)
    {
        var result = new Landmark();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Type = HMMFormatter.UnescapeAt(list, 1);
        result.Value = HMMFormatter.UnescapeAt(list, 2);
        result.Desc = HMMFormatter.UnescapeAt(list, 3);
        return result;
    }
}