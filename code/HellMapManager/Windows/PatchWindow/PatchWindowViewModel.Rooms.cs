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
            var rooms = Patch.Rooms.Items;
            if (string.IsNullOrEmpty(RoomsFilter))
            {
                return new ObservableCollection<PatchItem>(rooms);
            }
            return new ObservableCollection<PatchItem>(rooms.FindAll(r =>
            {
                if (r.Display is RoomDiff rd)
                {
                    var model = rd.Model;
                    if (model != null)
                    {
                        return model.Filter(RoomsFilter);
                    }
                }
                return false;
            }));
        }
    }
    
}
