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

}

public class Context
{
    public Dictionary<string, int> Tags = [];
    public List<ValueCondition> RoomConditions = [];
    public Dictionary<string, Room> Rooms = [];
    public Dictionary<string, bool> Whitelist = [];
    public Dictionary<string, bool> Blacklist = [];
    public List<RoomConditionExit> Shortcuts = [];
    public Dictionary<string, List<Path>> Paths = [];
    public Dictionary<string, Dictionary<string, bool>> BlockedLinks = [];

    public Dictionary<string, Dictionary<string, int>> CommandCosts = [];
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
        RoomConditions.AddRange(conditions);
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
    public Context ClearWhitelist()
    {
        Whitelist.Clear();
        return this;
    }
    public Context WithWhitelist(List<string> list)
    {
        foreach (var item in list)
        {
            Whitelist[item] = true;
        }
        return this;
    }
    public Context ClearBlacklist()
    {
        Blacklist.Clear();
        return this;
    }
    public Context WithBlacklist(List<string> list)
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
        Shortcuts.AddRange(list);
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
                Paths[item.From].Add(item);
            }
            else
            {
                Paths[item.From] = [item];
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
    public bool IsBlocked(string from, string to)
    {
        return BlockedLinks.ContainsKey(from) && BlockedLinks[from].ContainsKey(to);
    }
    public static Context FromEnvironment(Environment env)
    {
        var context = new Context();
        context.WithTags(env.Tags);
        context.WithRoomConditions(env.RoomConditions);
        context.WithRooms(env.Rooms);
        context.WithWhitelist(env.Whitelist);
        context.WithBlacklist(env.Blacklist);
        context.WithShortcuts(env.Shortcuts);
        context.WithPaths(env.Paths);
        context.WithBlockedLinks(env.BlockedLinks);
        context.WithCommandCosts(env.CommandCosts);
        return context;
    }
    public Environment ToEnvironment()
    {
        var env = new Environment();
        env.Tags = [.. Tags.Select(kv => new ValueTag(kv.Key, kv.Value))];
        env.RoomConditions = RoomConditions;
        env.Rooms = Rooms.Values.ToList();
        env.Whitelist = Whitelist.Keys.ToList();
        env.Blacklist = Blacklist.Keys.ToList();
        env.Shortcuts = Shortcuts;
        env.Paths = Paths.Values.SelectMany(p => p).ToList();
        foreach (var f in BlockedLinks)
        {
            foreach (var t in f.Value)
            {
                if (t.Value)
                {
                    env.BlockedLinks.Add(new Link() { From = f.Key, To = t.Key });
                }
            }
        }
        foreach (var c in CommandCosts)
        {
            foreach (var t in c.Value)
            {
                env.CommandCosts.Add(new CommandCost() { Command = c.Key, To = t.Key, Cost = t.Value });
            }
        }
        return env;
    }
}