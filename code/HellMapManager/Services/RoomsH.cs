using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using HellMapManager.Models;
using System;


namespace HellMapManager.Services;
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
            foreach (var extag in exit.Conditions)
            {
                if (extag.Not)
                {
                    CondExit.Insert(0, Escape(extag.Key)).Insert(0, "<");
                }
            }
            var Command = CondExit;
            foreach (var tag in exit.Conditions)
            {
                if (!tag.Not)
                {

                    Command.Insert(0, Escape(tag.Key)).Insert(0, ">");
                }
            }
            var Exit = Command.Append(ExitDef);
            Exits.Add(Exit);
        }
        var AllExits = new StringBuilder().AppendJoin(",", Exits);
        var RoomDef = new StringBuilder(Escape(room.Group));
        foreach (string tag in room.Tags)
        {
            RoomDef.Append('+').Append(Escape(tag));
        }
        var RoomDesc = new StringBuilder(Escape(room.Name));
        if (room.Group != "" || room.Tags.Count > 0)
        {
            RoomDesc.Append(RoomDef);
        }
        var RoomInfo = new StringBuilder(Escape(room.Key)).Append('=').Append(RoomDesc);
        var Line = RoomInfo.Append('|').Append(AllExits);
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
                        exit.Conditions.Add(new Condition(tag, false));
                    }
                }
                var ExtagsAndCommand = CondCommand.Split("<");
                var extagscount = ExtagsAndCommand.Length - 1;
                for (var i = 0; i < extagscount; i++)
                {
                    var extag = Unescape(ExtagsAndCommand[i].Trim());
                    if (extag != "")
                    {
                        exit.Conditions.Add(new Condition(extag, true));
                    }
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
public class RoomsH
{
    public static List<Room> Open(string file)
    {
        using (var fileStream = new FileStream(file, FileMode.Open))
        {
            using (var sr = new StreamReader(fileStream, Encoding.UTF8))
            {
                var body = sr.ReadToEnd();
                var rooms = Load(body);
                return rooms;
            }
        }
    }
    public static List<Room> Load(string data)
    {
        List<Room> result = [];
        var lines = data.Split("\n");
        foreach (string line in lines)
        {
            var linedata = line.Trim();
            if (linedata.Length == 0 || linedata.StartsWith("//"))
            {
                continue;
            }
            var room = RoomFormatter.DecodeRoom(linedata);
            if (room is not null)
            {
                result.Add(room);
            }
        }
        return result;
    }
}