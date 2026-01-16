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
                var models = AppKernel.MapDatabase.Current.Map.Variables;
                if (!string.IsNullOrEmpty(VariablesFilter))
                {
                    HellMapManager.Helpers.FilterHelper.ParseKeywords(VariablesFilter).ForEach(filter =>
                    {
                        models = models.FindAll(r => r.Filter(filter));
                    });
                }
                return new ObservableCollection<Variable>(models);
            }
            return [];
        }
    }
}
