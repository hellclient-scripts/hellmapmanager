using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitSnapshots()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
            if (AppKernel.MapDatabase.Current != null)
            {
                var models = AppKernel.MapDatabase.Current.Map.Snapshots;
                if (!string.IsNullOrEmpty(SnapshotsFilter))
                {
                    HellMapManager.Utils.FilterUtil.SplitFilter(SnapshotsFilter).ForEach(filter =>
                    {
                        models = models.FindAll(r => r.Filter(filter));
                    });
                }
                return new ObservableCollection<Snapshot>(models);
            }
            return [];
        }
    }
}
