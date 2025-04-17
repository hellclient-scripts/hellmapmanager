using System.Collections.Generic;

namespace HellMapManager.Models;

public class Condition(string key, bool not)
{
    public string Key { get; set; } = key;
    public bool Not { get; set; } = not;
}

public class TypedConditions(string key, List<string> conditions, bool not)
{
    public string Key { get; set; } = key;
    public List<string> Conditions { get; set; } = conditions;
    public bool Not { get; set; } = not;

}
public class Data(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
    public bool Validated()
    {
        return Key != "" && Value != "";
    }
    public Data Clone()
    {
        return new Data(Key, Value);
    }
}