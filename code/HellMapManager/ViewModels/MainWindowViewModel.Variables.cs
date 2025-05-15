using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitVariables()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
            if (AppKernel.MapDatabase.Current != null)
            {
                var traces = AppKernel.MapDatabase.Current.Map.Variables;
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
