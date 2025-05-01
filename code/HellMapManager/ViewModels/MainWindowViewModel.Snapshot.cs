using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitSnapshots()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredSnapshots));
        };
    }
    public string SnapshotsFilter { get; set; } = "";
    public void FilterSnapshots()
    {
        OnPropertyChanged(nameof(FilteredSnapshots));
    }
    public ObservableCollection<Snapshot> FilteredSnapshots
    {
        get
        {
            if (AppState.Main.Current != null)
            {
                var traces = AppState.Main.Current.Map.Snapshots;
                if (string.IsNullOrEmpty(SnapshotsFilter))
                {
                    return new ObservableCollection<Snapshot>(traces);
                }
                else
                {
                    return new ObservableCollection<Snapshot>(traces.FindAll(r => r.Filter(SnapshotsFilter)));
                }
            }
            return [];
        }
    }
}
