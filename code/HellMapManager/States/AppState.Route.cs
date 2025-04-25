using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertRoute(Route route)
    {
        if (Current != null)
        {

            Current.InsertRoute(route);
            Current.Map.Arrange();
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
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}