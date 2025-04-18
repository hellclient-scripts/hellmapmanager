using HellMapManager.Models;
using HellMapManager.Services;
namespace HellMapManager.States;

public partial class AppState
{
    public void ImportRoomsHFile(string file)
    {
        if (Current != null)
        {
            Current.ImportRooms(RoomsH.Open(file));
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void InsertRoom(Room room)
    {
        if (Current != null)
        {

            Current.InsertRoom(room);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveRoom(string key)
    {
        if (Current != null)
        {

            Current.RemoveRoom(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateRoom(Room old, Room current)
    {
        if (Current != null)
        {

            if (old.Key != current.Key)
            {
                Current.RemoveRoom(old.Key);
            }
            Current.InsertRoom(current);
            Current.Map.Arrange();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
}