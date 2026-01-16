using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitLandmarks()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredLandmarks));
        };
    }
    public string LandmarksFilter { get; set; } = "";
    public void FilterLandmarks()
    {
        OnPropertyChanged(nameof(FilteredLandmarks));
    }
    public ObservableCollection<PatchItem> FilteredLandmarks
    {
        get
        {
            var models = Patch.Landmarks.Items;
            if (!string.IsNullOrEmpty(LandmarksFilter))
            {
                HellMapManager.Helpers.FilterHelper.ParseKeywords(LandmarksFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is LandmarkDiff rd)
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
