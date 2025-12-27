using System.Collections.Generic;

namespace HellMapManager.Models;

public class SnapshotFilter(string? key, string? type, string? group)
{
    public string? Key { get; } = key;
    public string? Type { get; } = type;
    public string? Group { get; } = group;

    public int MaxCount { get; set; } = 0;
    public SnapshotFilter WithMaxCount(int count)
    {
        MaxCount = count;
        return this;
    }
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
        if (MaxCount > 0 && model.Count > MaxCount)
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

    public int MaxNoise = 0;
    private bool Match(string keyword, Snapshot model)
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
        int noise = 0;
        foreach (var keyword in Keywords)
        {
            if (keyword != "")
            {
                if (Match(keyword, model) == Any)
                {
                    if (!Any)
                    {
                        //在完全匹配时，只有噪音超过允许的最大值才验证失败。
                        noise++;
                        if (noise > MaxNoise)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
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
        Sum += model.Count;
        if (match)
        {
            Items.Add(model);
            Count += model.Count;
        }
    }
}