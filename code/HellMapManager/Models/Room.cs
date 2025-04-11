using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace HellMapManager.Models;


public class RoomData(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
    public bool Validated()
    {
        return Key != "" && Value != "";
    }
    public RoomData Clone()
    {
        return new RoomData(Key, Value);
    }
}

//房间的数据结构
public partial class Room
{
    public string Key { get; set; } = "";
    //房间的名称，显示用
    public string Name { get; set; } = "";
    //房间的描述，显示用
    public string Desc { get; set; } = "";
    //房间的区域，筛选用
    public string Group { get; set; } = "";
    //标签列表，筛选用
    public List<string> Tags = [];
    //房间出口列表
    public List<Exit> Exits { get; set; } = [];
    public List<RoomData> Data { get; set; } = [];
    public bool Validated()
    {
        return Key != "";
    }
    public Room Clone()
    {
        return new Room()
        {
            Key = Key,
            Name = Name,
            Desc = Desc,
            Group = Group,
            Tags = Tags.GetRange(0, Tags.Count),
            Data = Data.ConvertAll(d => d.Clone()),
        };
    }
}
public partial class Room
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Room))]
    public Room()
    {
    }
    //房间的key,必须唯一，不能为空
    public int ExitsCount
    {
        get => Exits.Count;
    }
    public string AllTags
    {
        get => String.Join(" , ", this.Tags.ToArray());
    }
    public bool HasExitTo(string key)
    {
        foreach (var exit in Exits)
        {
            if (exit.To == key)
            {
                return true;
            }
        }
        return false;
    }
    public void Sort()
    {
        Data.Sort((x, y) => x.Key.CompareTo(y.Key));
    }
    private void AddData(RoomData rd)
    {
        Data.RemoveAll((d) => d.Key == rd.Key);
        if (rd.Value != "")
        {
            Data.Add(rd);
        }
    }
    public void SetDatas(List<RoomData> list)
    {
        foreach (var rd in list)
        {
            AddData(rd);
        }
        Data.Sort((x, y) => x.Key.CompareTo(y.Key));
    }
}

public class RoomFormatter
{
    public static string Escape(string val)
    {
        return new StringBuilder(val)
        .Replace("%", "%25")
        .Replace("\n", "%0A")
        .Replace("=", "%3D")
        .Replace("@", "%40")
        .Replace("+", "%2B")
        .Replace("|", "%7C")
        .Replace(">", "%3E")
        .Replace("<", "%3C")
        .Replace(",", "%2C")
        .Replace("$", "%24")
        .ToString();
    }
    public static string Unescape(string val)
    {
        return new StringBuilder(val)
        .Replace("%0A", "\n")
        .Replace("%3D", "=")
        .Replace("%40", "@")
        .Replace("%2B", "+")
        .Replace("%7C", "|")
        .Replace("%3E", ">")
        .Replace("%3C", "<")
        .Replace("%2C", ",")
        .Replace("%24", "$")
        .Replace("%25", "%")
        .ToString();
    }
    public static StringBuilder EncodeRoom(Room room)
    {
        List<StringBuilder> Exits = [];
        foreach (Exit exit in room.Exits)
        {
            var ToRoom = exit.To;
            var Cost = exit.Cost == 1 ? "" : exit.Cost.ToString();
            var ExitDef = new StringBuilder(Escape(ToRoom)).Append(Cost == "" ? "" : ("$" + Escape(Cost)));
            var CondExit = new StringBuilder(Escape(exit.Command));
            foreach (string extag in exit.ExTags)
            {
                CondExit.Insert(0, Escape(extag)).Insert(0, "<");
            }
            var Command = CondExit;
            foreach (string tag in exit.Tags)
            {
                Command.Insert(0, Escape(tag)).Insert(0, ">");
            }
            var Exit = Command.Append(ExitDef);
            Exits.Add(Exit);
        }
        var AllExits = new StringBuilder().AppendJoin(",", Exits);
        var RoomDef = new StringBuilder(Escape(room.Group));
        foreach (string tag in room.Tags)
        {
            RoomDef.Append("+").Append(Escape(tag));
        }
        var RoomDesc = new StringBuilder(Escape(room.Name));
        if (room.Group != "" || room.Tags.Count > 0)
        {
            RoomDesc.Append(RoomDef);
        }
        var RoomInfo = new StringBuilder(Escape(room.Key)).Append("=").Append(RoomDesc);
        var Line = RoomInfo.Append("|").Append(AllExits);
        return Line;
    }
    public static Room? DecodeRoom(string line)
    {
        var room = new Room();
        var RoominfoAndAllExits = line.Split("|", 2);
        var RoomIDAndRoomDesc = RoominfoAndAllExits[0].Split("=", 2);
        if (RoomIDAndRoomDesc.Length < 2)
        {
            return null;
        }
        room.Key = Unescape(RoomIDAndRoomDesc[0].Trim());
        if (room.Key == "")
        {
            return null;
        }
        if (RoomIDAndRoomDesc.Length > 1)
        {
            var RoomNameAndRoomDef = RoomIDAndRoomDesc[1].Split("@", 2);
            room.Name = Unescape(RoomNameAndRoomDef[0].Trim());
            if (RoomNameAndRoomDef.Length > 1)
            {
                var RoomGroupAndRoomTags = RoomNameAndRoomDef[1].Split("+");
                room.Group = Unescape(RoomGroupAndRoomTags[0].Trim());
                for (var i = 1; i < RoomGroupAndRoomTags.Length; i++)
                {
                    var tag = Unescape(RoomGroupAndRoomTags[i].Trim());
                    room.Tags.Add(tag);
                }
            }
        }
        if (RoominfoAndAllExits.Length > 1)
        {
            var exits = RoominfoAndAllExits[1].Split(",");
            foreach (var exitdata in exits)
            {
                var exit = new Exit();
                var CommandAndExitDef = exitdata.Split(":", 2);
                if (CommandAndExitDef.Length < 2 || CommandAndExitDef[1].Trim() == "")
                {
                    continue;
                }
                var TagsAndCondCommand = CommandAndExitDef[0].Split(">");
                var CondCommand = TagsAndCondCommand.Last().Trim();
                if (CondCommand == "")
                {
                    continue;
                }
                var tagscount = TagsAndCondCommand.Length - 1;
                for (var i = 0; i < tagscount; i++)
                {
                    var tag = Unescape(TagsAndCondCommand[i].Trim());
                    if (tag != "")
                    {
                        exit.Tags.Add(tag);
                    }
                }
                var ExtagsAndCommand = CondCommand.Split("<");
                var extagscount = ExtagsAndCommand.Length - 1;
                for (var i = 0; i < extagscount; i++)
                {
                    var extag = Unescape(ExtagsAndCommand[i].Trim());
                    exit.ExTags.Add(extag);
                }
                exit.Command = Unescape(ExtagsAndCommand.Last().Trim());
                if (exit.Command == "")
                {
                    continue;
                }
                var ExitAndCost = CommandAndExitDef[1].Split("$", 2);
                exit.To = Unescape(ExitAndCost[0].Trim());
                if (exit.To == "")
                {
                    continue;
                }
                if (ExitAndCost.Length > 1)
                {
                    try
                    {
                        var cost = int.Parse(ExitAndCost[1].Trim());
                        exit.Cost = cost;
                    }
                    catch (Exception)
                    {
                    }
                }
                room.Exits.Add(exit);
            }
        }
        return room;
    }
}