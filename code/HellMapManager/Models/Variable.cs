namespace HellMapManager.Models;


public class Variable
{
    public Variable() { }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public bool Validated()
    {
        return Key != "";
    }
    public const string EncodeKey = "Variable";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Value),//1
                HMMFormatter.Escape(Group),//2
                HMMFormatter.Escape(Desc),//3
            ])
        );
    }
    public static Variable Decode(string val)
    {
        var result = new Variable();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Value = HMMFormatter.UnescapeAt(list, 1);
        result.Group = HMMFormatter.UnescapeAt(list, 2);
        result.Desc = HMMFormatter.UnescapeAt(list, 3);
        return result;
    }
}