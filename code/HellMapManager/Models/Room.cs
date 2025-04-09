using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace HellMapManager.Models;

//房间的数据结构
[XmlRootAttribute("Room")]
public class Room
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Room))]
    public Room()
    {
    }
    [XmlAttribute]
    //房间的key,必须唯一，不能为空
    public string Key { get; set; } = "";
    //房间的名称，显示用
    [XmlAttribute]
    public string Name { get; set; } = "";
    [XmlText]
    //房间的描述，显示用
    public string Desc { get; set; } = "";
    [XmlAttribute]
    //房间的区域，筛选用
    public string Zone { get; set; } = "";
    //标签列表，筛选用
    [XmlElement(ElementName = "Tag", Type = typeof(string))]
    public List<string> Tags = [];
    //房间出口列表
    [XmlElement(ElementName = "Exit", Type = typeof(Exit))]
    public List<Exit> Exits{get;set;} = [];
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
    public DateTime Updated;
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
        var RoomDef = new StringBuilder(Escape(room.Zone));
        foreach (string tag in room.Tags)
        {
            RoomDef.Append("+").Append(Escape(tag));
        }
        var RoomDesc = new StringBuilder(Escape(room.Name));
        if (room.Zone != "" || room.Tags.Count > 0)
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
                var RoomZoneAndRoomTags = RoomNameAndRoomDef[1].Split("+");
                room.Zone = Unescape(RoomZoneAndRoomTags[0].Trim());
                for (var i = 1; i < RoomZoneAndRoomTags.Length; i++)
                {
                    var tag = Unescape(RoomZoneAndRoomTags[i].Trim());
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