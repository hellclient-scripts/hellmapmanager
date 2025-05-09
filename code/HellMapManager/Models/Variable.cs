namespace HellMapManager.Models;
using System.Collections.Generic;


public class Variable
{
    public Variable() { }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public bool Validated()
    {
        return ItemKey.Validate(Key);
    }
    public const string EncodeKey = "Variable";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Value),//1
                HMMFormatter.Escape(Group),//2
                HMMFormatter.Escape(Desc),//3
            ])
        );
    }
    public static Variable Decode(string val)
    {
        var result = new Variable();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Value = HMMFormatter.UnescapeAt(list, 1);
        result.Group = HMMFormatter.UnescapeAt(list, 2);
        result.Desc = HMMFormatter.UnescapeAt(list, 3);
        return result;
    }
    public Variable Clone()
    {
        return new Variable()
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }
    public bool Filter(string filter)
    {
        if (Key.Contains(filter))
        {
            return true;
        }
        if (Value.Contains(filter))
        {
            return true;
        }
        if (Group.Contains(filter))
        {
            return true;
        }
        if (Desc.Contains(filter))
        {
            return true;
        }
        return false;
    }
    public bool Equal(Variable model)
    {
        if (Key == model.Key && Value == model.Value && Group == model.Group && Desc == model.Desc)
        {
            return true;
        }
        return false;
    }
    public void Arrange()
    { }
    public static void Sort(List<Variable> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
    }
}