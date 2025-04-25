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
    public void UpdateRegion(string key, Region current)
    {
        if (Current != null)
        {

            if (key != current.Key)
            {
                Current.RemoveRegion(key);
            }
            Current.InsertRegion(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}