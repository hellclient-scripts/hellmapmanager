using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Services;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitRooms()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(GetRooms));
        };
    }
    public ObservableCollection<Room> GetRooms
    {
        get => new ObservableCollection<Room>(AppState.Main.Current != null ? (AppState.Main.Current.Map.Rooms).ToArray() : []);
    }
}
