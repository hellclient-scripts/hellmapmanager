using System.Collections.Generic;

namespace HellMapManager.Models;

public class Condition(string key, bool not)
{
    public string Key { get; set; } = key;
    public bool Not { get; set; } = not;
    public bool Validated()
    {
        return Key != "";
    }
    public bool Equal(Condition model)
    {
        return Key == model.Key && Not == model.Not;
    }
    public Condition Clone()
    {
        return new Condition(Key, Not);
    }
}

public class ValueTag(string key, int value)
{
    public string Key { get; set; } = key;
    public int Value { get; set; } = value;
    public bool Validated()
    {
        return Key != "";
    }
    public bool Equal(ValueTag model)
    {
        return Key == model.Key && Value == model.Value;
    }
    public ValueTag Clone()
    {
        return new ValueTag(Key, Value);
    }
    public override string ToString()
    {
        return Value == 0 ? Key : $"{Key}:{Value}";
    }

}
public class ValueCondition(string key, int value, bool not)
{
    public string Key { get; set; } = key;
    public bool Not { get; set; } = not;
    public int Value { get; set; } = value;
    public bool Validated()
    {
        return Key != "";
    }
    public bool Equal(ValueCondition model)
    {
        return Key == model.Key && Not == model.Not && Value == model.Value;
    }
    public ValueCondition Clone()
    {
        return new ValueCondition(Key, Value, Not);
    }
    public override string ToString()
    {
        var label = Not ? $"!{Key}" : Key;
        return Value == 0 ? label : $"{label}:{Value}";
    }
    public string KeyLabel
    {
        get => Value == 0 ? Key : $"{Key}:{Value}";
    }
}


public class TypedConditions(string key, List<string> conditions, bool not)
{
    public string Key { get; set; } = key;
    public List<string> Conditions { get; set; } = conditions;
    public bool Not { get; set; } = not;
    public bool Validated()
    {
        return Key != "";
    }
    public bool Equal(TypedConditions model)
    {
        if (Key != model.Key || Not != model.Not)
        {
            return false;
        }
        if (Conditions.Count != model.Conditions.Count)
        {
            return false;
        }
        for (var i = 0; i < Conditions.Count; i++)
        {
            if (Conditions[i] != model.Conditions[i])
            {
                return false;
            }
        }
        return true;
    }
    public TypedConditions Clone()
    {
        return new TypedConditions(Key, new([.. Conditions]), Not);
    }

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
    public bool Equal(Data model)
    {
        return Key == model.Key && Value == model.Value;
    }
}