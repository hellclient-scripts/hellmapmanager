using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Cores;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.PickRoomWindow;

public class PickRoomWindowViewModel:ObservableObject
{
    public int GetMapRoomsCount
    {
        get => AppKernel.Instance.MapDatabase.Current != null ? (AppKernel.Instance.MapDatabase.Current.Map.Rooms.Count) : 0;
    }
    public string RoomsFilter { get; set; } = "";
    public void FilterRooms()
    {
        OnPropertyChanged(nameof(FilteredRooms));
    }
    public ObservableCollection<Room> FilteredRooms
    {
        get
        {
            if (AppKernel.Instance.MapDatabase.Current != null)
            {
                var rooms = AppKernel.Instance.MapDatabase.Current.Map.Rooms;
                if (string.IsNullOrEmpty(RoomsFilter))
                {
                    return new ObservableCollection<Room>(rooms);
                }
                else
                {
                    return new ObservableCollection<Room>(rooms.FindAll(r => r.Filter(RoomsFilter)));
                }
            }
            return [];
        }
    }

}
