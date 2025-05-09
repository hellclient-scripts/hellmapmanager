
using System;
using System.Collections.Generic;
using HellMapManager.Utils;
namespace HellMapManager.Models;

public class SnapshotKey(string key, string type, string value)
{
    public string Key { get; set; } = key;
    public string Type { get; set; } = type;
    public string Value { get; set; } = value;
    public override string ToString()
    {
        return UniqueKeyUtil.Join([Key, Type, Value]);
    }
    public bool Equal(SnapshotKey model)
    {
        if (Key == model.Key && Type == model.Type && Value == model.Value)
        {
            return true;
        }
        return false;
    }
}
public partial class Snapshot
{
    public static Snapshot Create(string key, string type, string value, string group)
    {
        return new Snapshot()
        {
            Key = key,

            Type = type,
            Value = value,
            Timestamp = (int)(new DateTimeOffset(DateTime.UtcNow)).ToUnixTimeSeconds(),
            Group = group,
        };
    }
    public string Key { get; set; } = "";
    public int Timestamp = 0;
    public string Group { get; set; } = "";
    public string Type { get; set; } = "";

    public string Value { get; set; } = "";
    public string TimeLabel { get => DateTimeOffset.FromUnixTimeSeconds(Timestamp).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
    public bool Validated()
    {
        return ItemKey.Validate(Key) && Timestamp > 0;
    }
    public const string EncodeKey = "Snapshot";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Type),//1
                HMMFormatter.Escape(Value),//2
                HMMFormatter.Escape(Group),//3
                HMMFormatter.Escape(Timestamp.ToString()),//4
            ])
        );
    }
    public static Snapshot Decode(string val)
    {
        var result = new Snapshot();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Type = HMMFormatter.UnescapeAt(list, 1);
        result.Value = HMMFormatter.UnescapeAt(list, 2);
        result.Group = HMMFormatter.UnescapeAt(list, 3);
        result.Timestamp = HMMFormatter.UnescapeIntAt(list, 4, -1);
        return result;
    }
    public Snapshot Clone()
    {
        return new Snapshot()
        {
            Key = Key,
            Timestamp = Timestamp,
            Group = Group,
            Type = Type,
            Value = Value,
        };
    }
    public bool Filter(string filter)
    {
        if (Key.Contains(filter) || Type.Contains(filter) || Value.Contains(filter) || Group.Contains(filter))
        {
            return true;
        }
        return false;
    }
    public bool Equal(Snapshot model)
    {
        if (Key == model.Key && Type == model.Type && Value == model.Value && Group == model.Group && Timestamp == model.Timestamp)
        {
            return true;
        }
        return false;
    }
    public void Arrange()
    {
    }
    public static void Sort(List<Snapshot> list)
    {
        list.Sort((x, y) => x.Group != y.Group ? x.Group.CompareTo(y.Group) : (x.Key != y.Key ? x.Key.CompareTo(y.Key) : (x.Timestamp != y.Timestamp ? x.Timestamp.CompareTo(y.Timestamp) : (x.Type != y.Type ? x.Type.CompareTo(y.Type) : x.Value.CompareTo(y.Value)))));
    }
    public SnapshotKey UniqueKey()
    {
        return new SnapshotKey(Key, Type, Value);
    }
}