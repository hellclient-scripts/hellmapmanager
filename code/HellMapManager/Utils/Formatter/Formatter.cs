using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HellMapManager.Utils.Formatter;
public class Token(string unesacped, string escaped)
{
    public string Escaped { get; set; } = escaped;
    public string Unescaped { get; set; } = unesacped;
}


public class KeyValue(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
}
public class HMMFormatter
{
    public static Token TokenEscape { get; } = new("%", "%25");
    public static Token TokenName { get; } = new(">", "%3E");
    public static Token TokenKey { get; } = new(":", "%3A");
    public static Token TokenSep { get; } = new("|", "%7C");
    public static Token TokenList { get; } = new(";", "%3B");
    public static Token TokenNewline { get; } = new("\n", "%0A");
    public static string Escape(string val)
    {
        return new StringBuilder(val)
        .Replace(TokenEscape.Unescaped, TokenEscape.Escaped)
        .Replace(TokenName.Unescaped, TokenName.Escaped)
        .Replace(TokenKey.Unescaped, TokenKey.Escaped)
        .Replace(TokenSep.Unescaped, TokenSep.Escaped)
        .Replace(TokenList.Unescaped, TokenList.Escaped)
        .Replace(TokenNewline.Unescaped, TokenNewline.Escaped)
        .ToString();
    }
    public static string Unescape(string val)
    {
        return new StringBuilder(val)
        .Replace(TokenNewline.Escaped, TokenNewline.Unescaped)
        .Replace(TokenList.Escaped, TokenList.Unescaped)
        .Replace(TokenSep.Escaped, TokenSep.Unescaped)
        .Replace(TokenKey.Escaped, TokenKey.Unescaped)
        .Replace(TokenName.Escaped, TokenName.Unescaped)
        .Replace(TokenEscape.Escaped, TokenEscape.Unescaped)
        .ToString();
    }
    public static string EncodeKeyValue(string key, string value)
    {
        return $"{key}{TokenKey}{value}";
    }
    public static string EncodeList(List<string> items)
    {
        return string.Join(TokenList.Unescaped, items);
    }
}
