using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertSnapshot(Snapshot model)
    {
        if (Current != null)
        {

            Current.InsertSnapshot(model);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveSnapshot(Snapshot model)
    {
        if (Current != null)
        {

            Current.RemoveSnapshot(model.Key, model.Type, model.Value);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}