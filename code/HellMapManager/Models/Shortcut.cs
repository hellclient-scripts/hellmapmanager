
using System.Collections.Generic;
namespace HellMapManager.Models;

public class RoomConditionExit:Exit{
    public List<Condition> RoomConditions { get; set; } = [];

}
public partial class Shortcut : RoomConditionExit
{
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";

    public new bool Validated()
    {
        return ItemKey.Validate(Key) && base.Validated();
    }
    public const string EncodeKey = "Shortcut";

    public new Shortcut Clone()
    {
        return new Shortcut()
        {
            Key = Key,
            Command = Command,
            To = To,
            RoomConditions = RoomConditions.ConvertAll(d => d.Clone()),
            Conditions = Conditions.ConvertAll(d => d.Clone()),
            Cost = Cost,
            Group = Group,
            Desc = Desc,
        };
    }
    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList2(RoomConditions.ConvertAll(d=>HMMFormatter.EncodeToggleValue(ToggleValue.FromCondition(d)))),//3
                HMMFormatter.Escape(Command),//4
                HMMFormatter.Escape(To),//5
                HMMFormatter.EncodeList2(Conditions.ConvertAll(d=>HMMFormatter.EncodeToggleValue(ToggleValue.FromCondition(d)))),//6
                HMMFormatter.Escape(HMMFormatter.Escape(Cost.ToString())),//7
            ])
        );
    }
    public static Shortcut Decode(string val)
    {
        var result = new Shortcut();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Group = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.RoomConditions = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(d => HMMFormatter.DecodeToggleValue(d).ToCondition());
        result.Command = HMMFormatter.UnescapeAt(list, 4);
        result.To = HMMFormatter.UnescapeAt(list, 5);
        result.Conditions = HMMFormatter.DecodeList2(HMMFormatter.At(list, 6)).ConvertAll(d => HMMFormatter.DecodeToggleValue(d).ToCondition());
        result.Cost = HMMFormatter.UnescapeIntAt(list, 7, 0);
        return result;
    }
    public bool Filter(string val)
    {
        if (Key.Contains(val) || Command.Contains(val) || To.Contains(val) || Group.Contains(val) || Desc.Contains(val))
        {
            return true;
        }
        return false;
    }
    public bool Equal(Shortcut model)
    {
        if (Key != model.Key || Command != model.Command || To != model.To || Group != model.Group || Desc != model.Desc || Cost != model.Cost)
        {
            return false;
        }
        if (RoomConditions.Count != model.RoomConditions.Count)
        {
            return false;
        }
        for (int i = 0; i < RoomConditions.Count; i++)
        {
            if (!RoomConditions[i].Equal(model.RoomConditions[i]))
            {
                return false;
            }
        }
        if (Conditions.Count != model.Conditions.Count)
        {
            return false;
        }
        for (int i = 0; i < Conditions.Count; i++)
        {
            if (!Conditions[i].Equal(model.Conditions[i]))
            {
                return false;
            }
        }
        return true;
    }
    public new void Arrange()
    {
        base.Arrange();
        RoomConditions.Sort(((x, y) =>
        {
            if (x.Not == y.Not)
            {
                return x.Key.CompareTo(y.Key);
            }
            else
            {
                return x.Not.CompareTo(y.Not);
            }
        }));
    }
    public static void Sort(List<Shortcut> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : x.Key.CompareTo(y.Key));
    }
}