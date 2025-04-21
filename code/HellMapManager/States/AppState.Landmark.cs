using HellMapManager.Models;
namespace HellMapManager.States;

public partial class AppState
{
    public void InsertLandmark(Landmark landmark)
    {
        if (Current != null)
        {

            Current.InsertLandmark(landmark);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveLandmark(string key, string type)
    {
        if (Current != null)
        {

            Current.RemoveLandmark(key, type);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateLandmark(Landmark old, Landmark current)
    {
        if (Current != null)
        {

            if (old.Key != current.Key)
            {
                Current.RemoveLandmark(old.Key, old.Type);
            }
            Current.InsertLandmark(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}