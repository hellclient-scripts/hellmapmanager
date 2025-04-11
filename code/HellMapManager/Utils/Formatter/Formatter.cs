using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HellMapManager.Utils.Formatter;


public class Condition(string key, bool not)
{
    public string Key { get; set; } = key;
    public bool Not { get; set; } = not;
}

public class Token(string unesacped, string escaped)
{
    public string Escaped { get; set; } = escaped;
    public string Unescaped { get; set; } = unesacped;
}

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
    public static string EncodeKeyValue1(string key, string value)
    {
        return $"{key}{TokenKey1.Unescaped}{value}";
    }
    public static KeyValue DecodeKeyValue1(string val)
    {
        var decoded = val.Split(TokenKey1.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeKeyValue2(string key, string value)
    {
        return $"{key}{TokenKey2.Unescaped}{value}";
    }

    public static KeyValue DecodeKeyValue2(string val)
    {
        var decoded = val.Split(TokenKey2.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeKeyValue3(string key, string value)
    {
        return $"{key}{TokenKey3.Unescaped}{value}";
    }
    public static KeyValue DecodeKeyValue3(string val)
    {
        var decoded = val.Split(TokenKey3.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeKeyValue4(string key, string value)
    {
        return $"{key}{TokenKey4.Unescaped}{value}";
    }
    public static KeyValue DecodeKeyValue4(string val)
    {
        var decoded = val.Split(TokenKey4.Unescaped, 2);
        return new KeyValue(decoded[0], decoded.Count() > 1 ? decoded[1] : "");
    }
    public static string EncodeList1(List<string> items)
    {
        return string.Join(TokenSep1.Unescaped, items);
    }
    public static List<string> DecodeList1(string val)
    {
        return [.. val.Split(TokenSep1.Unescaped)];
    }
    public static string EncodeList2(List<string> items)
    {
        return string.Join(TokenSep2.Unescaped, items);
    }
    public static List<string> DecodeList2(string val)
    {
        return [.. val.Split(TokenSep2.Unescaped)];
    }
    public static string EncodeList3(List<string> items)
    {
        return string.Join(TokenSep3.Unescaped, items);
    }
    public static List<string> DecodeList3(string val)
    {
        return [.. val.Split(TokenSep3.Unescaped)];
    }
    public static string EncodeList4(List<string> items)
    {
        return string.Join(TokenSep4.Unescaped, items);
    }
    public static List<string> DecodeList4(string val)
    {
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
    public static string EscapeCondition(Condition c)
    {
        return (c.Not ? "!" : "") + Escape(c.Key);
    }
    public static Condition UnescapeCondition(string val)
    {
        var not = val.Length > 0 && val.StartsWith('!');
        string key;
        if (not)
        {
            key = val.Substring(1);
        }
        else
        {
            key = val;
        }
        return new Condition(Escape(key), not);
    }
    public static Condition UnescapeConditionAt(List<string> list, int index)
    {
        return UnescapeCondition(At(list, index));
    }
}
