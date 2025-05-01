using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
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
                var routes = AppState.Main.Current.Map.Routes;
                if (string.IsNullOrEmpty(RoutesFilter))
                {
                    return new ObservableCollection<Route>(routes);
                }
                else
                {
                    return new ObservableCollection<Route>(routes.FindAll(r => r.Filter(RoutesFilter)));
                }
            }
            return [];
        }
    }

}
