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
}
