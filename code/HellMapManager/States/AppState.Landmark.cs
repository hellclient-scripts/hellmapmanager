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
    public void UpdateLandmark(string key, string type, Landmark current)
    {
        if (Current != null)
        {

            if (key != current.Key || type != current.Type)
            {
                Current.RemoveLandmark(key, type);
            }
            Current.InsertLandmark(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}