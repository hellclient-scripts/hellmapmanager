using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitRooms()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
            if (AppKernel.MapDatabase.Current != null)
            {
                var rooms = AppKernel.MapDatabase.Current.Map.Rooms;
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
