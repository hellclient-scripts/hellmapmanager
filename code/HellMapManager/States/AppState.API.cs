using HellMapManager.Models;
using HellMapManager.Services;

namespace HellMapManager.States;

public partial class AppState
{
    public void InsertLandmark(Landmark landmark)
    {
        if (Current != null)
        {

            Current.InsertLandmark(landmark);
            Landmark.Sort(Current.Map.Landmarks);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveLandmark(string key, string type)
    {
        if (Current != null)
        {

            Current.RemoveLandmark(key, type);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateLandmark(string key, string type, Landmark current)
    {
        if (Current != null)
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
    public void InsertMarker(Marker marker)
    {
        if (Current != null)
        {

            Current.InsertMarker(marker);
            Marker.Sort(Current.Map.Markers);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveMarker(string key)
    {
        if (Current != null)
        {

            Current.RemoveMarker(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateMarker(string key, Marker current)
    {
        if (Current != null)
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
    public void InsertRegion(Region region)
    {
        if (Current != null)
        {

            Current.InsertRegion(region);
            Region.Sort(Current.Map.Regions);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveRegion(string key)
    {
        if (Current != null)
        {

            Current.RemoveRegion(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateRegion(string key, Region current)
    {
        if (Current != null)
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
    public void ImportRoomsHFile(string file)
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
    public void InsertRoom(Room room)
    {
        if (Current != null)
        {

            Current.InsertRoom(room);
            Room.Sort(Current.Map.Rooms);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveRoom(string key)
    {
        if (Current != null)
        {

            Current.RemoveRoom(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateRoom(string key, Room current)
    {
        if (Current != null)
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
    public void InsertRoute(Route route)
    {
        if (Current != null)
        {

            Current.InsertRoute(route);
            Route.Sort(Current.Map.Routes);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveRoute(string key)
    {
        if (Current != null)
        {

            Current.RemoveRoute(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateRoute(string key, Route current)
    {
        if (Current != null)
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
    public void InsertShortcut(Shortcut model)
    {
        if (Current != null)
        {

            Current.InsertShortcut(model);
            Shortcut.Sort(Current.Map.Shortcuts);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveShortcut(string key)
    {
        if (Current != null)
        {

            Current.RemoveShortcut(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateShortcut(string key, Shortcut current)
    {
        if (Current != null)
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
    public void InsertSnapshot(Snapshot model)
    {
        if (Current != null)
        {

            Current.InsertSnapshot(model);
            Snapshot.Sort(Current.Map.Snapshots);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveSnapshot(string key, string type, string value)
    {
        if (Current != null)
        {

            Current.RemoveSnapshot(key, type, value);
            Snapshot.Sort(Current.Map.Snapshots);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void InsertTrace(Trace trace)
    {
        if (Current != null)
        {

            Current.InsertTrace(trace);
            Trace.Sort(Current.Map.Traces);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveTrace(string key)
    {
        if (Current != null)
        {

            Current.RemoveTrace(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateTrace(string key, Trace current)
    {
        if (Current != null)
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
    public void InsertVariable(Variable model)
    {
        if (Current != null)
        {

            Current.InsertVariable(model);
            Variable.Sort(Current.Map.Variables);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveVariable(string key)
    {
        if (Current != null)
        {

            Current.RemoveVariable(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateVariable(string key, Variable current)
    {
        if (Current != null)
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