using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertAlias(Alias alias)
    {
        if (Current != null)
        {

            Current.InsertAlias(alias);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveAlias(string key)
    {
        if (Current != null)
        {

            Current.RemoveAlias(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateAlias(Alias old, Alias current)
    {
        if (Current != null)
        {

            if (old.Key != current.Key)
            {
                Current.RemoveAlias(old.Key);
            }
            Current.InsertAlias(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}