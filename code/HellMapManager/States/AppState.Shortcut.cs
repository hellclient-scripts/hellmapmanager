using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertShortcut(Shortcut model)
    {
        if (Current != null)
        {

            Current.InsertShortcut(model);
            Current.Map.Arrange();
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
    public void UpdateShortcut(Shortcut old, Shortcut current)
    {
        if (Current != null)
        {

            if (old.Key != current.Key)
            {
                Current.RemoveShortcut(old.Key);
            }
            Current.InsertShortcut(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}