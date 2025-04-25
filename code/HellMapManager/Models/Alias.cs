



namespace HellMapManager.Models;

public partial class Alias
{
    public Alias() { }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Group { get; set; } = "";
    public string Message { get; set; } = "";
    public bool Validated()
    {
        return Key != "" && Value != "";
    }
    public Alias Clone()
    {
        return new Alias
        {
            Key = Key,
            Value = Value,
            Desc = Desc,
            Group = Group,
            Message = Message,
        };
    }
    public const string EncodeKey = "Alias";
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Value),//1
                HMMFormatter.Escape(Group),//2
                HMMFormatter.Escape(Desc),//3
                HMMFormatter.Escape(Message),//4

            ])
        );
    }
    public static Alias Decode(string val)
    {
        var result = new Alias();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Value = HMMFormatter.UnescapeAt(list, 1);
        result.Group = HMMFormatter.UnescapeAt(list, 2);
        result.Desc = HMMFormatter.UnescapeAt(list, 3);
        result.Message = HMMFormatter.UnescapeAt(list, 4);
        return result;
    }
}
public partial class Alias
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

}