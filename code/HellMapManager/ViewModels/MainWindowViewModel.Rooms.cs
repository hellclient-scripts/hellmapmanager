using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Services;
using HellMapManager.Windows.RelationMapWindow;
using Microsoft.Msagl.DebugHelpers;
using System.Threading.Tasks;
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
        get => new ObservableCollection<Room>(this.AppState.Current != null ? (this.AppState.Current.Map.Rooms).ToArray() : []);
    }
    public void OnDumpRoom(Room room)
    {
        var rm = Mapper.RelationMap(AppState.Current!, room.Key, 5);
        if (rm is not null)
        {
            AppState.RaiseShowRelationMapEvent(this, rm);
        }
    }
}
