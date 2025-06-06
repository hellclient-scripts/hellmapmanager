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
                var models = AppKernel.MapDatabase.Current.Map.Landmarks;
                if (string.IsNullOrEmpty(LandmarksFilter))
                {
                    return new ObservableCollection<Landmark>(models);
                }
                else
                {
                    return new ObservableCollection<Landmark>(models.FindAll(r => r.Filter(LandmarksFilter)));
                }
            }
            return [];
        }
    }
}
