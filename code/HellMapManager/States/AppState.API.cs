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
            });
            return list;
        }
        return [];
    }
    public void APIInsertLandmark(Landmark landmark)
    {
        if (Current != null && landmark.Validated())
        {
            Current.InsertLandmark(landmark);
            Landmark.Sort(Current.Map.Landmarks);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveLandmark(string key, string type)
    {
        if (Current != null)
        {

            Current.RemoveLandmark(key, type);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIUpdateLandmark(string key, string type, Landmark current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key || type != current.Type)
            {
                Current.RemoveLandmark(key, type);
            }
            Current.InsertLandmark(current);
            Landmark.Sort(Current.Map.Landmarks);
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
            });
            return list;
        }
        return [];
    }
    public void APIInsertMarker(Marker marker)
    {
        if (Current != null && marker.Validated())
        {

            Current.InsertMarker(marker);
            Marker.Sort(Current.Map.Markers);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveMarker(string key)
    {
        if (Current != null)
        {

            Current.RemoveMarker(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIUpdateMarker(string key, Marker current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key)
            {
                Current.RemoveMarker(key);
            }
            Current.InsertMarker(current);
            Marker.Sort(Current.Map.Markers);
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
            });
            return list;
        }
        return [];
    }
    public void APIInsertRegion(Region region)
    {
        if (Current != null && region.Validated())
        {

            Current.InsertRegion(region);
            Region.Sort(Current.Map.Regions);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveRegion(string key)
    {
        if (Current != null)
        {

            Current.RemoveRegion(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIUpdateRegion(string key, Region current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key)
            {
                Current.RemoveRegion(key);
            }
            Current.InsertRegion(current);
            Region.Sort(Current.Map.Regions);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIImportRoomsHFile(string file)
    {
        if (Current != null)
        {
            var rooms = RoomsH.Open(file);
            foreach (var room in rooms)
            {
                Current.InsertRoom(room);
            }
            Room.Sort(Current.Map.Rooms);
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
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
            });
            return list;
        }
        return [];
    }
    public void APIInsertRoom(Room room)
    {
        if (Current != null && room.Validated())
        {

            Current.InsertRoom(room);
            Room.Sort(Current.Map.Rooms);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveRoom(string key)
    {
        if (Current != null)
        {

            Current.RemoveRoom(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIUpdateRoom(string key, Room current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key)
            {
                Current.RemoveRoom(key);
            }
            Current.InsertRoom(current);
            Room.Sort(Current.Map.Rooms);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertRoute(Route route)
    {
        if (Current != null && route.Validated())
        {

            Current.InsertRoute(route);
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
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveRoute(string key)
    {
        if (Current != null)
        {

            Current.RemoveRoute(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIUpdateRoute(string key, Route current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key)
            {
                Current.RemoveRoute(key);
            }
            Current.InsertRoute(current);
            Route.Sort(Current.Map.Routes);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertShortcut(Shortcut model)
    {
        if (Current != null && model.Validated())
        {

            Current.InsertShortcut(model);
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
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveShortcut(string key)
    {
        if (Current != null)
        {

            Current.RemoveShortcut(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIUpdateShortcut(string key, Shortcut current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key)
            {
                Current.RemoveShortcut(key);
            }
            Current.InsertShortcut(current);
            Shortcut.Sort(Current.Map.Shortcuts);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertSnapshot(Snapshot model)
    {
        if (Current != null && model.Validated())
        {

            Current.InsertSnapshot(model);
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
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveSnapshot(string key, string type, string value)
    {
        if (Current != null)
        {

            Current.RemoveSnapshot(key, type, value);
            Snapshot.Sort(Current.Map.Snapshots);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertTrace(Trace trace)
    {
        if (Current != null && trace.Validated())
        {

            Current.InsertTrace(trace);
            Trace.Sort(Current.Map.Traces);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIRemoveTrace(string key)
    {
        if (Current != null)
        {

            Current.RemoveTrace(key);
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
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
            });
            return list;
        }
        return [];
    }
    public void APIUpdateTrace(string key, Trace current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key)
            {
                Current.RemoveTrace(key);
            }
            Current.InsertTrace(current);
            Trace.Sort(Current.Map.Traces);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIInsertVariable(Variable model)
    {
        if (Current != null && model.Validated())
        {

            Current.InsertVariable(model);
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
                if (option.Group is not null && model.Group != option.Key)
                {
                    return;
                }
            });
            return list;
        }
        return [];
    }
    public void APIRemoveVariable(string key)
    {
        if (Current != null)
        {

            Current.RemoveVariable(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void APIUpdateVariable(string key, Variable current)
    {
        if (Current != null && current.Validated())
        {

            if (key != current.Key)
            {
                Current.RemoveVariable(key);
            }
            Current.InsertVariable(current);
            Variable.Sort(Current.Map.Variables);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}