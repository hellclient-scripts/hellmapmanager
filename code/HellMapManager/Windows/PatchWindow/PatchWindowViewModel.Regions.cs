using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitRegions()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredRegions));
        };
    }
    public string RegionsFilter { get; set; } = "";
    public void FilterRegions()
    {
        OnPropertyChanged(nameof(FilteredRegions));
    }
    public ObservableCollection<PatchItem> FilteredRegions
    {
        get
        {
            var rooms = Patch.Regions.Items;
            if (string.IsNullOrEmpty(RegionsFilter))
            {
                return new ObservableCollection<PatchItem>(rooms);
            }
            return new ObservableCollection<PatchItem>(rooms.FindAll(r =>
            {
                if (r.Display is RegionDiff rd)
                {
                    var model = rd.Model;
                    if (model != null)
                    {
                        return model.Filter(RegionsFilter);
                    }
                }
                return false;
            }));
        }
    }
    
}
