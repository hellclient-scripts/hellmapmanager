using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertRegion(Region region)
    {
        if (Current != null)
        {

            Current.InsertRegion(region);
            Current.Map.Arrange();
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
    public void UpdateRegion(Region old, Region current)
    {
        if (Current != null)
        {

            if (old.Key != current.Key)
            {
                Current.RemoveRegion(old.Key);
            }
            Current.InsertRegion(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}