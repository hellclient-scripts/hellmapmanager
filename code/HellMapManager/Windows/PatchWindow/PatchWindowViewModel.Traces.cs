using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitTraces()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredTraces));
        };
    }
    public string TracesFilter { get; set; } = "";
    public void FilterTraces()
    {
        OnPropertyChanged(nameof(FilteredTraces));
    }
    public ObservableCollection<PatchItem> FilteredTraces
    {
        get
        {
            var models = Patch.Traces.Items;
            if (!string.IsNullOrEmpty(TracesFilter))
            {
                HellMapManager.Helpers.FilterHelper.ParseKeywords(TracesFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is TraceDiff rd)
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
