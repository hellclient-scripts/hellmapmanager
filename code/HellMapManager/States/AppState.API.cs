using System.Collections.Generic;
using HellMapManager.Models;

namespace HellMapManager.States;
public class APIListOption
{
    private readonly Dictionary<string, bool> AllKeys = new();
    private readonly Dictionary<string, bool> AllGroups = new();
    public APIListOption Clear()
    {
        AllKeys.Clear();
        AllGroups.Clear();
        return this;
    }
    public APIListOption WithKeys(List<string> keys)
    {
        foreach (var key in keys)
        {
            AllKeys[key] = true;
        }
        return this;
    }
    public APIListOption WithGroups(List<string> groups)
    {
        foreach (var group in groups)
        {
            AllGroups[group] = true;
        }
        return this;
    }
    public List<string> Keys()
    {
        var result = new List<string>();
        foreach (var key in AllKeys.Keys)
        {
            result.Add(key);
        }
        return result;
    }
    public List<string> Groups()
    {
        var result = new List<string>();
        foreach (var group in AllGroups.Keys)
        {
            result.Add(group);
        }
        return result;
    }
    public bool Validate(string key, string group)
    {
        if (AllKeys.Count > 0 && !AllKeys.ContainsKey(key))
        {
            return false;
        }
        if (AllGroups.Count > 0 && !AllGroups.ContainsKey(group))
        {
            return false;
        }
        return true;
    }
    public bool IsEmpty()
    {
        return AllKeys.Count == 0 && AllGroups.Count == 0;
    }
}
public partial class AppState
{
    public List<Landmark> APIListLandmarks(APIListOption option)
    {
        if (Current != null)
        {

            if (option.IsEmpty())
            {
                return Current.Map.Landmarks;
            }
            var list = new List<Landmark>() { };
            Current.Map.Landmarks.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIInsertLandmarks(List<Landmark> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertLandmark(model);
                }
            }
            Landmark.Sort(Current.Map.Landmarks);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveLandmarks(List<LandmarkKey> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveLandmark(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Marker> APIListMarkers(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Markers;
            }
            var list = new List<Marker>() { };
            Current.Map.Markers.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIInsertMarkers(List<Marker> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertMarker(model);
                }
            }
            Marker.Sort(Current.Map.Markers);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveMarkers(List<string> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveMarker(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Region> APIListRegions(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Regions;
            }
            var list = new List<Region>() { };
            Current.Map.Regions.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIInsertRegions(List<Region> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertRegion(model);
                }
            }
            Region.Sort(Current.Map.Regions);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveRegions(List<string> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveRegion(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Room> APIListRooms(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Rooms;
            }
            var list = new List<Room>() { };
            Current.Map.Rooms.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIInsertRooms(List<Room> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertRoom(model);
                }
            }
            Room.Sort(Current.Map.Rooms);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveRooms(List<string> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveRoom(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertRoutes(List<Route> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertRoute(model);
                }
            }
            Route.Sort(Current.Map.Routes);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Route> APIListRoutes(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Routes;
            }
            var list = new List<Route>() { };
            Current.Map.Routes.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveRoutes(List<string> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveRoute(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertShortcuts(List<Shortcut> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertShortcut(model);
                }
            }
            Shortcut.Sort(Current.Map.Shortcuts);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Shortcut> APIListShortcuts(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Shortcuts;
            }
            var list = new List<Shortcut>() { };
            Current.Map.Shortcuts.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveShortcuts(List<string> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveShortcut(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertSnapshots(List<Snapshot> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertSnapshot(model);
                }
            }
            Snapshot.Sort(Current.Map.Snapshots);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Snapshot> APIListSnapshots(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Snapshots;
            }
            var list = new List<Snapshot>() { };
            Current.Map.Snapshots.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveSnapshots(List<SnapshotKey> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var k in keys)
            {
                Current.RemoveSnapshot(k);
            }
            Snapshot.Sort(Current.Map.Snapshots);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertTraces(List<Trace> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertTrace(model);
                }
            }
            Trace.Sort(Current.Map.Traces);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }

    public void APIRemoveTraces(List<string> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveTrace(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Trace> APIListTraces(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Traces;
            }
            var list = new List<Trace>() { };
            Current.Map.Traces.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIInsertVariables(List<Variable> models)
    {
        if (Current != null && models.Count > 0)
        {
            foreach (var model in models)
            {
                if (model.Validated())
                {
                    Current.InsertVariable(model);
                }
            }
            Variable.Sort(Current.Map.Variables);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Variable> APIListVariables(APIListOption option)
    {
        if (Current != null)
        {
            if (option.IsEmpty())
            {
                return Current.Map.Variables;
            }
            var list = new List<Variable>() { };
            Current.Map.Variables.ForEach((model) =>
            {
                if (option.Validate(model.Key, model.Group))
                {
                    list.Add(model);
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveVariables(List<string> keys)
    {
        if (Current != null && keys.Count > 0)
        {
            foreach (var key in keys)
            {
                Current.RemoveVariable(key);
            }
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Step> APIQueryPathAny(string from, List<string> target, Context context)
    {
        // Todo
        return [];
    }

    public List<Step> APIQueryPathAll(string start, List<string> target, Context context)
    {
        // Todo
        return [];
    }
    public List<Step> APIQueryPathOrdered(string start, List<string> target, Context context)
    {
        // Todo
        return [];
    }
    public List<string> APIQueryRegionRooms(string key)
    {
        // Todo
        return [];
    }

    public List<string> APIDilate(List<string> src, int iterations, Context context)
    {
        // Todo
        return [];
    }
    public string APITrackExit(string start, string command, Context context)
    {
        if (Current != null)
        {
            if (Current.Cache.Rooms.TryGetValue(start, out Room? room))
            {
                foreach (var exit in room.Exits)
                {
                    if (exit.Command == command && context.ValidateExit(exit))
                    {
                        return exit.To;
                    }
                }
            }
        }
        return "";
    }
    public string GetVariable(string key)
    {
        if (Current != null)
        {
            if (Current.Cache.Variables.TryGetValue(key, out Variable? variable))
            {
                return variable.Value;
            }
        }
        return "";
    }
    public void APIExpireSnapshot(SnapshotFilter filter, int ExpiredBeforeTimestamp)
    {
        if (Current != null)
        {
            Current.Map.Snapshots.RemoveAll((s) =>
            {
                if (filter.Validate(s))
                {
                    return s.Timestamp < ExpiredBeforeTimestamp;
                }
                return false;
            });
            Snapshot.Sort(Current.Map.Snapshots);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public List<Room> APISearchRooms(RoomFilter filter)
    {
        if (Current != null)
        {
            var result = new List<Room>() { };
            Current.Map.Rooms.ForEach((model) =>
            {
                if (filter.Validate(model))
                {
                    result.Add(model);
                }
            });
            return result;
        }
        //Todo
        return [];
    }

    public List<Room> APIFilterRooms(List<string> src, RoomFilter filter)
    {
        if (Current != null)
        {
            var result = new List<Room>() { };
            src.ForEach((key) =>
            {
                if (Current.Cache.Rooms.TryGetValue(key, out Room? model))
                {
                    if (filter.Validate(model))
                    {
                        result.Add(model);
                    }
                }
            });
            return result;
        }
        //Todo
        return [];
    }
    public void APITakeSnapshot(string key, string type, string value, string group)
    {

        if (Current != null)
        {
            var snapshot = Snapshot.Create(key, type, value, group);
            Current.InsertSnapshot(snapshot);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APISearchSnapshots()
    {
        //Todo
    }
    public void APITrace(string key, string location)
    {
        if (Current != null)
        {
            if (Current.Cache.Traces.TryGetValue(key, out Trace? trace))
            {
                if (trace.Locations.Contains(location))
                {
                    return;
                }
                trace.AddLocations(new List<string>() { location });
                trace.Arrange();
                Current.MarkAsModified();
                RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public void APITagRoom(string key, string tag)
    {
        if (Current != null)
        {
            if (Current.Cache.Rooms.TryGetValue(key, out Room? room))
            {
                if (room.Tags.Contains(tag))
                {
                    return;
                }
                var prev = room.ToString();
                room.Tags.Add(tag);
                room.Arrange();
                if (room.ToString() != prev)
                {
                    Current.MarkAsModified();
                    RaiseMapFileUpdatedEvent(this);
                }
                return;
            }
        }
    }
    public void APIUntagRoom(string key, string tag)
    {
        if (Current != null)
        {
            if (Current.Cache.Rooms.TryGetValue(key, out Room? room))
            {
                if (!room.Tags.Contains(tag))
                {
                    return;
                }
                var prev = room.ToString();
                room.Tags.Remove(tag);
                room.Arrange();
                if (room.ToString() != prev)
                {
                    Current.MarkAsModified();
                    RaiseMapFileUpdatedEvent(this);
                }
                return;
            }
        }
    }
    public void APISetRoomData(string roomkey, string datakey, string datavalue)
    {
        if (Current != null)
        {
            if (Current.Cache.Rooms.TryGetValue(roomkey, out Room? room))
            {
                var prev = room.ToString();
                room.Data.RemoveAll((d) => d.Key == datakey);
                room.Data.Add(new Data(datakey, datavalue));
                room.Arrange();
                if (room.ToString() != prev)
                {
                    Current.MarkAsModified();
                    RaiseMapFileUpdatedEvent(this);
                }
                return;
            }
        }

    }
    public void APIGroupRoom(string key, string group)
    {
        if (Current != null)
        {
            if (Current.Cache.Rooms.TryGetValue(key, out Room? room))
            {
                if (room.Group == group)
                {
                    return;
                }
                room.Group = group;
                Room.Sort(Current.Map.Rooms);
                Current.MarkAsModified();
                RaiseMapFileUpdatedEvent(this);
            }
        }
    }
}