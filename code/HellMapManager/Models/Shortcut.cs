using HellMapManager.Utils.Formatter;
using System.Collections.Generic;
namespace HellMapManager.Models;

public partial class Shortcut : Exit
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public List<string> RoomTags { get; set; } = [];
    public List<string> RoomExTags { get; set; } = [];
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
                HMMFormatter.EncodeList2(RoomTags.ConvertAll(HMMFormatter.Escape)),//3
                HMMFormatter.EncodeList2(RoomExTags.ConvertAll(HMMFormatter.Escape)),//4
                HMMFormatter.Escape(Command),//5
                HMMFormatter.Escape(To),//6
                HMMFormatter.EncodeList2(Tags.ConvertAll(HMMFormatter.Escape)),//7
                HMMFormatter.EncodeList2(ExTags.ConvertAll(HMMFormatter.Escape)),//8
                HMMFormatter.Escape(HMMFormatter.Escape(Cost.ToString())),//9
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
        result.RoomTags = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(HMMFormatter.Unescape);
        result.RoomExTags = HMMFormatter.DecodeList2(HMMFormatter.At(list, 4)).ConvertAll(HMMFormatter.Unescape);
        result.Command = HMMFormatter.UnescapeAt(list, 5);
        result.To = HMMFormatter.UnescapeAt(list, 6);
        result.Tags = HMMFormatter.DecodeList2(HMMFormatter.At(list, 7)).ConvertAll(HMMFormatter.Unescape);
        result.ExTags = HMMFormatter.DecodeList2(HMMFormatter.At(list, 8)).ConvertAll(HMMFormatter.Unescape);
        result.Cost = HMMFormatter.UnescapeIntAt(list, 9, 0);
        return result;
    }
}