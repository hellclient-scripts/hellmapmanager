using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitMarkers()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredMarkers));
        };
    }
    public string MarkersFilter { get; set; } = "";
    public void FilterMarkers()
    {
        OnPropertyChanged(nameof(FilteredMarkers));
    }
    public ObservableCollection<Marker> FilteredMarkers
    {
        get
        {
            if (AppState.Main.Current != null)
            {
                var markers = AppState.Main.Current.Map.Markers;
                if (string.IsNullOrEmpty(MarkersFilter))
                {
                    return new ObservableCollection<Marker>(markers);
                }
                else
                {
                    return new ObservableCollection<Marker>(markers.FindAll(r => r.Filter(MarkersFilter)));
                }
            }
            return [];
        }
    }

}
