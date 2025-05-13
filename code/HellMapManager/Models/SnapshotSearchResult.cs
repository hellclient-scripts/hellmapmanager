using System.Collections.Generic;

namespace HellMapManager.Models;
public class SnapshotFilter(string? key, string? type, string? group)
{
    public string? Key { get; set; } = key;
    public string? Type { get; set; } = type;
    public string? Group { get; set; } = group;
    public bool Validate(Snapshot model)
    {
        if (Key != null && model.Key != Key)
        {
            return false;
        }
        if (Type != null && model.Type != Type)
        {
            return false;
        }
        if (Group != null && model.Group != Group)
        {
            return false;
        }
        return true;
    }
}

public class SnapshotSearch
{
    public string? Type { get; set; }
    public string? Group { get; set; }
    public List<string> Keywords { get; set; } = [];
    public bool PartialMatch = true;
    public bool Any = false;
    private bool match(string keyword, Snapshot model)
    {

        if (PartialMatch)
        {
            return model.Value.Contains(keyword);
        }
        else
        {
            return model.Value == keyword;
        }
    }
    public bool Validate(Snapshot model)
    {
        if (Type != null && model.Type != Type)
        {
            return false;
        }
        if (Group != null && model.Group != Group)
        {
            return false;
        }
        if (Keywords.Count == 0)
        {
            return true;
        }

        foreach (var keyword in Keywords)
        {
            if (keyword != "")
            {
                if (match(keyword, model) == Any)
                {
                    return Any;
                }
            }
        }
        return !Any;
    }
}

public class SnapshotSearchResult
{
    public string Key { get; set; } = "";
    public int Sum { get; set; } = 0;
    public int Count { get; set; } = 0;
    public List<Snapshot> Items { get; set; } = [];
    public void Add(Snapshot model, bool match)
    {
        Items.Add(model);
        Sum += model.Count;
        if (match)
        {
            Count += model.Count;
        }
    }
}