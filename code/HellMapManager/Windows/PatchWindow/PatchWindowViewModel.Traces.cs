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
            var trace = Patch.Traces.Items;
            if (string.IsNullOrEmpty(TracesFilter))
            {
                return new ObservableCollection<PatchItem>(trace);
            }
            return new ObservableCollection<PatchItem>(trace.FindAll(r =>
            {
                if (r.Display is TraceDiff rd)
                {
                    var model = rd.Model;
                    if (model != null)
                    {
                        return model.Filter(TracesFilter);
                    }
                }
                return false;
            }));
        }
    }
    
}
