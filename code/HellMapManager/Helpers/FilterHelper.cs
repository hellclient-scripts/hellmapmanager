
using System;
using System.Collections.Generic;
using System.Linq;
using HellMapManager.Models;
using HellMapManager.Utils.ControlCode;

namespace HellMapManager.Helpers;

public class FilterHelper
{
    const string TypeKey = "key";
    const string TypeName = "name";
    const string TypeTo = "to";
    const string TypeTag = "tag";
    const string TypeCommand = "command";
    const string TypeGroup = "group";
    const string TypeType = "type";
    const string TypeValue = "value";
    const string TypeDesc = "desc";
    const string TypeMisc = "misc";

    const string TypeMessage = "message";
    static readonly ControlCode escaper = new ControlCode().
        WithCommand(new Command("\\", "0", "\\\\")).
        WithCommand(new Command("\n", "1", "\\n")).
        WithCommand(new Command(",", "2", "\\,")).
        WithCommand(new Command("=", "3", "\\=")).
        WithCommand(new Command(" ", "4", "\\ ")).
        WithCommand(new Command("!", "5", "\\!"));

    public static FilterKeyword ParseKeyword(string raw)
    {
        var encoded = escaper.Decode(raw);
        var keyword = new FilterKeyword();
        if (encoded.StartsWith("!"))
        {
            keyword.Not = true;
            encoded = encoded[1..];
        }
        var data = encoded.Split(['='], 2);
        if (data.Length > 1)
        {
            keyword.Type = escaper.Encode(data[0].Trim()) switch
            {
                "" => FilterKeywordType.Any,
                TypeKey => FilterKeywordType.Key,
                TypeName => FilterKeywordType.Name,
                TypeTo => FilterKeywordType.To,
                TypeTag => FilterKeywordType.Tag,
                TypeCommand => FilterKeywordType.Command,
                TypeGroup => FilterKeywordType.Group,
                TypeMisc => FilterKeywordType.Misc,
                TypeType => FilterKeywordType.Type,
                TypeDesc => FilterKeywordType.Desc,
                TypeValue => FilterKeywordType.Value,
                TypeMessage => FilterKeywordType.Message,
                _ => FilterKeywordType.Wrong,
            };
            keyword.Value = escaper.Encode(data[1]);
            keyword.PartialMatch = false;
        }
        else
        {
            keyword.Value = escaper.Encode(data[0]);
            keyword.PartialMatch = true;
        }
        return keyword;
    }
    public static List<FilterKeyword> ParseKeywords(string filter)
    {
        return filter.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList().ConvertAll(r => ParseKeyword(r));
    }

}

