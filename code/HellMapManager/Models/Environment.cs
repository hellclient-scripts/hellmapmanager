using System.Collections.Generic;


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
    public List<Shortcut> Shortcuts = [];
    public List<string> Whitelist = [];
    public List<string> Blocklist = [];
    public List<Link> BlockedLinks = [];
    public int MaxExitCost=0;
}