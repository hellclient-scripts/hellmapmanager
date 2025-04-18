using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Services;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitRoutes()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredRoutes));
        };
    }
    public string RoutesFilter { get; set; } = "";
    public void FilterRoutes()
    {
        OnPropertyChanged(nameof(FilteredRoutes));
    }
    public ObservableCollection<Route> FilteredRoutes
    {
        get
        {
            if (AppState.Main.Current != null)
            {
                var aliases = AppState.Main.Current.Map.Routes;
                if (string.IsNullOrEmpty(RoutesFilter))
                {
                    return new ObservableCollection<Route>(aliases);
                }
                else
                {
                    return new ObservableCollection<Route>(aliases.FindAll(r => r.Filter(RoutesFilter)));
                }
            }
            return [];
        }
    }

}
