using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Services;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitVariables()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredVariables));
        };
    }
    public string VariablesFilter { get; set; } = "";
    public void FilterVariables()
    {
        OnPropertyChanged(nameof(FilteredVariables));
    }
    public ObservableCollection<Variable> FilteredVariables
    {
        get
        {
            if (AppState.Main.Current != null)
            {
                var traces = AppState.Main.Current.Map.Variables;
                if (string.IsNullOrEmpty(VariablesFilter))
                {
                    return new ObservableCollection<Variable>(traces);
                }
                else
                {
                    return new ObservableCollection<Variable>(traces.FindAll(r => r.Filter(VariablesFilter)));
                }
            }
            return [];
        }
    }
}
