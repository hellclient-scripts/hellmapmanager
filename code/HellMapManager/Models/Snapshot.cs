
using System;
using System.Collections.Generic;
using Avalonia.Input;
namespace HellMapManager.Models;
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
        return Key != "" && Timestamp > 0;
    }
    public const string EncodeKey = "Snapshot";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Timestamp.ToString()),//1
                HMMFormatter.Escape(Group),//2
                HMMFormatter.Escape(Type),//3
                                HMMFormatter.Escape(Value),//4
            ])
        );
    }
    public static Snapshot Decode(string val)
    {
        var result = new Snapshot();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Timestamp = HMMFormatter.UnescapeIntAt(list, 1, -1);
        result.Group = HMMFormatter.UnescapeAt(list, 2);
        result.Type = HMMFormatter.UnescapeAt(list, 3);
        result.Value = HMMFormatter.UnescapeAt(list, 4);
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
}