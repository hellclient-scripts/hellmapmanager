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
            var models = Patch.Shortcuts.Items;
            if (!string.IsNullOrEmpty(ShortcutsFilter))
            {
                HellMapManager.Utils.FilterUtil.SplitFilter(ShortcutsFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is ShortcutDiff rd)
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
