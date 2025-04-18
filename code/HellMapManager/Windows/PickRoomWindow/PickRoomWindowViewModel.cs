using System;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.States;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.PickRoomWindow;

public class PickRoomWindowViewModel:ObservableObject
{
    public static int GetMapRoomsCount
    {
        get => AppState.Main.Current != null ? (AppState.Main.Current.Map.Rooms.Count) : 0;
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
            if (AppState.Main.Current != null)
            {
                var rooms = AppState.Main.Current.Map.Rooms;
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
