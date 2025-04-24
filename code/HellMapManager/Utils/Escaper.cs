using System.Collections.Generic;
using System.Net;
using HellMapManager.Models;

namespace HellMapManager.Utils;

public class Escaper
{
    public const string Token = "\\";
    public const string EscapedToken = "\\\\";
    private class _internalItem(string unescaped, string escaped)
    {
        public string Placeholder = EscapeToPlaceHolder(escaped);
        public string Unescaped = unescaped;
        public string Escaped = escaped;
    }
    private List<_internalItem> _items = [];
    private const string _internalToken = "$";
    private const string _placeholderInternal = "$0";
    private const string _placeholderToken = "$1";
    private static string EscapeToPlaceHolder(string str)
    {
        return str.Replace(_internalToken, _placeholderInternal).Replace(EscapedToken, _placeholderToken);
    }
    private static string UnescapeFromPlaceHolder(string str)
    {
        return str.Replace(_placeholderInternal, _internalToken).Replace(_placeholderToken, Token);
    }
    public Escaper WithItem(string unescaped, string escaped)
    {
        _items.Add(new _internalItem(unescaped, escaped));
        return this;
    }
    public string Escape(string str)
    {
        str = str.Replace(Token, EscapedToken);
        foreach (var item in _items)
        {
            str = str.Replace(item.Unescaped, item.Escaped);
        }
        return str;
    }
    public string Unescape(string str)
    {
        str = EscapeToPlaceHolder(str);
        foreach (var item in _items)
        {
            str = str.Replace(item.Escaped, item.Placeholder);
        }
        str = str.Replace(Token, "");
        str = UnescapeFromPlaceHolder(str);
        return str;
    }
}