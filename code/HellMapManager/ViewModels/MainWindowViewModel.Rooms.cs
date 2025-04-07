using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using HellMapManager.Models;
using HellMapManager.Services;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using HellMapManager.Views.Mapfile.Rooms;
using System.Linq;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void UpdateRooms()
    {
        OnPropertyChanged(nameof(GetRooms));
    }
    public ObservableCollection<Room> GetRooms
    {
        get => new ObservableCollection<Room>(this.AppState.Current != null ? (this.AppState.Current.Map.Rooms).ToArray() : []);
    }
    public void OnDumpRoom(Room room)
    {
        Console.WriteLine(String.Join("\n", Mapper.RelationMap(AppState.Current!, room.Key, 5).Dump()));
    }
}
