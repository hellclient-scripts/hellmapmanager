using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public RegionItem ToRegionItem()
    {
        return new RegionItem(HMMFormatter.Unescape(Key) == "Room" ? RegionItemType.Room : RegionItemType.Zone, HMMFormatter.Unescape(Value), Not);
    }
    public static ToggleKeyValue FromRegionItem(RegionItem i)
    {
        return new ToggleKeyValue(HMMFormatter.Escape(i.Type == RegionItemType.Room ? "Room" : "Zone"), HMMFormatter.Escape(i.Value), i.Not);
    }

}
public class ToggleKeyValues(string key, List<string> values, bool not)
{
    public bool Not = not;
    public string Key = key;
    public List<string> Values = values;
    public string NotLabel { get => Not ? HMMFormatter.TokenNot.Unescaped : ""; }
    public TypedConditions ToTypedConditions()
    {
        return new TypedConditions(HMMFormatter.Unescape(Key), Values.ConvertAll(HMMFormatter.Unescape), Not);
    }
    public static ToggleKeyValues FromTypedConditions(TypedConditions c)
    {
        return new ToggleKeyValues(HMMFormatter.Escape(c.Key), c.Conditions.ConvertAll(HMMFormatter.Escape), c.Not);
    }
}

public class Token(string unesacped, string escaped)
{
    public string Escaped { get; set; } = escaped;
    public string Unescaped { get; set; } = unesacped;
}

//四层简单结构格式化工具
//只支持列表和键值对列表，最多支持3层
public class HMMFormatter
{
    public static Token TokenEscape { get; } = new("%", "%25");
    public static Token TokenKey1 { get; } = new(">", "%3E");
    public static Token TokenKey2 { get; } = new(":", "%3A");
    public static Token TokenKey3 { get; } = new("=", "%3D");
    public static Token TokenKey4 { get; } = new("@", "%40");
    public static Token TokenSep1 { get; } = new("|", "%7C");
    public static Token TokenSep2 { get; } = new(";", "%3B");
    public static Token TokenSep3 { get; } = new(",", "%2C");
    public static Token TokenSep4 { get; } = new("&", "%26");
    public static Token TokenNot { get; } = new("!", "%21");
    public static Token TokenNewline { get; } = new("\n", "%0A");
    public static string Escape(string val)
    {
        return new StringBuilder(val)
        .Replace(TokenEscape.Unescaped, TokenEscape.Escaped)
        .Replace(TokenKey1.Unescaped, TokenKey1.Escaped)
        .Replace(TokenKey2.Unescaped, TokenKey2.Escaped)
        .Replace(TokenKey3.Unescaped, TokenKey3.Escaped)
        .Replace(TokenKey4.Unescaped, TokenKey4.Escaped)
        .Replace(TokenSep1.Unescaped, TokenSep1.Escaped)
        .Replace(TokenSep2.Unescaped, TokenSep2.Escaped)
        .Replace(TokenSep3.Unescaped, TokenSep3.Escaped)
        .Replace(TokenSep4.Unescaped, TokenSep4.Escaped)
        .Replace(TokenNot.Unescaped, TokenNot.Escaped)
        .Replace(TokenNewline.Unescaped, TokenNewline.Escaped)
        .ToString();
    }
    public static string Unescape(string val)
    {
        return new StringBuilder(val)
        .Replace(TokenNewline.Escaped, TokenNewline.Unescaped)
        .Replace(TokenNot.Escaped, TokenNot.Unescaped)
        .Replace(TokenSep4.Escaped, TokenSep4.Unescaped)
        .Replace(TokenSep3.Escaped, TokenSep3.Unescaped)
        .Replace(TokenSep2.Escaped, TokenSep2.Unescaped)
        .Replace(TokenSep1.Escaped, TokenSep1.Unescaped)
        .Replace(TokenKey4.Escaped, TokenKey4.Unescaped)
        .Replace(TokenKey3.Escaped, TokenKey3.Unescaped)
        .Replace(TokenKey2.Escaped, TokenKey2.Unescaped)
        .Replace(TokenKey1.Escaped, TokenKey1.Unescaped)
        .Replace(TokenEscape.Escaped, TokenEscape.Unescaped)
        .ToString();
    }
    public static string EncodeKeyAndValue1(string key, string val)
    {
        return EncodeKeyValue1(new KeyValue(key, val));
    }

    public static string EncodeKeyValue1(KeyValue kv)
    {
        return $"{kv.Key}{TokenKey1.Unescaped}{kv.Value}";
    }

