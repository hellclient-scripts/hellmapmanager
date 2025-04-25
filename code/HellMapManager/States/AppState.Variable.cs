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
    public void UpdateVariable(string key, Variable current)
    {
        if (Current != null)
        {

            if (key != current.Key)
            {
                Current.RemoveVariable(key);
            }
            Current.InsertVariable(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}