using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
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
}