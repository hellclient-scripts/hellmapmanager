using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitRooms()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredRooms));
        };
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
