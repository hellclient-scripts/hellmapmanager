using System.Collections.Generic;
using System.Net;
using HellMapManager.Models;

namespace HellMapManager.Utils;

public class Escaper
{
    public Escaper()
    {
        _items = [new _internalItem("\\", "\\\\")];
    }
    public const string Token = "\\";
    public const string EscapedToken = "\\\\";
    private class _internalItem(string unescaped, string escaped)
    {
        //转义后符号对应的占为符
        public string EscapedPlaceholder = EscapeToPlaceHolder(escaped);
        //解码时不直接一步解出，转为代解码时的占位符号
        public string UnescapedPlaceholder = EscapeToPlaceHolderUsed(unescaped);
        public string Unescaped = unescaped;
        public string Escaped = escaped;
    }
    private List<_internalItem> _items = [];
    private const string _internalToken = "$";
    //转义_internaltoken本身
    private const string _placeholderInternal = "$0";
    //转义未被使用过的Token
    private const string _placeholderToken = "$1";
    //转义未items中使用过的Token，这样未使用过的token可以删除
    private const string _placeholderTokenUsed = "$2";
    private static string EscapeToPlaceHolder(string str)
    {
        return str.Replace(_internalToken, _placeholderInternal).Replace(Token, _placeholderToken);
    }
    private static string EscapeToPlaceHolderUsed(string str)
    {
        return str.Replace(_internalToken, _placeholderInternal).Replace(Token, _placeholderTokenUsed);
    }
    private static string UnescapeFromPlaceHolderUsed(string str)
    {
        return str.Replace(_placeholderTokenUsed, Token).Replace(_placeholderInternal, _internalToken);
    }

    public Escaper WithItem(string unescaped, string escaped)
    {
        _items.Add(new _internalItem(unescaped, escaped));
        return this;
    }
    public string Escape(string str)
    {
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
            str = str.Replace(item.EscapedPlaceholder, item.UnescapedPlaceholder);
        }
        str = str.Replace(_placeholderToken, "");
        str = UnescapeFromPlaceHolderUsed(str);
        return str;
    }
}