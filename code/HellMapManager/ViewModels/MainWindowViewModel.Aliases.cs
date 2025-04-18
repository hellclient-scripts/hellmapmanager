using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Services;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitAliases()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredAliases));
        };
    }
    public string AliasesFilter { get; set; } = "";
    public void FilterAliases()
    {
        OnPropertyChanged(nameof(FilteredAliases));
    }
    public ObservableCollection<Alias> FilteredAliases
    {
        get
        {
            if (AppState.Main.Current != null)
            {
                var aliases = AppState.Main.Current.Map.Aliases;
                if (string.IsNullOrEmpty(AliasesFilter))
                {
                    return new ObservableCollection<Alias>(aliases);
                }
                else
                {
                    return new ObservableCollection<Alias>(aliases.FindAll(r => r.Filter(AliasesFilter)));
                }
            }
            return [];
        }
    }

}
