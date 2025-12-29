using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitRooms()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredRooms));
        };
    }
    public string RoomsFilter { get; set; } = "";
    public void FilterRooms()
    {
        OnPropertyChanged(nameof(FilteredRooms));
    }
    public ObservableCollection<PatchItem> FilteredRooms
    {
        get
        {
            var models = Patch.Rooms.Items;
            if (!string.IsNullOrEmpty(RoomsFilter))
            {
                HellMapManager.Utils.FilterUtil.SplitFilter(RoomsFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is RoomDiff rd)
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
