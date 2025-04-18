using System.Collections.Generic;
namespace HellMapManager.Models;

public class Cache
{
    public Dictionary<string, Room> Rooms = [];
    public Dictionary<string, Alias> Aliases = [];
    public Dictionary<string, Route> Routes = [];

}