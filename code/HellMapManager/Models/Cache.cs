using System.Collections.Generic;
namespace HellMapManager.Models;

public class Cache
{
    public Dictionary<string, Room> Rooms = [];
    public Dictionary<string, Marker> Markers = [];
    public Dictionary<string, Route> Routes = [];
    public Dictionary<string, Trace> Traces = [];
    public Dictionary<string, Region> Regions = [];
    public Dictionary<string, Landmark> Landmarks = [];
    public Dictionary<string, Shortcut> Shortcuts = [];
    public Dictionary<string, Variable> Variables = [];
    public Dictionary<string, Snapshot> Snapshots = [];

}