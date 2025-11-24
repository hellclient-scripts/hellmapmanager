using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitShortcuts()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredShortcuts));
        };
    }
    public string ShortcutsFilter { get; set; } = "";
    public void FilterShortcuts()
    {
        OnPropertyChanged(nameof(FilteredShortcuts));
    }
    public ObservableCollection<PatchItem> FilteredShortcuts
    {
        get
        {
            var trace = Patch.Shortcuts.Items;
            if (string.IsNullOrEmpty(ShortcutsFilter))
            {
                return new ObservableCollection<PatchItem>(trace);
            }
            return new ObservableCollection<PatchItem>(trace.FindAll(r =>
            {
                if (r.Display is ShortcutDiff rd)
                {
                    var model = rd.Model;
                    if (model != null)
                    {
                        return model.Filter(ShortcutsFilter);
                    }
                }
                return false;
            }));
        }
    }
    
}
