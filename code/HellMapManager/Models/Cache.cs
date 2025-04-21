using System.Collections.Generic;
namespace HellMapManager.Models;

public class Cache
{
    public Dictionary<string, Room> Rooms = [];
    public Dictionary<string, Alias> Aliases = [];
    public Dictionary<string, Route> Routes = [];
    public Dictionary<string, Trace> Traces = [];
    public Dictionary<string, Region> Regions = [];
    public Dictionary<string, Landmark> Landmarks = [];
    public Dictionary<string, Shortcut> Shortcuts = [];

}