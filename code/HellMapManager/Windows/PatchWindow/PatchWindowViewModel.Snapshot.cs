using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitSnapshots()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredSnapshots));
        };
    }
    public string SnapshotsFilter { get; set; } = "";
    public void FilterSnapshots()
    {
        OnPropertyChanged(nameof(FilteredSnapshots));
    }
    public ObservableCollection<PatchItem> FilteredSnapshots
    {
        get
        {
            var rooms = Patch.Snapshots.Items;
            if (string.IsNullOrEmpty(SnapshotsFilter))
            {
                return new ObservableCollection<PatchItem>(rooms);
            }
            return new ObservableCollection<PatchItem>(rooms.FindAll(r =>
            {
                if (r.Display is SnapshotDiff rd)
                {
                    var model = rd.Model;
                    if (model != null)
                    {
                        return model.Filter(SnapshotsFilter);
                    }
                }
                return false;
            }));
        }
    }
    
}
