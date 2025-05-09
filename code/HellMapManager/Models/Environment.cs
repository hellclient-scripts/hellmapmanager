using System.Collections.Generic;
using System.Collections.ObjectModel;


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
public class Environment
{
    public List<string> Tags = [];
    public List<Condition> RoomConditions = [];
    public List<Room> Rooms = [];
    public List<Path> Paths = [];
    public bool DisableShortcuts = true;
    public List<Exit> Shortcuts = [];
    public List<string> Whitelist = [];
    public List<string> Blocklist = [];
    public List<Link> BlockedLinks = [];
    public int MaxExitCost = 0;
}

public class Context
{
    public Dictionary<string, bool> Tags = new();
    public Dictionary<string, bool> RoomConditions = new();
    public Dictionary<string, Room> Rooms = new();
    public bool DisableShortcuts = true;
    public Dictionary<string, bool> Whitelist = new();
    public Dictionary<string, bool> Blocklist = new();
    public List<Exit> Shortcuts = [];

    public int MaxExitCost = 0;
    public bool ValidateExit(Exit exit, MapFile mf)
    {
        //Todo
        return true;
    }
    public bool ValidateRoom(Room room, MapFile mf)
    {
        //Todo
        return true;
    }
    public bool ValidatePath(Path path, MapFile mf)
    {
        //Todo
        return true;
    }

}