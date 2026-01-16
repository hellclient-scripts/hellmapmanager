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
            var models = Patch.Regions.Items;
            if (!string.IsNullOrEmpty(RegionsFilter))
            {
                HellMapManager.Helpers.FilterHelper.ParseKeywords(RegionsFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is RegionDiff rd)
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
