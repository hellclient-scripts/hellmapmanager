using System;
using System.Text.RegularExpressions;

namespace HellMapManager.Models;

public enum FilterKeywordType
{
    Any,
    Wrong,
    Misc,
    Message,
    Key,
    Name,
    To,
    Tag,
    Command,
    Group,
    Type,

    Desc,
    Value,
}
public class FilterKeyword
{
    public FilterKeywordType Type = FilterKeywordType.Any;
    public bool Not = false;
    public bool PartialMatch = true;
    public string Value = "";
    public bool Match(string target, FilterKeywordType type)
    {
        if (Type != FilterKeywordType.Any && Type != type)
        {
            return false;
        }
        return Not != (PartialMatch ? target.Contains(Value) : target == Value);
    }
}