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
        AppState.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(GetRooms));
        };
    }
    public ObservableCollection<Room> GetRooms
    {
        get => new ObservableCollection<Room>(AppState.Current != null ? (AppState.Current.Map.Rooms).ToArray() : []);
    }
    public void OnDumpRoom(Room room)
    {
        var rm = Mapper.RelationMap(AppState.Current!, room.Key, AppPreset.RelationMaxDepth);
        if (rm is not null)
        {
            AppState.RaiseShowRelationMapEvent(this, rm);
        }
    }
}
