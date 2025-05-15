using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitLandmarks()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredLandmarks));
        };
    }
    public string LandmarksFilter { get; set; } = "";
    public void FilterLandmarks()
    {
        OnPropertyChanged(nameof(FilteredLandmarks));
    }
    public ObservableCollection<Landmark> FilteredLandmarks
    {
        get
        {
            if (AppKernel.MapDatabase.Current != null)
            {
                var traces = AppKernel.MapDatabase.Current.Map.Landmarks;
                if (string.IsNullOrEmpty(LandmarksFilter))
                {
                    return new ObservableCollection<Landmark>(traces);
                }
                else
                {
                    return new ObservableCollection<Landmark>(traces.FindAll(r => r.Filter(LandmarksFilter)));
                }
            }
            return [];
        }
    }
}
