using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitTraces()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(FilteredTraces));
        };
    }
    public string TracesFilter { get; set; } = "";
    public void FilterTraces()
    {
        OnPropertyChanged(nameof(FilteredTraces));
    }
    public ObservableCollection<Trace> FilteredTraces
    {
        get
        {
            if (AppState.Main.Current != null)
            {
                var traces = AppState.Main.Current.Map.Traces;
                if (string.IsNullOrEmpty(TracesFilter))
                {
                    return new ObservableCollection<Trace>(traces);
                }
                else
                {
                    return new ObservableCollection<Trace>(traces.FindAll(r => r.Filter(TracesFilter)));
                }
            }
            return [];
        }
    }

}
