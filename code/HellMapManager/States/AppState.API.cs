using System.Collections.Generic;
using HellMapManager.Models;
using HellMapManager.Services;

namespace HellMapManager.States;
public class APIListOption
{
    public string? Key { get; set; } = null;
    public string? Group { get; set; } = null;
}
public partial class AppState
{
    public List<Landmark> APIListLandmarks(APIListOption option)
    {
        if (Current != null)
        {
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Landmarks;
            }
            var list = new List<Landmark>() { };
            Current.Map.Landmarks.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Markers;
            }
            var list = new List<Marker>() { };
            Current.Map.Markers.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Regions;
            }
            var list = new List<Region>() { };
            Current.Map.Regions.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Rooms;
            }
            var list = new List<Room>() { };
            Current.Map.Rooms.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Group)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Routes;
            }
            var list = new List<Route>() { };
            Current.Map.Routes.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Group)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Shortcuts;
            }
            var list = new List<Shortcut>() { };
            Current.Map.Shortcuts.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Group)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Snapshots;
            }
            var list = new List<Snapshot>() { };
            Current.Map.Snapshots.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Group)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Traces;
            }
            var list = new List<Trace>() { };
            Current.Map.Traces.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Group)
                {
                    return;
                }
                list.Add(model);
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
            if (option.Key is null && option.Group is null)
            {
                return Current.Map.Variables;
            }
            var list = new List<Variable>() { };
            Current.Map.Variables.ForEach((model) =>
            {
                if (option.Key is not null && model.Key != option.Key)
                {
                    return;
                }
                if (option.Group is not null && model.Group != option.Group)
                {
                    return;
                }
                list.Add(model);
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
}