using System.Collections.Generic;


namespace HellMapManager.Models;

public partial class Route
{
    public Route() { }
    public string Key { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Group { get; set; } = "";

    public List<string> Rooms = [];
    public bool Validated()
    {
        return Key != "";
    }
    public const string EncodeKey = "Route";

    public string Encode()
    {
        return HMMFormatter.EncodeKeyAndValue1(EncodeKey,
            HMMFormatter.EncodeList1([
                HMMFormatter.Escape(Key),//0
                HMMFormatter.Escape(Group),//1
                HMMFormatter.Escape(Desc),//2
                HMMFormatter.EncodeList2(Rooms.ConvertAll(HMMFormatter.Escape)),//3
            ])
        );
    }
    public static Route Decode(string val)
    {
        var result = new Route();
        var kv = HMMFormatter.DecodeKeyValue1(val);
        var list = HMMFormatter.DecodeList1(kv.Value);
        result.Key = HMMFormatter.UnescapeAt(list, 0);
        result.Group = HMMFormatter.UnescapeAt(list, 1);
        result.Desc = HMMFormatter.UnescapeAt(list, 2);
        result.Rooms = HMMFormatter.DecodeList2(HMMFormatter.At(list, 3)).ConvertAll(HMMFormatter.Unescape);
        return result;
    }

}

public partial class Route
{
    public int RoomsCount
    {
        get => Rooms.Count;
    }
    public string AllRooms
    {
        get
        {
            return string.Join(";", Rooms);
        }
    }
    public bool Filter(string val)
    {
        if (Key.Contains(val) ||
            Desc.Contains(val) ||
            Group.Contains(val))
        {
            return true;
        }
        foreach (var room in Rooms)
        {
            if (room.Contains(val))
            {
                return true;
            }
        }
        return false;
    }

}