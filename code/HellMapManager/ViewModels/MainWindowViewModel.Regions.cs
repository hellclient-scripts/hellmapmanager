using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitRegions()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredRegions));
        };
    }
    public string RegionsFilter { get; set; } = "";
    public void FilterRegions()
    {
        OnPropertyChanged(nameof(FilteredRegions));
    }
    public ObservableCollection<Region> FilteredRegions
    {
        get
        {
            if (AppKernel.MapDatabase.Current != null)
            {
                var traces = AppKernel.MapDatabase.Current.Map.Regions;
                if (string.IsNullOrEmpty(RegionsFilter))
                {
                    return new ObservableCollection<Region>(traces);
                }
                else
                {
                    return new ObservableCollection<Region>(traces.FindAll(r => r.Filter(RegionsFilter)));
                }
            }
            return [];
        }
    }

}