    public static KeyValue DecodeKeyValue1(string val)
    {
        var decoded = val.Split(TokenKey1.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeKeyAndValue2(string key, string val)
    {
        return EncodeKeyValue2(new KeyValue(key, val));
    }
    public static string EncodeKeyValue2(KeyValue kv)
    {
        return $"{kv.Key}{TokenKey2.Unescaped}{kv.Value}";
    }

    public static KeyValue DecodeKeyValue2(string val)
    {
        var decoded = val.Split(TokenKey2.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeKeyAndValue3(string key, string val)
    {
        return EncodeKeyValue3(new KeyValue(key, val));
    }
    public static string EncodeKeyValue3(KeyValue kv)
    {
        return $"{kv.Key}{TokenKey3.Unescaped}{kv.Value}";
    }
    public static KeyValue DecodeKeyValue3(string val)
    {
        var decoded = val.Split(TokenKey3.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeKeyAndValue4(string key, string val)
    {
        return EncodeKeyValue4(new KeyValue(key, val));
    }
    public static string EncodeKeyValue4(KeyValue kv)
    {
        return $"{kv.Key}{TokenKey4.Unescaped}{kv.Value}";
    }
    public static KeyValue DecodeKeyValue4(string val)
    {
        var decoded = val.Split(TokenKey4.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeToggleKeyValue1(ToggleKeyValue kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue1(kv.Key, kv.Value), kv.Not));
    }
    public static ToggleKeyValue DecodeToggleKeyValue1(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue1(v.Value);
        return new ToggleKeyValue(kv.Key, kv.Value, v.Not);
    }
    public static string EncodeToggleKeyValue2(ToggleKeyValue kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue2(kv.Key, kv.Value), kv.Not));
    }
    public static ToggleKeyValue DecodeToggleKeyValue2(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue2(v.Value);
        return new ToggleKeyValue(kv.Key, kv.Value, v.Not);
    }
    public static string EncodeToggleKeyValue3(ToggleKeyValue kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue3(kv.Key, kv.Value), kv.Not));
    }
    public static ToggleKeyValue DecodeToggleKeyValue3(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue3(v.Value);
        return new ToggleKeyValue(kv.Key, kv.Value, v.Not);
    }
    public static string EncodeToggleKeyValue4(ToggleKeyValue kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue4(kv.Key, kv.Value), kv.Not));
    }
    public static ToggleKeyValue DecodeToggleKeyValue4(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue4(v.Value);
        return new ToggleKeyValue(kv.Key, kv.Value, v.Not);
    }

    public static string EncodeToggleKeyValues1(ToggleKeyValues kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue1(kv.Key, EncodeList1(kv.Values)), kv.Not));
    }
    public static ToggleKeyValues DecodeToggleKeyValues1(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue1(v.Value);
        return new ToggleKeyValues(kv.Key, DecodeList1(kv.Value), v.Not);
    }
    public static string EncodeToggleKeyValues2(ToggleKeyValues kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue2(kv.Key, EncodeList2(kv.Values)), kv.Not));
    }
    public static ToggleKeyValues DecodeToggleKeyValues2(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue2(v.Value);
        return new ToggleKeyValues(kv.Key, DecodeList2(kv.Value), v.Not);
    }
    public static string EncodeToggleKeyValues3(ToggleKeyValues kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue3(kv.Key, EncodeList3(kv.Values)), kv.Not));
    }
    public static ToggleKeyValues DecodeToggleKeyValues3(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue3(v.Value);
        return new ToggleKeyValues(kv.Key, DecodeList3(kv.Value), v.Not);
    }
    public static string EncodeToggleKeyValues4(ToggleKeyValues kv)
    {
        return EncodeToggleValue(new ToggleValue(EncodeKeyAndValue4(kv.Key, EncodeList4(kv.Values)), kv.Not));
    }
    public static ToggleKeyValues DecodeToggleKeyValues4(string val)
    {
        var v = DecodeToggleValue(val);
        var kv = DecodeKeyValue4(v.Value);
        return new ToggleKeyValues(kv.Key, DecodeList4(kv.Value), v.Not);
    }

    public static string EncodeList1(List<string> items)
    {
        return string.Join(TokenSep1.Unescaped, items);
    }
    public static List<string> DecodeList1(string val)
    {
        if (val == "")
        {
            return [];
        }

        return [.. val.Split(TokenSep1.Unescaped)];
    }
    public static string EncodeList2(List<string> items)
    {

        return string.Join(TokenSep2.Unescaped, items);
    }
    public static List<string> DecodeList2(string val)
    {
        if (val == "")
        {
            return [];
        }
        return [.. val.Split(TokenSep2.Unescaped)];
    }
    public static string EncodeList3(List<string> items)
    {
        return string.Join(TokenSep3.Unescaped, items);
    }
    public static List<string> DecodeList3(string val)
    {
        if (val == "")
        {
            return [];
        }

        return [.. val.Split(TokenSep3.Unescaped)];
    }
    public static string EncodeList4(List<string> items)
    {
        return string.Join(TokenSep4.Unescaped, items);
    }
    public static List<string> DecodeList4(string val)
    {
        if (val == "")
        {
            return [];
        }
        return [.. val.Split(TokenSep4.Unescaped)];
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
        try
        {
            return int.Parse(Unescape(val));
        }
        catch
        {
            return defaultValue;
        }
    }
    public static int UnescapeIntAt(List<string> list, int index, int defaultValue)
    {
        return UnescapeInt(At(list, index), defaultValue);
    }
    public static string EncodeToggleValue(ToggleValue v)
    {
        return (v.Not ? TokenNot.Unescaped : "") + v.Value;
    }
    public static ToggleValue DecodeToggleValue(string val)
    {
        var not = val.Length > 0 && val.StartsWith(TokenNot.Unescaped);
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
}
