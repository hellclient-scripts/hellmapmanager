using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitRoutes()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
            if (AppKernel.MapDatabase.Current != null)
            {
                var models = AppKernel.MapDatabase.Current.Map.Routes;
                if (!string.IsNullOrEmpty(RoutesFilter))
                {
                    HellMapManager.Utils.FilterUtil.SplitFilter(RoutesFilter).ForEach(filter =>
                    {
                        models = models.FindAll(r => r.Filter(filter));
                    });
                }
                return new ObservableCollection<Route>(models);
            }
            return [];
        }
    }

}
