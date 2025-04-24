using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertVariable(Variable model)
    {
        if (Current != null)
        {

            Current.InsertVariable(model);
            Current.Map.Arrange();
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
    public void UpdateVariable(Variable old, Variable current)
    {
        if (Current != null)
        {

            if (old.Key != current.Key)
            {
                Current.RemoveVariable(old.Key);
            }
            Current.InsertVariable(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}