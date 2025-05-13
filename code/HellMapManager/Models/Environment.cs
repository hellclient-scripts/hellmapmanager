using System.Collections.Generic;
using System.Linq;


namespace HellMapManager.Models;

public class Path : Exit
{
    public string From { get; set; } = "";
}
public class Link
{
    public string From { get; set; } = "";
    public string To { get; set; } = "";
}
public class CommandCost
{
    public string Command { get; set; } = "";
    public string To { get; set; } = "";
    public int Cost { get; set; } = 1;
}
public class Environment
{
    public List<ValueTag> Tags = [];
    public List<ValueCondition> RoomConditions = [];
    public List<Room> Rooms = [];
    public List<Path> Paths = [];
    public List<RoomConditionExit> Shortcuts = [];
    public List<string> Whitelist = [];
    public List<string> Blacklist = [];
    public List<Link> BlockedLinks = [];
    public List<CommandCost> CommandCosts = [];
    public bool DisableShortcuts = false;

    public int MaxExitCost = 0;
}

public class Context
{
    public Context(MapFile mf)
    {
        MapFile = mf;
    }
    public MapFile MapFile;
    public Dictionary<string, int> Tags = [];
    public List<ValueCondition> RoomConditions = [];
    public Dictionary<string, Room> Rooms = [];
    public Dictionary<string, bool> Whitelist = [];
    public Dictionary<string, bool> Blacklist = [];
    public List<RoomConditionExit> Shortcuts = [];
    public Dictionary<string, List<Path>> Paths = [];
    public Dictionary<string, Dictionary<string, bool>> BlockedLinks = [];

    public Dictionary<string, Dictionary<string, int>> CommandCosts = [];
    public bool DisableShortcuts = false;

    public int MaxExitCost = 0;
    public Context ClearTags()
    {
        Tags.Clear();
        return this;
    }
    public Context WithTags(List<ValueTag> tags)
    {
        foreach (var tag in tags)
        {
            Tags[tag.Key] = tag.Value;
        }
        return this;
    }
    public Context ClearRoomConditions()
    {
        RoomConditions.Clear();
        return this;
    }
    public Context WithRoomConditions(List<ValueCondition> conditions)
    {
        RoomConditions = (List<ValueCondition>)RoomConditions.Concat(conditions);
        return this;
    }
    public Context ClearRooms()
    {
        Rooms.Clear();
        return this;
    }
    public Context WithRooms(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            Rooms[room.Key] = room;
        }
        return this;
    }
    public Context ClearWhiteList()
    {
        Whitelist.Clear();
        return this;
    }
    public Context WithWhiteList(List<string> list)
    {
        foreach (var item in list)
        {
            Whitelist[item] = true;
        }
        return this;
    }
    public Context ClearBlackList()
    {
        Whitelist.Clear();
        return this;
    }
    public Context WithBlackList(List<string> list)
    {
        foreach (var item in list)
        {
            Blacklist[item] = true;
        }
        return this;
    }
    public Context ClearShortcuts()
    {
        Shortcuts.Clear();
        return this;
    }
    public Context WithShortcuts(List<RoomConditionExit> list)
    {
        Shortcuts = (List<RoomConditionExit>)Shortcuts.Concat(list);
        return this;
    }
    public Context ClearPaths()
    {
        Paths.Clear();
        return this;
    }
    public Context WithPaths(List<Path> list)
    {
        foreach (var item in list)
        {

            if (Paths.TryGetValue(item.From, out var paths))
            {
                Paths[item.From] = (List<Path>)paths.Concat(list);
            }
            else
            {
                Paths[item.From] = [.. list];
            }

        }
        return this;
    }
    public Context ClearBlockedLinks()
    {
        BlockedLinks.Clear();
        return this;
    }
    public Context WithBlockedLinks(List<Link> list)
    {
        foreach (var link in list)
        {
            if (!BlockedLinks.ContainsKey(link.From))
            {
                BlockedLinks[link.From] = [];
            }
            BlockedLinks[link.From][link.To] = true;
        }
        return this;
    }
    public Context ClearCommandCosts()
    {
        CommandCosts.Clear();
        return this;
    }
    public Context WithCommandCosts(List<CommandCost> list)
    {
        foreach (var item in list)
        {
            if (!CommandCosts.ContainsKey(item.Command))
            {
                CommandCosts[item.Command] = [];
            }
            CommandCosts[item.Command][item.To] = item.Cost;
        }
        return this;
    }
    public Context WithDisableShortcuts(bool val)
    {
        this.DisableShortcuts = val;
        return this;
    }
    public bool IsBlocked(string from, string to)
    {
        return BlockedLinks.ContainsKey(from) && BlockedLinks[from].ContainsKey(to);
    }
    public Room? GetRoom(string key, MapFile mf)
    {
        Room? room = null;
        if (!Rooms.TryGetValue(key, out room))
        {
            if (!mf.Cache.Rooms.TryGetValue(key, out room))
            {
                return null;
            }
        }
        return room;
    }
    public int GetExitCost(Exit exit)
    {
        if (CommandCosts.TryGetValue(exit.Command, out var costs))
        {
            if (costs.TryGetValue(exit.To, out var cost))
            {
                return cost;
            }
        }
        return exit.Cost;
    }
    public List<Exit> GetRoomExits(Room room)
    {
        List<Exit> result = [.. room.Exits];
        if (Paths.TryGetValue(room.Key, out var list))
        {
            result = (List<Exit>)result.Concat(list);
        }
        if (!DisableShortcuts)
        {
            MapFile.Map.Shortcuts.ForEach(e =>
            {
                if (room.ValidteConditions(e.RoomConditions))
                {
                    result.Add(e);
                }
            });
            Shortcuts.ForEach(e =>
            {
                if (room.ValidteConditions(e.RoomConditions))
                {
                    result.Add(e);
                }
            });
        }
        return result;
    }
    public bool ValidateExit(Exit exit)
    {

        var room = GetRoom(exit.To, MapFile);
        if (room == null)
        {
            return false;
        }

        if (!ValidateRoom(room))
        {
            return false;
        }
        foreach (var condition in exit.Conditions)
        {
            if (Tags.ContainsKey(condition.Key) != condition.Not)
            {
                return false;
            }
        }
        return true;
    }
    public bool ValidateRoom(Room room)
    {
        if (BlockedLinks.ContainsKey(room.Key))
        {
            return false;
        }
        if (Whitelist.Count > 0 && !Whitelist.ContainsKey(room.Key))
        {
            return false;
        }
        if (!room.ValidteConditions(RoomConditions))
        {
            return false;
        }
        return true;
    }
    public bool ValidatePath(string start, Exit exit)
    {
        if (IsBlocked(start, exit.To))
        {
            return false;
        }
        return ValidateExit(exit);
    }
}