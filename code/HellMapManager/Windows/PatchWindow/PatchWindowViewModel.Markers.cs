using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitMarkers()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredMarkers));
        };
    }
    public string MarkersFilter { get; set; } = "";
    public void FilterMarkers()
    {
        OnPropertyChanged(nameof(FilteredMarkers));
    }
    public ObservableCollection<PatchItem> FilteredMarkers
    {
        get
        {
            var markers = Patch.Markers.Items;
            if (string.IsNullOrEmpty(MarkersFilter))
            {
                return new ObservableCollection<PatchItem>(markers);
            }
            return new ObservableCollection<PatchItem>(markers.FindAll(r =>
            {
                if (r.Display is MarkerDiff rd)
                {
                    var model = rd.Model;
                    if (model != null)
                    {
                        return model.Filter(MarkersFilter);
                    }
                }
                return false;
            }));
        }
    }
    
}
