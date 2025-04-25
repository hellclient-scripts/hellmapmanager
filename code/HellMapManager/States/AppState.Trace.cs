using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertTrace(Trace trace)
    {
        if (Current != null)
        {

            Current.InsertTrace(trace);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveTrace(string key)
    {
        if (Current != null)
        {

            Current.RemoveTrace(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateTrace(string key, Trace current)
    {
        if (Current != null)
        {

            if (key != current.Key)
            {
                Current.RemoveTrace(key);
            }
            Current.InsertTrace(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}