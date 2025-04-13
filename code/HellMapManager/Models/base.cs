namespace HellMapManager.Models;

public class Condition(string key, bool not)
{
    public string Key { get; set; } = key;
    public bool Not { get; set; } = not;
}

public class TypedCondition(string key, string value, bool not)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
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