using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public partial void InitTraces()
    {
        AppKernel.Instance.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
            if (AppKernel.Instance.MapDatabase.Current != null)
            {
                var traces = AppKernel.Instance.MapDatabase.Current.Map.Traces;
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
