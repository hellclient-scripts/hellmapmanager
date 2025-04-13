
namespace HellMapManager.Models;
public class Condition(string key, bool not)
{
    public string Key { get; set; } = key;
    public bool Not { get; set; } = not;
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
