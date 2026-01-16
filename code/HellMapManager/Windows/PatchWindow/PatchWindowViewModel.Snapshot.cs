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
            var models = Patch.Snapshots.Items;
            if (!string.IsNullOrEmpty(SnapshotsFilter))
            {
                HellMapManager.Helpers.FilterHelper.ParseKeywords(SnapshotsFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is SnapshotDiff rd)
                        {
                            var model = rd.Model;
                            if (model != null)
                            {
                                return model.Filter(filter);
                            }
                        }
                        return false;

                    });
                });
            }
            return new ObservableCollection<PatchItem>(models);
        }
    }
    
}
