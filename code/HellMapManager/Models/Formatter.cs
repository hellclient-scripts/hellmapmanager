using System.Collections.Generic;
using HellMapManager.Utils.ControlCode;
namespace HellMapManager.Models;

public class KeyValue(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
    public string UnescapeKey()
    {
        return HMMFormatter.Unescape(Key);
    }
    public string UnescapeValue()
    {
        return HMMFormatter.Unescape(Value);
    }
    public Data ToData()
    {
        return new Data(HMMFormatter.Unescape(Key), HMMFormatter.Unescape(Value));
    }
    public static KeyValue FromData(Data k)
    {
        return new KeyValue(HMMFormatter.Escape(k.Key), HMMFormatter.Escape(k.Value));
    }
    public ValueTag ToValueTag()
    {
        return new ValueTag(HMMFormatter.Unescape(Key), HMMFormatter.UnescapeInt(Value, 0));
    }
    public static KeyValue FromValueTag(ValueTag k)
    {
        return new KeyValue(HMMFormatter.Escape(k.Key), k.Value.ToString());
    }

}

public class ToggleValue(string value, bool not)
{
    public bool Not { get; set; } = not;
    public string Value { get; set; } = value;
    public string UnescapeValue()
    {
        return HMMFormatter.Unescape(Value);
    }
    public Condition ToCondition()
    {
        return new Condition(HMMFormatter.Unescape(Value), Not);
    }
    public static ToggleValue FromCondition(Condition c)
    {
        return new ToggleValue(HMMFormatter.Escape(c.Key), c.Not);
    }
}
public class ToggleKeyValue(string key, string value, bool not)
{
    public bool Not = not;
    public string Key = key;
    public string Value = value;
    public string UnescapeKey()
    {
        return HMMFormatter.Unescape(Key);
    }
    public string UnescapeValue()
    {
        return HMMFormatter.Unescape(Value);
    }

    public RegionItem ToRegionItem()
    {
        return new RegionItem(HMMFormatter.Unescape(Key) == "Room" ? RegionItemType.Room : RegionItemType.Zone, HMMFormatter.Unescape(Value), Not);
    }
    public static ToggleKeyValue FromRegionItem(RegionItem i)
    {
        return new ToggleKeyValue(HMMFormatter.Escape(i.Type == RegionItemType.Room ? "Room" : "Zone"), HMMFormatter.Escape(i.Value), i.Not);
    }
    public ValueCondition ToValueCondition()
    {
        return new ValueCondition(HMMFormatter.Unescape(Key), HMMFormatter.UnescapeInt(Value, 0), Not);
    }
    public static ToggleKeyValue FromValueCondition(ValueCondition c)
    {
        return new ToggleKeyValue(HMMFormatter.Escape(c.Key), c.Value.ToString(), c.Not);
    }


}
public class ToggleKeyValues(string key, List<string> values, bool not)
{
    public bool Not = not;
    public string Key = key;
    public List<string> Values = values;
    public TypedConditions ToTypedConditions()
    {
        return new TypedConditions(HMMFormatter.Unescape(Key), Values.ConvertAll(HMMFormatter.Unescape), Not);
    }
    public static ToggleKeyValues FromTypedConditions(TypedConditions c)
    {
        return new ToggleKeyValues(HMMFormatter.Escape(c.Key), c.Conditions.ConvertAll(HMMFormatter.Escape), c.Not);
    }
}


