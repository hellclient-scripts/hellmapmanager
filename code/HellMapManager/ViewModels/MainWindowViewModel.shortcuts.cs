using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitShortcuts()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredShortcuts));
        };
    }
    public string ShortcutsFilter { get; set; } = "";
    public void FilterShortcuts()
    {
        OnPropertyChanged(nameof(FilteredShortcuts));
    }
    public ObservableCollection<Shortcut> FilteredShortcuts
    {
        get
        {
            if (AppState.Main.Current != null)
            {
                var traces = AppState.Main.Current.Map.Shortcuts;
                if (string.IsNullOrEmpty(ShortcutsFilter))
                {
                    return new ObservableCollection<Shortcut>(traces);
                }
                else
                {
                    return new ObservableCollection<Shortcut>(traces.FindAll(r => r.Filter(ShortcutsFilter)));
                }
            }
            return [];
        }
    }
}
