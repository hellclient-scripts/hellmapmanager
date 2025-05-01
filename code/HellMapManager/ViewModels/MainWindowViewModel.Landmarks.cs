using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitLandmarks()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
            if (AppState.Main.Current != null)
            {
                var traces = AppState.Main.Current.Map.Landmarks;
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
