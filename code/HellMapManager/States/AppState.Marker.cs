using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
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
}