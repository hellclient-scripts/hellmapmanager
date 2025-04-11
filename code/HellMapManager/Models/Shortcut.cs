using HellMapManager.Utils.Formatter;
using System.Collections.Generic;
namespace HellMapManager.Models;

public partial class Shortcut : Exit
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public List<Condition> RoomConditions { get; set; } = [];
    public new bool Validated()
    {
        return Key != "" && base.Validated();
    }
    public const string EncodeKey = "Shortcut";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList2(RoomConditions.ConvertAll(HMMFormatter.EscapeCondition)),//3
                HMMFormatter.Escape(Command),//4
                HMMFormatter.Escape(To),//5
                HMMFormatter.EncodeList2(Conditions.ConvertAll(HMMFormatter.EscapeCondition)),//6
                HMMFormatter.Escape(HMMFormatter.Escape(Cost.ToString())),//7
            ])
        );
    }
    public static Shortcut Decode(string val)
    {
        var result = new Shortcut();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Group = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.RoomConditions = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(HMMFormatter.UnescapeCondition);
        result.Command = HMMFormatter.UnescapeAt(list, 4);
        result.To = HMMFormatter.UnescapeAt(list, 5);
        result.Conditions = HMMFormatter.DecodeList2(HMMFormatter.At(list, 6)).ConvertAll(HMMFormatter.UnescapeCondition);
        result.Cost = HMMFormatter.UnescapeIntAt(list, 7, 0);
        return result;
    }
}