public class HMMLevel(Command keyToken, Command sepToken)
{
    public Command KeyToken { get; set; } = keyToken;
    public Command SepToken { get; set; } = sepToken;
}
//五层简单结构格式化工具
//只支持列表和键值对列表，最多支持5层
public class HMMFormatter
{
    public static HMMLevel Level1 { get; } = new HMMLevel(new Command(">", "1", "\\>"), new Command("|", "6", "\\|"));
    public static HMMLevel Level2 { get; } = new HMMLevel(new Command(":", "2", "\\:"), new Command(";", "7", "\\;"));
    public static HMMLevel Level3 { get; } = new HMMLevel(new Command("=", "3", "\\="), new Command(",", "8", "\\,"));
    public static HMMLevel Level4 { get; } = new HMMLevel(new Command("@", "4", "\\@"), new Command("&", "9", "\\&"));
    public static HMMLevel Level5 { get; } = new HMMLevel(new Command("^", "5", "\\^"), new Command("`", "10", "\\`"));
    public static Command TokenNot { get; } = new Command("!", "11", "\\!");
    public static Command TokenNewline { get; } = new Command("\n", "12", "\\n");
    public static readonly ControlCode Escaper = (new ControlCode())
    .WithCommand(new Command("\\", "0", "\\\\"))
        .WithCommand(Level1.KeyToken)
        .WithCommand(Level1.SepToken)
        .WithCommand(Level2.KeyToken)
        .WithCommand(Level2.SepToken)
        .WithCommand(Level3.KeyToken)
        .WithCommand(Level3.SepToken)
        .WithCommand(Level4.KeyToken)
        .WithCommand(Level4.SepToken)
        .WithCommand(Level5.KeyToken)
        .WithCommand(Level5.SepToken)
        .WithCommand(TokenNot)
        .WithCommand(TokenNewline)
        .WithCommand(new Command("", "99", "\\"))
;

    public static string Escape(string val)
    {
        return Escaper.Encode(val);
    }
    public static string Unescape(string val)
    {
        return Escaper.Decode(val);
    }
    public static string EncodeKeyAndValue(HMMLevel level, string key, string val)
    {
        return EncodeKeyValue(level, new KeyValue(key, val));
    }

    public static string EncodeKeyValue(HMMLevel level, KeyValue kv)
    {
        return $"{kv.Key}{level.KeyToken.Raw}{kv.Value}";
    }

    public static KeyValue DecodeKeyValue(HMMLevel level, string val)
    {
        var decoded = val.Split(level.KeyToken.Raw, 2);
        return new KeyValue(decoded[0], decoded.Length > 1 ? decoded[1] : "");
    }
    public static string EncodeToggleKeyValue(HMMLevel level, ToggleKeyValue kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue(level, kv.Key, kv.Value), kv.Not));
    }
    public static ToggleKeyValue DecodeToggleKeyValue(HMMLevel level, string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue(level, v.Value);
        return new ToggleKeyValue(kv.Key, kv.Value, v.Not);
    }

    public static string EncodeToggleKeyValues(HMMLevel level, ToggleKeyValues kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue(level, kv.Key, EncodeList(level, kv.Values)), kv.Not));
    }
    public static ToggleKeyValues DecodeToggleKeyValues1(HMMLevel level, string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue(level, v.Value);
        return new ToggleKeyValues(kv.Key, DecodeList(level, kv.Value), v.Not);
    }
    public static string EncodeList(HMMLevel level, List<string> items)
    {
        return string.Join(level.SepToken.Raw, items);
    }
    public static List<string> DecodeList(HMMLevel level, string val)
    {
        if (val == "")
        {
            return [];
        }

        return [.. val.Split(level.SepToken.Raw)];
    }
    public static string At(List<string> list, int index)
    {
        return index >= 0 && index < list.Count ? list[index] : "";
    }
    public static string UnescapeAt(List<string> list, int index)
    {
        return Unescape(At(list, index));
    }
    public static int UnescapeInt(string val, int defaultValue)
    {
        if (!int.TryParse(Unescape(val), out var result))
        {
            return defaultValue;
        }
        return result;
    }
    public static int UnescapeIntAt(List<string> list, int index, int defaultValue)
    {
        return UnescapeInt(At(list, index), defaultValue);
    }
    public static string EncodeToggleValue(ToggleValue v)
    {
        return (v.Not ? TokenNot.Raw : "") + v.Value;
    }
    public static ToggleValue DecodeToggleValue(string val)
    {
        var not = val.Length > 0 && val.StartsWith(TokenNot.Raw);
        string key;
        if (not)
        {
            key = val.Substring(1);
        }
        else
        {
            key = val;
        }
        return new ToggleValue(key, not);

    }
    public static List<string> EscapeList(List<string> list)
    {
        return list.ConvertAll(Escape);
    }
    public static List<string> UnescapeList(List<string> list)
    {
        return list.ConvertAll(Unescape);
    }
}
